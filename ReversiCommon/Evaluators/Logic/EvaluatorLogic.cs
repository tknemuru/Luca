using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.TypedDataSet;

namespace ReversiCommon.Evaluators.Logic
{
    /// <summary>
    /// <para>スコアインデックス評価ロジッククラス</para>
    /// </summary>
    public class ScoreIndexEvaluatorLogic
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
        /// <para>スコアインデックスを取得する</para>
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public ReversiCommonDataSet.ST_SCORE_INDEXDataTable GetScoreIndex(int stage)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM ST_SCORE_INDEX");
            sb.AppendLine(" WHERE STAGE = " + stage);

            ReversiCommonDataSet.ST_SCORE_INDEXDataTable dt = new ReversiCommonDataSet.ST_SCORE_INDEXDataTable();
            SQL.ReadDataAdapter(sb.ToString(), dt);

            return dt;
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
