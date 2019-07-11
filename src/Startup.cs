using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using myrestful.Infrastructure;
using myrestful.Services;
using myrestful.Repositories;

namespace myrestful
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
            string connectionString = Configuration.GetConnectionString("DefaultDB");
            if(connectionString == null)
            {
                connectionString = @"Server=localhost;Database=myrestful;Username=learning;Password=qaz";
            }

            services.AddDbContext<DBContextEmployees>(options => options.UseNpgsql(connectionString));
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseExceptionHandler(new ExceptionHandlerOptions{
                ExceptionHandler = new JsonExceptionMiddleware().Invoke
            });

            app.UseMiddleware<BasicAuthMiddleware>();
            app.UseMvc();
        }
    }
}
