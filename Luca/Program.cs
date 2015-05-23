using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Thor;
using Luca.Dumm.Program;
using Luca.Stat.Program;
using Luca.Jose.Program;
using Luca.IndexRebuild.Program;
using Luca.Optimization.Program;
using Luca.Optimization.Test;
//using ReversiCommon.Searchs.Test;

namespace Luca
{
    /// <summary>
    /// <para>Lucaバッチメインクラス</para>
    /// </summary>
    class Program
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        static void Main(string[] args)
        {
            // Thorファイルを読み込んでDBに登録する
            //new PG_THR001().RegisterThorData();

            // Thorファイルからインデックスを登録する
            //new PG_DUM001().DummyExecute();

            // ターンステージデータを作成する
            //new PG_STA001().CreateTurnStage();

            // コストインデックスの初期化を行う
            //new PG_STA001().InitializeScoreIndex();

            // 評価値の更新を行う
            //new PG_STA001().StatisticsExecute();

            // 評価値の更新を行う（高速バージョン）
            //new PG_STA002().DummyExecute();

            // 定石の評価値の更新を行う
            //new PG_JOS001().StatisticsExecute();

            // インデックスの正規化を行う
            //new Normalization().CreateIndexNormalize();

            // インデックスの統計処理を行う
            //new IndexOptimizer().Execute();

            // インデックスをCSVに出力する
            //new OutputIndexMatrix().Execute();

            // インデックスをJSONに出力する
            //new OutputJsonIndexMatrix().Execute();

            //new SteepestDescentTest().TestEasyExecute();

            //new NegaMaxBasicTest().TestNegaMax();
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
