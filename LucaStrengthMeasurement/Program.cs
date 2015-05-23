using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LucaStrengthMeasurement.Worker;

namespace LucaStrengthMeasurement
{
    class Program
    {
        static void Main(string[] args)
        {
            // 学習結果の比較を行う
            new StrengthMeasurer("旧バージョンVS新計算バージョン(再検査)").Excute();
        }
    }
}
