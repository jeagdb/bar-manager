using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

using BM = BarManagement.DataAccess;

[assembly: TestFramework("barManagementTests.Startup", "AssemblyName")]
namespace barManagementTests
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<BM.Interfaces.ICocktailsRepository, BM.CocktailsRepository>();
            services.AddTransient<BM.Interfaces.ICocktailsCompositionRepository, BM.CocktailsCompositionRepository>();
            services.AddTransient<BM.Interfaces.IStocksRepository, BM.StocksRepository>();
            services.AddTransient<BM.Interfaces.ITransactionsRepository, BM.TransactionsRepository>();
        }

        protected override IHostBuilder CreateHostBuilder(AssemblyName assemblyName) =>
            base.CreateHostBuilder(assemblyName)
                .ConfigureServices(ConfigureServices);
    }
}
