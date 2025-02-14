namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context,
        IClientRepository clientRepository,
        IPaymentRepository paymentRepository,
        IRegisteringRepository registeringRepository,
        IEventRepository eventRepository,
        IOrganizerRepository organizerRepository)

        {
            _context = context;

            ClientRepository = clientRepository;
            PaymentRepository = paymentRepository;
            RegisteringRepository = registeringRepository;
            EventRepository = eventRepository;
            OrganizerRepository = organizerRepository;
        }

        public IClientRepository ClientRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IRegisteringRepository RegisteringRepository { get; }
        public IEventRepository EventRepository { get; }
        public IOrganizerRepository OrganizerRepository { get; }



        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}