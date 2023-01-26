using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoWebAPIApp.Data;

namespace MongoWebAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly MongoDbService _service;
        // GET: api/Books
        public BooksController(MongoDbService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<Book>> Get()
        {
            return await _service.GetAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id:length(24)}")]
        public async Task<string> Get(string id)
        {
            //return await _service.FindById(id);
            return await _service.GetTitle(id);
        }

        // POST: api/Books
        [HttpPost]
        public void Post([FromBody] Book book)
        {
            _service.CreateAsync(book);
        }

        // PUT: api/Books/5
        [HttpPut("{id:length(24)}")]
        public void Put(string id, [FromBody] Book value)
        {
            if (id == value.Id)
            {
                _service.Update(id, value);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id:length(24)}")]
        public void Delete(string id)
        {
            
        }
    }
}
