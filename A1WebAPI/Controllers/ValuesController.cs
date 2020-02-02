using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace A1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {
                "Maurício Marcos",
                "Juliana de Campos Marcos",
                "Anthony de Campos Marcos"
            };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return string.Format("Retono request com id - {0}", id.ToString());
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return string.Format("O registro {0} foi deletado com sucesso.", id.ToString());
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] string request)
        {
            return request;
        }

        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string request)
        {
            return string.Format("O valor recuperado da URL é: {0}. O valor recuperado do corpo da requisição é: {1}", id.ToString(), request);
        }

    }
}
