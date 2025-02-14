namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        IClientRepository ClientRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IOrganizerRepository OrganizerRepository { get; }
        IEventRepository EventRepository { get; }
        IRegisteringRepository RegisteringRepository { get; }

    }
}