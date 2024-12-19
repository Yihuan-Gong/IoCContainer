using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample.Birds
{
    public class Sparrow : ABird
    {
        public Sparrow(IFood food, ILogger<Sparrow> logger) : base(food)
        {
            this.Logger = logger;
        }

        public ILogger<Sparrow> Logger { get; set; }



        //public Sparrow()
        //{
        //    Logger = Service.GetInstance<ILogger<Sparrow>>();
        //}

        //public Sparrow(IFood food , ILogger<Sparrow> logger)
        //{
        //    this.Logger = logger;
        //    this.food = food;
        //}

        //public Sparrow(ILogger<Sparrow> logger)
        //{
        //    Logger = logger;
        //}

        public override void Eat()
        {
            Console.WriteLine("麻雀吃飯:" + Food.Name );

            Logger.LogInformation("麻雀吃飯");
        }

        public override void SayAge()
        {
            Console.WriteLine($"我{Age}歲");

            Logger.LogInformation($"我{Age}歲");
        }
    }
}
