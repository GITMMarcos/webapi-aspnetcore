using A1WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace A1WebAPI.Utils.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        //setup da classe formatador de Output de CSV
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            // Realizada essa validação para que não seja lançada uma exceção na linha 31 ou 44, ou seja,
            // caso o tipo de retorno seja uma string, é porque no controller a respostafoi diferente de OK (http 200)
            if (context.ObjectType == typeof(string))
                return response.WriteAsync(context.Object.ToString());

            var tarefas = (IEnumerable<Tarefa>)context.Object;

            if (tarefas != null)
            {
                foreach (var item in tarefas)
                {
                    FormatOutput(buffer, item);
                }

                return response.WriteAsync(buffer.ToString());
            }

            var tarefa = context.Object as Tarefa;

            FormatOutput(buffer, tarefa);

            return response.WriteAsync(buffer.ToString());

        }

        private void FormatOutput(StringBuilder buffer, Tarefa item)
        {
            buffer.AppendLine($"{item.Id};{item.Nome};{item.isFinalizado}");
        }
    }
}
