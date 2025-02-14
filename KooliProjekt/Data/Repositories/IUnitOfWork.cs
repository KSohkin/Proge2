namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IOrganizerRepository OrganizerRepository { get; }
        IEventRepository EventRepository { get; }
        IRegisteringRepository RegisteringRepository { get; }

    }
}