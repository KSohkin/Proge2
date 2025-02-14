using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ClientsService : IClientService
    {
        private readonly IUnitOfWork _uof;

        public ClientsService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.ClientRepository.Delete(Id);
        }

        public async Task<Client> Get(int? Id)
        {
            return await _uof.ClientRepository.Get((int)Id);
        }

        public async Task<PagedResult<Client>> List(int page, int pageSize)
        {
            return await _uof.ClientRepository.List(page, pageSize);
        }

        public async Task Save(Client client)
        {
            await _uof.ClientRepository.Save(client);
        }
    }
}