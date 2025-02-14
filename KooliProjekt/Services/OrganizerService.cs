using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IUnitOfWork _uof;

        public OrganizerService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.OrganizerRepository.Delete(Id);
        }

        public async Task<Organizer> Get(int? Id)
        {
            return await _uof.OrganizerRepository.Get((int)Id);
        }

        public async Task<PagedResult<Organizer>> List(int page, int pageSize)
        {
            return await _uof.OrganizerRepository.List(page, pageSize);
        }

        public async Task Save(Organizer organizer)
        {
            await _uof.OrganizerRepository.Save(organizer);
        }
    }
}