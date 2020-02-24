using System;
using System.Collections.Generic;
using System.Linq;
using A1WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace A1WebAPI.Controllers
{
    /// <summary>
    /// Serviço responsável pelas ações relacionadas às Tarefas da Aplicação
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        // Propriedade para armazenar o objeto injetado no construtor do MemoryCache
        private readonly IMemoryCache _memoryCache;

        public TarefaController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary> Retorna a lista com todas as tarefas.</summary>
        /// <response code="200">Lista todas as tarefas.</response>
        /// <returns>Lista de tarefas</returns>
        [FormatFilter]
        [HttpGet]
        //[Produces("application/xml")] //Força o método da API retornar o conteúdo no formato XML
        [ProducesResponseType(typeof(IEnumerable<Tarefa>), StatusCodes.Status200OK)]
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


        /// <summary>Retorna uma tarefa correspondente ao id selecionado.</summary>
        /// <response code="400">Quando o id for negativo.</response>
        /// <response code="204">Quando não for identificada a tarefa a partir do id.</response>
        /// <response code="200">Quando a tarefa for localizada a partir do id.</response>
        /// <param name="id">Identificador único da Tarefa.</param>
        /// <returns></returns>
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

            // Aqui é criado ou obtido o cache a partir do seu ID. Caso ainda esteja em cache (o tempo que o cache ficará disponível está na configuração: entry.SlidingExpiration = TimeSpan.FromSeconds(30);)
            // Uma vez que não esteja em cache, a linha 104 será executada (pensando em uma aplicação que acessaria um repositório de dados, então a chamada ao repositório só ocorreria quando o cache não existir ou tiver expirado)
            var cache = _memoryCache.GetOrCreate($"tarefa-{id}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(30);
                return Ok(tarefaResultado);
            });


            if (tarefaResultado == null) return NoContent();

            return Ok(cache);
        }
    }
}