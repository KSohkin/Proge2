using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KooliProjekt.Controllers
{
    [Route("api/Clients")]
    [ApiController]
    public class ClientApiController : ControllerBase
    {
        private readonly IClientService _clientsService;

        public ClientApiController(IClientService clientsService)
        {
            _clientsService = clientsService;
        }


        // GET: api/<ClientApiController>
        [HttpGet]
        public async Task<IEnumerable<Client>> Get()
        {
            var result = await _clientsService.List(1, 10000);
            return result.Results;
        }

        // GET api/<ClientApiController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var list = await _clientsService.Get(id);
            if (list == null)
            {
                return NotFound();
            }

            return list;
        }

        // POST api/<ClientApiController>
        [HttpPost]
        public async Task<object> Post([FromBody] Client list)
        {
            await _clientsService.Save(list);

            return Ok(list);
        }

        // PUT api/<ClientApiController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Client list)
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            await _clientsService.Save(list);
            return Ok();

        }

        // DELETE api/<ClientApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var list = await (_clientsService.Get(id));
            if (list == null)
            {
                return NotFound();
            }

            await _clientsService.Delete(id);
            return Ok();
        }
    }
}