
using Delivora.Mappings;

namespace Delivora;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // Configure the database connection string
        var connectionString = builder.Configuration.GetConnectionString("Delivery");

        builder.Services.AddDbContext<DeliveryContext>(options =>
            options.UseSqlServer(connectionString));


        // Add Identity
        builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<DeliveryContext>()
            .AddDefaultTokenProviders(); // for generating tokens for password reset, email confirmation, change email.
        
        // Add Jwt Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero // Remove default 5 minutes clock skew
            };
        });


        // Add Services
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<UnitOfWorks>();
        builder.Services.AddScoped<IFileService, FileService>();

        builder.Services.AddAutoMapper(op => op.AddProfile<MappingProfile>());


        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAngular", policy =>
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
        });
        

        var app = builder.Build();


        // Apply migrations and seed data
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DeliveryContext>();
            await db.Database.MigrateAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            foreach (var role in new[] { "Admin", "Customer", "Driver" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }

            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (!adminUsers.Any())
            {
                var adminUsername = builder.Configuration["DefaultAdmin:Username"] ?? "admin";
                var adminEmail = builder.Configuration["DefaultAdmin:Email"] ?? "admin@delivora.local";
                var admin = await userManager.FindByNameAsync(adminUsername)
                    ?? await userManager.FindByEmailAsync(adminEmail);

                if (admin is null)
                {
                    admin = new AppUser
                    {
                        UserName = adminUsername,
                        Email = adminEmail,
                        FullName = builder.Configuration["DefaultAdmin:FullName"] ?? "System Admin",
                        PhoneNumber = builder.Configuration["DefaultAdmin:PhoneNumber"],
                        Status = UserStatus.Active,
                        BirthDate = DateTime.Today
                    };

                    var password = builder.Configuration["DefaultAdmin:Password"] ?? "Admin12345";
                    var result = await userManager.CreateAsync(admin, password);
                    if (!result.Succeeded)
                    {
                        admin = null;
                    }
                }

                if (admin is not null)
                {
                    admin.Status = UserStatus.Active;
                    if (!await userManager.IsInRoleAsync(admin, "Admin"))
                        await userManager.AddToRoleAsync(admin, "Admin");

                    if (!await db.Admins.AnyAsync(a => a.UserId == admin.Id))
                        db.Admins.Add(new Admin { UserId = admin.Id });

                    await db.SaveChangesAsync();
                }
            }

            if (!await db.PaymentMethods.AnyAsync())
            {
                db.PaymentMethods.AddRange(
                    new PaymentMethod { Name = PaymentMethodsName.CashOnDelivery, Description = "Pay cash on delivery", IsActive = true },
                    new PaymentMethod { Name = PaymentMethodsName.Card, Description = "Credit / debit card", IsActive = true },
                    new PaymentMethod { Name = PaymentMethodsName.Wallet, Description = "Mobile wallet", IsActive = true }
                );
                await db.SaveChangesAsync();
            }
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAngular");

        app.UseAuthentication();

        app.Use(async (context, next) =>
        {
            var unsafeMethod = HttpMethods.IsPost(context.Request.Method)
                || HttpMethods.IsPut(context.Request.Method)
                || HttpMethods.IsPatch(context.Request.Method)
                || HttpMethods.IsDelete(context.Request.Method);

            if (unsafeMethod && context.User.Identity?.IsAuthenticated == true && !context.User.IsInRole("Admin"))
            {
                var userIdValue = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdValue, out var userId))
                {
                    var db = context.RequestServices.GetRequiredService<DeliveryContext>();
                    var status = await db.Users
                        .Where(u => u.Id == userId)
                        .Select(u => u.Status)
                        .FirstOrDefaultAsync();

                    if (status == UserStatus.Suspended)
                    {
                        context.Response.StatusCode = StatusCodes.Status423Locked;
                        await context.Response.WriteAsJsonAsync(new { message = "Your account is locked. You can view your account, but actions are disabled." });
                        return;
                    }
                }
            }
            
            await next();
        });

        app.UseAuthorization();

        app.UseStaticFiles();
        app.MapControllers();

        app.Run();
    }
}
