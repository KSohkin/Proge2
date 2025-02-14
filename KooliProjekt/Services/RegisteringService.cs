using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class RegisteringService : IRegisteringService
    {
        private readonly IUnitOfWork _uof;

        public RegisteringService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.RegisteringRepository.Delete(Id);
        }

        public async Task<Registering> Get(int? Id)
        {
            return await _uof.RegisteringRepository.Get((int)Id);
        }

        public async Task<PagedResult<Registering>> List(int page, int pageSize)
        {
            return await _uof.RegisteringRepository.List(page, pageSize);
        }

        public async Task Save(Registering registering)
        {
            await _uof.RegisteringRepository.Save(registering);
        }
    }
}