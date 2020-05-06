using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BarManagement.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BarManagement
{
    public class Startup
    {
        private string _connectionString;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddEntityFrameworkSqlServer()
                        .AddDbContext<DataAccess.EfModels.barDBContext>(options => options.UseSqlServer(_connectionString));

            services.AddAutoMapper(typeof(DataAccess.AutomapperProfiles));
            services.AddRazorPages();
            services.AddControllers();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddMemoryCache();
            services.AddMvc()
                .AddSessionStateTempDataProvider();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<DataAccess.Interfaces.IDrinksRepository, DrinksRepository>();
            services.AddTransient<DataAccess.Interfaces.IStocksRepository, StocksRepository>();
            services.AddTransient<DataAccess.Interfaces.ICocktailsRepository, CocktailsRepository>();
            services.AddTransient<DataAccess.Interfaces.ICocktailsCompositionRepository, CocktailsCompositionRepository>();
            services.AddTransient<DataAccess.Interfaces.ITransactionsRepository, TransactionsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _connectionString = Configuration.GetConnectionString("barDB");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
