using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReversiCommon.Collections;
using ReversiCommon.Evaluators.Logic;
using ReversiCommon.TypedDataSet;
using ReversiCommon.Utility;
using TKCommon.Debugger;
using System.Configuration;
using System.Diagnostics;
using TKCommon.Utility;

namespace ReversiCommon.Evaluators
{
    /// <summary>
    /// <para>スコアインデックス評価関数クラス</para>
    /// </summary>
    public class ScoreIndexEvaluator : IEvaluator
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ターンステージ</para>
        /// </summary>
        private static int m_TurnStage = 0;

        /// <summary>
        /// <para>スコアインデックスディクショナリ</para>
        /// </summary>
        private static Dictionary<string, double> m_IndexDic;

        /// <summary>
        /// <para>ロジッククラスインスタンス</para>
        /// </summary>
        private ScoreIndexEvaluatorLogic m_logic;

        /// <summary>
        /// 分析する場合はTrue
        /// </summary>
        private bool m_IsAnalytics;

        /// <summary>
        /// インデックスの概要情報
        /// </summary>
        public Dictionary<string, double> IndexDescription;

        /// <summary>
        /// スコアインデックスのファイルパス
        /// </summary>
        private string m_ScoreIndexFilePath;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public ScoreIndexEvaluator(bool isAnalytics = false)
        {
            this.m_logic = new ScoreIndexEvaluatorLogic();
            this.IndexDescription = new Dictionary<string, double>();
            this.m_IsAnalytics = isAnalytics;
            this.m_ScoreIndexFilePath = ConfigurationManager.AppSettings["Score Index File Path"];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath"></param>
        public ScoreIndexEvaluator(string scoreIndexFilePath)
        {
            this.m_logic = new ScoreIndexEvaluatorLogic();
            this.IndexDescription = new Dictionary<string, double>();
            this.m_ScoreIndexFilePath = scoreIndexFilePath;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>評価値を取得</para>
        /// </summary>
        /// <returns></returns>
        public double GetEvaluate()
        {
            StopWatchLogger.StartEventWatch("ScoreIndexEvaluator");

            // スコアインデックスの更新
            this.UpdateScoreIndexTable();

            // 現在のインデックス値を取得
            StopWatchLogger.StartEventWatch("現在のインデックス値を取得");
            List<ScoreIndex> scoreIndexList = ScoreIndexGenerator.GetIndexList();
            StopWatchLogger.StopEventWatch("現在のインデックス値を取得");

            // 評価値を取得
            StopWatchLogger.StartEventWatch("評価値を取得");
            double score = 0;
            foreach (ScoreIndex scoreIndex in scoreIndexList)
            {
                if (m_IndexDic.ContainsKey(scoreIndex.Key))
                {
                    score += (m_IndexDic[scoreIndex.Key] * scoreIndex.Value);
                }
            }
            StopWatchLogger.StopEventWatch("評価値を取得");

            StopWatchLogger.StartEventWatch("拡張情報の評価値を取得");
            // 拡張情報の評価値を取得
            // パリティ
            score += m_IndexDic["0" + IndexExtraInformation.Parity.Name] * double.Parse(TurnKeeper.Parity);

            // 黒着手可能数
            double blackMobility = Board.GetAllPutablePointerCount(Color.Black);
            double whiteMobility = Board.GetAllPutablePointerCount(Color.White);
            double mobility = blackMobility - whiteMobility;
            score += m_IndexDic["0" + IndexExtraInformation.Mobility.Name] * mobility;
            
            // 全部なしチェック
            score += this.GetAllNothingScore(blackMobility, whiteMobility);
            StopWatchLogger.StopEventWatch("拡張情報の評価値を取得");

            StopWatchLogger.StopEventWatch("ScoreIndexEvaluator");

            // 概要を作成
            //this.MakeIndexDescription();

            return score;
        }

        /// <summary>
        /// スコアインデックスのキーを取得する
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="indexNo"></param>
        /// <returns></returns>
        public static string GetScoreIndexKey(string indexName, string indexNo)
        {
            return (indexNo + indexName);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>スコアインデックスを更新する</para>
        /// </summary>
        private void UpdateScoreIndexTable()
        {
            StopWatchLogger.StartEventWatch("UpdateScoreIndexTable");
            if(m_TurnStage != TurnKeeper.NowStage)
            {
                m_TurnStage = TurnKeeper.NowStage;
                m_IndexDic = new Dictionary<string, double>();
                string csv = FileUtility.ReadToEnd(string.Format(this.m_ScoreIndexFilePath, m_TurnStage));
                string[] csvList = csv.Split(',');
                Debug.Assert((csvList.Length % 2 == 0), "要素数が奇数です。");
                for (int i = 0; i < csvList.Length; i += 2)
                {
                    m_IndexDic.Add(csvList[i], double.Parse(csvList[i + 1]));
                }
            }
            StopWatchLogger.StopEventWatch("UpdateScoreIndexTable");
        }

        /// <summary>
        /// インデックス概要を作成する
        /// </summary>
        private void MakeIndexDescription()
        {
            // 分析しない場合は何もしない
            if (!this.m_IsAnalytics) { return; }

            List<CostIndexGenerator> costIndexList = CostIndexGenerator.GetIndexList();


            // 評価値を取得
            foreach (CostIndexGenerator costIndex in costIndexList)
            {
                if (m_IndexDic.ContainsKey(NormalizeIndex.Get(costIndex.Name, costIndex.Index) + costIndex.Name))
                {
                    this.IndexDescription.Add(costIndex.Description, (m_IndexDic[NormalizeIndex.Get(costIndex.Name, costIndex.Index) + costIndex.Name] * NormalizeIndex.GetNormalizeType(costIndex.Name, costIndex.Index)));
                }
                else
                {
                    this.IndexDescription.Add(costIndex.Description, 0);
                }
            }

            // 拡張情報の評価値を取得
            // パリティ
            this.IndexDescription.Add("パリティ", m_IndexDic["0" + IndexExtraInformation.Parity.Name] * double.Parse(TurnKeeper.Parity));

            // 黒着手可能数
            double mobility = (Board.GetAllPutablePointerCount(Color.Black)) - Board.GetAllPutablePointerCount(Color.White);
            this.IndexDescription.Add("着手可能数", m_IndexDic["0" + IndexExtraInformation.Mobility.Name] * mobility);

        }

        /// <summary>
        /// 全てなしの場合のスコアを取得
        /// </summary>
        /// <returns></returns>
        private double GetAllNothingScore(double blackMobility, double whiteMobility)
        {
            if (blackMobility == 0 && whiteMobility == 0)
            {
                return (double)((Board.GetBlackCount() - Board.GetWhiteCount()) * 100000);
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #endregion
    }
}
