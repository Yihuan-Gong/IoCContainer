using SelfMadeContainerExample.Birds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections;
using DTO;
using System.Reflection;

namespace SelfMadeContainerExample
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            // Transit => 每一次從容器拿出都是全新的物件
            // Scope => 這整個API Request 操作的生命週期都會拿到相同的物件
            // Singleton => 無論何時何地 只要從容器拿出都是同一個物件
            //Service.AddSingleton<ABird, Sparrow>(x => x.Age = 5);

            //var feedbird = new FeedBird();
            //feedbird.Feed();
            //feedbird.SayAge();

            //Service.ExtensionCollection.Add(ServiceDescriptor.Singleton(new Sparrow { Age = 10 }));

            Dictionary<string, int> dicts = new Dictionary<string, int>();

            // ILogger`1
            ILogger log = new Logger<Service>(new LoggerFactory());
            ILoggerFactory logger = LoggerFactory.Create(x =>
            {
                x.AddNLog();
            });


            var config = CreateConfig();
            Service.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });
            Service.AddSingleton<IFood, Rice>(); 
            Service.AddTransit<ABird,Sparrow>();

            IServiceProvider sp = Service.BuildServiceProvider();

            var taskList = new List<Task<List<ABird>>>();
            for (int i = 0; i< 10; i++)
            {
                taskList.Add(GetBirdAsync(sp));
            }

            var temp = await Task.WhenAll(taskList);


            Console.ReadKey();


            //ABird sparrow = sp.GetService<ABird>();
            ////var sparrow = Service.GetInstance<ABird>();
            //sparrow.SayAge();
            //sparrow.Eat();


            //var config = CreateConfig();
            //var serviceCollection = new ServiceCollection();
            //var serviceProvider = serviceCollection
            //    .AddSingleton<Sparrow>(sp => new Sparrow(sp.GetService<ILogger<Sparrow>>(), 10))
            //    .AddLogging(loggingBuilder =>
            //    {
            //        loggingBuilder.ClearProviders();
            //        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            //        loggingBuilder.AddNLog(config);
            //    })
            //    .BuildServiceProvider();
            //var sparrow = serviceProvider.GetService<Sparrow>();
            //sparrow.SayAge();
            //sparrow.Eat();


            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton<ABird, Eagle>()
            //    .AddSingleton<ABird, Sparrow>()
            //    .BuildServiceProvider();
            //var list = serviceProvider.GetService<ABird>();


            //Service.AddSingleton<ABird, Eagle>();
            //Service.AddSingleton<ABird, Sparrow>();
            //var list2 = Service.GetInstance<IEnumerable<ABird>>();
            //var list3 = Service.GetInstance<IEnumerable<ABird>>();


            //var type = typeof(IEnumerable<Eagle>);
            //var eagle = type.GetGenericArguments()[0];

            //Type type = typeof(IEnumerable<Eagle>);
            //List<object> eagle = new List<object> { new Eagle() };

            //var list = ToSpecificType<IEnumerable<Eagle>>(eagle);


            //IEnumerable<Eagle> list = (IEnumerable<Eagle>)eagle;
            //IEnumerable<Eagle> list = eagle.OfType<Eagle>();



        }


        private static IConfiguration CreateConfig()
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", true, true)
                         .Build();
            return config;
        }

        private static async Task<List<ABird>> GetBirdAsync(IServiceProvider sp)
        {
            var results = new List<ABird>();

            for (int i = 0; i < 10; i++)
            {
                var result = sp.GetService<ABird>();
                results.Add(result);

                Console.WriteLine(result.GetHashCode());
            }

            return results;
        }
    }
}
