
namespace Delivora.Repositories.UnitOfWorks;

public class UnitOfWorks
{
    public readonly DeliveryContext _context;

    private DelivoraGenericRepository<Admin> _adminRepository;
    private DelivoraGenericRepository<Customer>? _customerRepository;
    private DelivoraGenericRepository<Driver>? _driverRepository;

    private DelivoraGenericRepository<Address>? _addressRepository;

    private DelivoraGenericRepository<Order>? _orderRepository;
    private DelivoraGenericRepository<OrderItem>? _orderItemsRepository;

    private DelivoraGenericRepository<Food>? _foodRepository;
    private DelivoraGenericRepository<Category>? _categoryRepository;

    private DelivoraGenericRepository<Payment>? _paymentRepository;
    private DelivoraGenericRepository<PaymentMethod>? _paymentMethodRepository;


    private DelivoraGenericRepository<OrderReview>? _orderReviewRepository;
    private DelivoraGenericRepository<FoodReview>? _foodReviewRepository;

    

    public UnitOfWorks(DeliveryContext context)
    {
        _context = context;
    }


    #region Users
    public DelivoraGenericRepository<Admin> AdminRepository
    {
        get
        {
            if (_adminRepository == null)
            {
                _adminRepository = new DelivoraGenericRepository<Admin>(_context);
            }
            return _adminRepository;
        }
    }

    public DelivoraGenericRepository<Customer> CustomerRepository
    {
        get
        {
            if (_customerRepository == null)
            {
                _customerRepository = new DelivoraGenericRepository<Customer>(_context);
            }
            return _customerRepository;
        }
    }

    public DelivoraGenericRepository<Driver> DriverRepository
    {
        get
        {
            if (_driverRepository == null)
            {
                _driverRepository = new DelivoraGenericRepository<Driver>(_context);
            }
            return _driverRepository;
        }
    }
    #endregion


    public DelivoraGenericRepository<Address> AddressRepository
    {
        get
        {
            if (_addressRepository == null)
            {
                _addressRepository = new DelivoraGenericRepository<Address>(_context);
            }
            return _addressRepository;
        }
    }



    public DelivoraGenericRepository<Order> OrderRepository
    {
        get
        {
            if (_orderRepository == null)
            {
                _orderRepository = new DelivoraGenericRepository<Order>(_context);
            }
            return _orderRepository;
        }
    }

    public DelivoraGenericRepository<OrderItem> OrderItemsRepository
    {
        get
        {
            if (_orderItemsRepository == null)
            {
                _orderItemsRepository = new DelivoraGenericRepository<OrderItem>(_context);
            }
            return _orderItemsRepository;
        }
    }




    public DelivoraGenericRepository<Food> FoodRepository
    {
        get
        {
            if (_foodRepository == null)
            {
                _foodRepository = new DelivoraGenericRepository<Food>(_context);
            }
            return _foodRepository;
        }
    }

    public DelivoraGenericRepository<Category> CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new DelivoraGenericRepository<Category>(_context);
            }
            return _categoryRepository;
        }
    }




    public DelivoraGenericRepository<Payment> PaymentRepository
    {
        get
        {
            if (_paymentRepository == null)
            {
                _paymentRepository = new DelivoraGenericRepository<Payment>(_context);
            }
            return _paymentRepository;
        }
    }

    public DelivoraGenericRepository<PaymentMethod> PaymentMethodRepository
    {
        get
        {
            if (_paymentMethodRepository == null)
            {
                _paymentMethodRepository = new DelivoraGenericRepository<PaymentMethod>(_context);
            }
            return _paymentMethodRepository;
        }
    }




    public DelivoraGenericRepository<OrderReview> OrderReviewRepository
    {
        get
        {
            if (_orderReviewRepository == null)
            {
                _orderReviewRepository = new DelivoraGenericRepository<OrderReview>(_context);
            }
            return _orderReviewRepository;
        }
    }

    public DelivoraGenericRepository<FoodReview> FoodReviewRepository
    {
        get
        {
            if (_foodReviewRepository == null)
            {
                _foodReviewRepository = new DelivoraGenericRepository<FoodReview>(_context);
            }
            return _foodReviewRepository;
        }
    }




    // ========================= Save Changes =========================
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}
