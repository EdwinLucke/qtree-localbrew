using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qtree.core.common;

namespace qtree.core.website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyFirstController : ControllerBase
    {
        /// <summary>
        /// Samenvatting
        /// </summary>
        /// <returns>200 | OK ; the data is returned in the data object</returns>
        // GET: api/MyFirst
        [HttpGet]
        public ObjectResult Get()
        {
            // ApiResponse.
            return Ok(ApiResponse.DataResponse(new string[] { "value1", "value2" }));
            //return new string[] { "value1", "value2" };
        }

        // GET: api/MyFirst/5
        [HttpGet("{id}", Name = "Get")]
        public ObjectResult Get(int id)
        {
            try
            {
                if (id == 2) throw new EntryPointNotFoundException();
                return Ok(ApiResponse.DataResponse($"ID value={id}"));
            }catch(Exception ex)
            {
                return Ok(ApiResponse.Error(ex, $"ID value={id}"));
            }
        }

        // POST: api/MyFirst
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MyFirst/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
