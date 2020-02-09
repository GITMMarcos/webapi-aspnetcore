using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using A1WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        [FormatFilter]
        [HttpGet]
        //[Produces("application/xml")] //Força o método da API retornar o conteúdo no formato XML
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


        [HttpGet("{id:int}.{format?}")] // Possibilita que o cliente informe o tipo de retorno na própria URI: EX.: https://localhost:44311/api/tarefa/2.xml
        //[Route("{id:int}.{format?}")] // A rota pode ser definida também diretamente na decoração do verbo http. Ex.: [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Tarefa> Get(int id)
        {

            //Recuperando valores do Header: Keys usuario e senha
            var headerUsuario = Request.Headers.FirstOrDefault(h => h.Key == "usuario").Value;
            var headerSenha = Request.Headers.FirstOrDefault(h => h.Key == "senha").Value;

            if (id < 0) return BadRequest($"Objeto não localizado: {id}");

            var lista = new List<Tarefa>();

            for (int i = 0; i < 10; i++)
            {
                var tarefa = new Tarefa()
                {
                    Id = i,
                    Nome = $"Tarefa id: {i}",
                    isFinalizado = (i % 2 == 0) ? true : false
                };

                lista.Add(tarefa);
            }

            var tarefaResultado = lista.FirstOrDefault(t => t.Id == id);
            if (tarefaResultado == null) return NoContent();

            return Ok(tarefaResultado);
        }
    }
}