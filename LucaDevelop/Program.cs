using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LucaDevelop.Worker;
using System.Configuration;
using ReversiCommon.Test;

namespace LucaDevelop
{
    class Program
    {
        static void Main(string[] args)
        {
            // 読み込みインスタンスを作成
            //string path = System.Configuration.ConfigurationManager.AppSettings["Kifu File Path"];
            //new VsOthaVsThellKifuReader(path, "*.repair").Read();

            // 棋譜ファイルのリペアを実行
            //new KifuFileRepair(path, "*.new").Repair();

            // 実行ファイルの配布
            //new FileDistributer().Distribute();
            
            // インデックスの統計を実行する
            //new IndexOptimizer().Execute();
            //while (true)
            //{
            //    ;
            //}

            // スコアインデックスを外だしする
            //new ScoreIndexOutputter().Execute();

            // 定石ファイルを作成する
            //new BookMaker(ConfigurationManager.AppSettings["Kifu File Path"], "*.repair").Make();

            // 定石を分析する
            //new BookAnalyzer(ConfigurationManager.AppSettings["Kifu File Path"], "*.repair").Make();

            // テストのテスト
            //new ScoreIndexGeneratorTest().TestScoreIndexList("corner2X5_1", "corner2X5", 29551, 29497, -1, 1);

            // 正規化インデックスを出力する
            new OutputNormalizeIndex().Execute();
        }
    }
}
