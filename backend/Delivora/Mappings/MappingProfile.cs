
namespace Delivora.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // For Category
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.FoodList,
                opt => opt.MapFrom(s => s.Foods.Select(f => f.Name).ToList()));
        
        CreateMap<CreateCategoryDto, Category>()
            .ForMember(d => d.ImageUrl, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Foods, opt => opt.Ignore());
        

        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(d => d.ImageUrl, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Foods, opt => opt.Ignore());



        // For Food
        CreateMap<Food, FoodDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name)
            );

        CreateMap<CreateFoodDto, Food>()
            .ForMember(d => d.ImageUrl, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Category, opt => opt.Ignore());
        
        CreateMap<UpdateFoodDto, Food>()
            .ForMember(d => d.ImageUrl, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Category, opt => opt.Ignore());



        // For Address
        CreateMap<Address, AddressDto>();
        CreateMap<CreateAddressDto, Address>();
        CreateMap<UpdateAddressDto, Address>();


        // For Order
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.CustomerName,
                opt => opt.MapFrom(s => s.Customer.User.FullName))
            .ForMember(d => d.DriverName,
                opt => opt.MapFrom(s => s.Driver!.User.FullName))
            .ForMember(d => d.AddressTitle,
                opt => opt.MapFrom(s => s.Address.Title))
            .ForMember(d => d.Items,
                opt => opt.MapFrom(s => s.OrderItems));

        // For OrderItem
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.FoodName,
                opt => opt.MapFrom(s => s.Food.Name));


        // For Payment
        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.MethodName,
                opt => opt.MapFrom(s => s.PaymentMethod.Name.ToString()));

        // For PaymentMethod
        CreateMap<PaymentMethod, PaymentMethodDto>();
        CreateMap<CreatePaymentMethodDto, PaymentMethod>();


        // For Order Reviews
        CreateMap<OrderReview, OrderReviewDto>()
            .ForMember(d => d.CustomerName,
                opt => opt.MapFrom(s => s.Customer.User.FullName));
        
        CreateMap<CreateOrderReviewDto, OrderReview>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CustomerId, opt => opt.Ignore());


        // For Food Reviews
        CreateMap<FoodReview, FoodReviewDto>()
            .ForMember(d => d.CustomerName,
                opt => opt.MapFrom(s => s.Customer.User.FullName))
            .ForMember(d => d.FoodName,
                opt => opt.MapFrom(s => s.Food.Name));
        
        CreateMap<CreateFoodReviewDto, FoodReview>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.LastUpdate,
                opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CustomerId, opt => opt.Ignore());



        // For Driver
        CreateMap<Driver, DriverDto>()
            .ForMember(d => d.FullName,
                opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.Email,
                opt => opt.MapFrom(s => s.User.Email))
            .ForMember(d => d.Phone,
                opt => opt.MapFrom(s => s.User.PhoneNumber));

    }
}
