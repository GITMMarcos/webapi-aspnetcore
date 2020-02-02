using System;
using System.Collections.Generic;
using A1WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace A1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Tarefa>> Get()
        {
            var rdn = new Random();
            var max = rdn.Next(100);

            var listaTarefa = new List<Tarefa>();

            for (int i = 1; i <= max; i++)
            {
                var tarefa = new Tarefa()
                {
                    Id = i,
                    Nome = $"Tarefa com a descrião: {i}",
                    isFinalizado = (i % 2 == 0) ? true : false
                };

                listaTarefa.Add(tarefa);
            }

            if (listaTarefa == null)
            {
                return NoContent();
            }
            if (listaTarefa.Count > 50)
            {
                return BadRequest($"Quantidade de tarefas excedo o valor máximo: {listaTarefa.Count}");
            }

            return Ok(listaTarefa);
        }
    }
}