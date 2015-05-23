using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LucaPerformanceMeasurement.Worker;

namespace LucaPerformanceMeasurement
{
    class Program
    {
        static void Main(string[] args)
        {
            new PerformanceMeasurer("セーフリスト使用").Excute();
        }
    }
}
