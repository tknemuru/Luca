using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Optimization.Logic;
using System.Data;
using ReversiCommon.Collections;
using TKCommon.Collections;
using ReversiCommon.Utility;
using TKCommon.Debugger;
using TKCommon.Utility;
using TKCommon.Collections.JsonCollections;
using ReversiCommon.Collections.JsonCollections;

namespace Luca.Optimization.Program
{
    /// <summary>
    /// インデックスオプティマイザクラス
    /// </summary>
    public class IndexOptimizer
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// ロジックインスタンス
        /// </summary>
        private IndexOptimaizerLogic m_Logic;

        /// <summary>
        /// ステージ
        /// </summary>
        private int m_Stage;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IndexOptimizer()
        {
            this.m_Logic = new IndexOptimaizerLogic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute()
        {
            List<int> turnList = new List<int>();
            for (int i = (((OutputJsonIndexMatrix.START_STAGE - 1) * OutputJsonIndexMatrix.ONE_STAGE_TURN) + 1); i <= (OutputJsonIndexMatrix.END_STAGE * OutputJsonIndexMatrix.ONE_STAGE_TURN); i++)
            {
                if ((i % OutputJsonIndexMatrix.ONE_STAGE_TURN) == 0)
                {
                    this.m_Stage = (i / OutputJsonIndexMatrix.ONE_STAGE_TURN);
                    turnList.Add(i);
                    this.m_Logic = new IndexOptimaizerLogic();
                    DebugUtility.OutputStringToConsole("[STAGE" + this.m_Stage.ToString() + " START]" + DateTime.Now.ToString());
                    StopWatchLogger.StartEventWatch("IndexOptimizer:Stage" + this.m_Stage.ToString());
                    this.Execute(turnList);
                    StopWatchLogger.StopEventWatch("IndexOptimizer:Stage" + this.m_Stage.ToString());
                    DebugUtility.OutputStringToConsole("[STAGE" + this.m_Stage.ToString() + " END]" + DateTime.Now.ToString());
                    StopWatchLogger.DisplayAllEventTimes();
                    StopWatchLogger.ClearAllEventTimes();
                    turnList = new List<int>();
                }
                else
                {
                    turnList.Add(i);
                }
            }
        }

        /// <summary>
        /// インデックスの統計を実行する
        /// </summary>
        public void Execute(List<int> turnList)
        {
            // マスタインデックスデータを取得
            List<MasterIndexJson> masterList = JsonUtility.Deserialize<List<MasterIndexJson>>(OutputJsonIndexMatrix.JSON_FILE_PATH + string.Format(OutputJsonIndexMatrix.JSON_FILE_NAME_MASTER_INDEX, this.m_Stage));

            // JSONデータからインデックス行列を復元
            SparseMatrixJsonIn<double> json = JsonUtility.Deserialize<SparseMatrixJsonIn<double>>(OutputJsonIndexMatrix.JSON_FILE_PATH + string.Format(OutputJsonIndexMatrix.JSON_FILE_NAME, this.m_Stage));
            SparseMatrix<double> A = new SparseMatrix<double>(json);
            SparseVectorJsonIn<double> bjson = JsonUtility.Deserialize<SparseVectorJsonIn<double>>(OutputJsonIndexMatrix.JSON_FILE_PATH + string.Format(OutputJsonIndexMatrix.JSON_FILE_NAME_VECTOR_B, this.m_Stage));
            SparseVector<double> b = new SparseVector<double>(bjson);

            // xベクトルを作成
            SparseVector<double> x = new SparseVector<double>(A.Width, 0);

            // BiCGSTAB法を実行
            StopWatchLogger.StartEventWatch("BiCgStabUsingSparseVector");
            SparseVector<double> trueX = new BiCgStabUsingSparseVector().Execute(A, b, x);
            StopWatchLogger.StopEventWatch("BiCgStabUsingSparseVector");

            // DBを結果を登録
            this.m_Logic.InsertScoreIndex(masterList, trueX, this.m_Stage);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
