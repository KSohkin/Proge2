using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class EventsService : IEventsService
    {
        private readonly IUnitOfWork _uof;

        public EventsService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.EventRepository.Delete(Id);
        }

        public async Task<Event> Get(int? Id)
        {
            return await _uof.EventRepository.Get((int)Id);
        }

        public async Task<PagedResult<Event>> List(int page, int pageSize)
        {
            return await _uof.EventRepository.List(page, pageSize);
        }

        public async Task Save(Event @event)
        {
            await _uof.EventRepository.Save(@event);
        }
    }
}