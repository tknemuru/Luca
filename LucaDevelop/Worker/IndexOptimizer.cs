using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;
using ReversiCommon.Collections.JsonCollections;
using TKCommon.Utility;
using System.Configuration;
using TKCommon.Collections.JsonCollections;
using System.IO;
using TKCommon.Collections;
using TKCommon.Debugger;
using Luca.Optimization.Program;
using Luca.Optimization.Logic;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// インデックスオプティマイザクラス
    /// </summary>
    public class IndexOptimizer
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IndexOptimizer()
        {
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute()
        {
            //List<string> StageList = ConfigurationManager.AppSettings["Optimize Stage List"].Split(',').ToList();
            //foreach (string stage in StageList)
            //{
            //    this.Execute(stage);
            //}
            //string stage = this.GetStageFromProgramDirectory();
            string stage = "15";
            Console.WriteLine("OptimaizeStage:" + stage);
            this.Execute(stage);
        }

        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute(string stage)
        {
            // マスタインデックスデータを取得
            List<MasterIndexJson> masterList = JsonUtility.Deserialize<List<MasterIndexJson>>(ConfigurationManager.AppSettings["Master Index File Path"]);

            // 巨大疎行列の作成
            IEnumerable<string> files = Directory.EnumerateFiles(FileUtility.GetFileDirectory(ConfigurationManager.AppSettings["Index Matrix File Path"]), "*." + stage, SearchOption.AllDirectories).OrderBy(key => key);
            SparseBigMatrix A = new SparseBigMatrix(files, masterList.Count + IndexExtraInformation.All.Count, 0);

            // 結果リストの作成
            files = Directory.EnumerateFiles(FileUtility.GetFileDirectory(ConfigurationManager.AppSettings["Result Score File Path"]), "*." + stage, SearchOption.AllDirectories).OrderBy(key => key);
            List<double> b = new List<double>();
            foreach (string file in files)
            {
                string csv = FileUtility.ReadToEnd(file);
                var rangeB = from a in csv.Replace(Environment.NewLine, ",").Split(',')
                             where a != ""
                             select double.Parse(a);
                b.AddRange(rangeB);
            }

            // xベクトルを作成
            SparseVector<double> x = new SparseVector<double>(A.Width, 0);

            // 最急降下法を実行
            StopWatchLogger.StartEventWatch("SteepestDescentUsingSparseBigMatrix");
            IEnumerable<string> masterFiles = Directory.EnumerateFiles(FileUtility.GetFileDirectory(ConfigurationManager.AppSettings["Index Matrix File Path"]), "*." + stage, SearchOption.AllDirectories).OrderBy(key => key);
            SparseVector<double> trueX = new SteepestDescentUsingSparseBigMatrix(masterFiles).Execute(A, b, x);
            StopWatchLogger.StopEventWatch("SteepestDescentUsingSparseBigMatrix");

            //// DBを結果を登録
            //new IndexOptimaizerLogic().InsertScoreIndex(masterList, trueX, int.Parse(stage));

            // CSVを出力
            new ScoreIndexOutputter().Execute(masterList, trueX, int.Parse(stage));
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// プログラム実行ディレクトリからステージを取得する
        /// </summary>
        /// <returns></returns>
        private string GetStageFromProgramDirectory()
        {
            string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeName = FileUtility.GetFileName(dir);
            dir = dir.Replace("\\" + exeName, "");
            return FileUtility.GetFileName(dir);
        }
        #endregion
        #endregion
    }
}
