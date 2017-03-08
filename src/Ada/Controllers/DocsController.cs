using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ada.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Ada.Controllers
{
    [Route("api/[controller]")]
    public class DocsController : Controller
    {
        private IDocumentService _service;

        public DocsController(IDocumentService service)
        {
            _service = service;
        }


        [HttpGet("generate")]
        public IActionResult Generate()
        {
            try
            {
                _service.ProcessDocuments();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Success");
        }


        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
