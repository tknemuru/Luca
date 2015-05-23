using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;
using ReversiCommon.Evaluators.Logic;
using System.Data;
using ReversiCommon.TypedDataSet;
using ReversiCommon.Evaluators;
using System.Configuration;
using TKCommon.Utility;

using ReversiCommon.Collections.JsonCollections;
using TKCommon.Collections.JsonCollections;
using TKCommon.Collections;
using TKCommon.Debugger;
using Luca.Optimization.Program;
using Luca.Optimization.Logic;
using System.Diagnostics;

namespace LucaDevelop.Worker
{
    /// <summary>
    /// スコアインデックスを外部ファイルに出力する
    /// </summary>
    public class ScoreIndexOutputter
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// 出力処理を実行する
        /// </summary>
        public void Execute()
        {
            foreach (int stage in TurnKeeper.Stage)
            {
                ReversiCommonDataSet.ST_SCORE_INDEXDataTable dt = this.GetScoreIndex(stage);
                string csv = this.GetScoreIndexCsv(dt);
                string path = string.Format(ConfigurationManager.AppSettings["Score Index File Path"], stage);
                FileUtility.Write(csv, path, false);
            }
        }

        /// <summary>
        /// 出力処理を実行する
        /// </summary>
        public void Execute(List<MasterIndexJson> masterList, SparseVector<double> x, int stage)
        {
            string csv = this.GetScoreIndexCsv(masterList, x);
            string path = string.Format(ConfigurationManager.AppSettings["Score Index File Path"], stage);
            FileUtility.Write(csv, path, false);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// スコアインデックステーブルを取得する
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        private ReversiCommonDataSet.ST_SCORE_INDEXDataTable GetScoreIndex(int stage)
        {
            return new ScoreIndexEvaluatorLogic().GetScoreIndex(stage);
        }

        /// <summary>
        /// スコアインデックスのCSVフォーマット文字列を取得する
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetScoreIndexCsv(ReversiCommonDataSet.ST_SCORE_INDEXDataTable dt)
        {
            string csv = string.Empty;
            foreach (ReversiCommonDataSet.ST_SCORE_INDEXRow row in dt)
            {
                string item = ScoreIndexEvaluator.GetScoreIndexKey(row.INDEX_NAME, row.INDEX_NO.ToString()) + "," + row.SCORE.ToString("R");
                if (string.IsNullOrEmpty(csv))
                {
                    csv = item;
                }
                else
                {
                    csv += ("," + item);
                }
            }
            return csv;
        }

        /// <summary>
        /// スコアインデックスデータを登録する
        /// </summary>
        /// <param name="masterDt"></param>
        /// <param name="x"></param>
        private string GetScoreIndexCsv(List<MasterIndexJson> masterList, SparseVector<double> x)
        {
            string csv = string.Empty;

            Debug.Assert(((masterList.Count + IndexExtraInformation.All.Count) == x.Count()), "要素数が違います。");

            int i = 0;
            foreach (MasterIndexJson row in masterList)
            {
                if (x[i] != 0D)
                {
                    string item = ScoreIndexEvaluator.GetScoreIndexKey(row.IndexName, row.NormalizeIndexNo) + "," + x[i].ToString("R");
                    if (string.IsNullOrEmpty(csv))
                    {
                        csv = item;
                    }
                    else
                    {
                        csv += ("," + item);
                    }
                }
                i++;
            }

            // 拡張情報
            foreach (IndexExtraInformation extInfo in IndexExtraInformation.All)
            {
                string item = ScoreIndexEvaluator.GetScoreIndexKey(extInfo.Name, "0") + "," + x[i + extInfo.Index].ToString("R");
                csv += ("," + item);
            }
            return csv;
        }
        #endregion
        #endregion
    }
}
