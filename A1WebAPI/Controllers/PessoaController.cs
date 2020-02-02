using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A1WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A1WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private static IList<Pessoa> _listaPessoas = new List<Pessoa>();

        public PessoaController()
        {
            CarregaElementosDaBaseEmMemoria();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pessoa>> GetAll()
        {
            return _listaPessoas.ToList();
        }

        [HttpGet("{age:int}")]
        public ActionResult<Pessoa> GetByAge(int age)
        {
            return _listaPessoas.FirstOrDefault(p => p.Idade == age);
        }

        [HttpPost]
        public ActionResult<Pessoa> Post([FromBody] Pessoa pessoa)
        {
            _listaPessoas.Add(pessoa);

            return pessoa;
        }

        [HttpPut("{age:int}")]
        public ActionResult<Pessoa> Put(int age, [FromBody] Pessoa pessoa)
        {
            var pessoaBaseDados = _listaPessoas.FirstOrDefault(p => p.Idade == age);

            if (pessoaBaseDados != null)
            {
                _listaPessoas.Remove(pessoaBaseDados);

                pessoaBaseDados.Nome = pessoa.Nome;
                pessoaBaseDados.Idade = pessoa.Idade;

                _listaPessoas.Add(pessoaBaseDados);
            }

            return pessoaBaseDados;
        }

        [HttpDelete("{age:int}")]
        public ActionResult<int> Delete(int age)
        {
            var pessoaBaseDados = _listaPessoas.FirstOrDefault(p => p.Idade == age);

            if (pessoaBaseDados != null)
            {
                _listaPessoas.Remove(pessoaBaseDados);
            }

            return age; 
        }

        // Em APIs Restfull o verbo Patch é utilizado para atualizar parcialmente um objeto.
        [HttpPatch("{age:int}/{name}")]
        public ActionResult<Pessoa> Patch(int age, string name)
        {
            Pessoa pessoa = new Pessoa()
            {
                Nome = name,
                Idade = age
            };

            return pessoa;
        } 

        private void CarregaElementosDaBaseEmMemoria()
        {
            if (_listaPessoas.Count == 0)
            {
                  _listaPessoas.Add(
                  new Pessoa()
                  {
                      Nome = "Maurício Marcos",
                      Idade = 34
                  });

                _listaPessoas.Add(
                    new Pessoa()
                    {
                        Nome = "Anthony de Campos Marcos",
                        Idade = 2
                    });

                _listaPessoas.Add(
                    new Pessoa()
                    {
                        Nome = "Juliana de Campos Marcos",
                        Idade = 33
                    });
            }
        }
    }
}