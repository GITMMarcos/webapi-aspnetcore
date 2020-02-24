using System;
using System.IO;
using System.Reflection;
using A1WebAPI.Configs;
using A1WebAPI.Utils.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace A1WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Recuperando valores do appsetings.json por intermédio da classe auxiliar criada para representar a configuração
            var configuracao = new Configuracao();
            Configuration.GetSection("Configuracao").Bind(configuracao);

            services.AddMvc(options => {
                // Adicionado para que seja carregada a dependência no container da classe que foi criada
                // para responder na formatação de CSV: classe criada => CsvOutputFormatter
                options.OutputFormatters.Add(new CsvOutputFormatter());
            })

            // Adicionado para que seja possível retornar o resultado da chamada a API no formato XML, quando a aplicação
            // cliente solicitar no cabeçalho o valor de retorno accept : application/xml
            .AddXmlDataContractSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Comando utilizado para retornar o nome do arquivo XML A1WebAPI.xml. Esse arquivo é adicionado na raiz do projeto automaticamente,
            // uma vez que tenha alterado nas propriedades do projeto (API) > Build > Output Path.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            // Uma vez obtido o nome do arquivo de configuração, recupera-se o caminho absoluto do arquivo.
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // Realiza a injeção de dependência do Swagger no container
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("Docv1", new OpenApiInfo() { Title = "APIs do Sistema", Version = "v1"});
                config.SwaggerDoc("Docv2", new OpenApiInfo() { Title = "APIs do Sistema", Version = "v2" });
                // Esta configuração é populada a partir dos dados gerados bo arquivo obtido na variável xmlPath
                config.IncludeXmlComments(xmlPath);
            });

            // Realiza/inicializa a injeção de dependência, carregando as dependências do memory cache
            services.AddMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();


            // Add via NuGet: Swashbuckle.AspNetCore
            app.UseSwagger();
            app.UseSwaggerUI(config => {
                // config.SwaggerDoc("Docv1", new OpenApiInfo() { Title = "APIs do Sistema", Version = "v1"} a partir do nome "Docv1",
                // a configuração abaixo consegue gerar a interface para o usuário com a devida correspondência.
                config.SwaggerEndpoint("/swagger/Docv1/swagger.json", "APIs do Sistema - Endpoints Versão 1");
                config.SwaggerEndpoint("/swagger/Docv2/swagger.json", "APIs do Sistema - Endpoints Versão 2");
            });

            app.UseMvc();
        }
    }
}
