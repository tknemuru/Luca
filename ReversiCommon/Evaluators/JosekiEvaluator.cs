using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Evaluators.Logic;
using ReversiCommon.Collections;
using ReversiCommon.Utility;
using System.Data;
using TKCommon.Debugger;

namespace ReversiCommon.Evaluators
{
    /// <summary>
    /// <para>定石評価関数クラス</para>
    /// </summary>
    public class JosekiEvaluator : IEvaluator
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラスインスタンス</para>
        /// </summary>
        private JosekiEvaluatorLogic m_logic;

        /// <summary>
        /// <para>定石リスト（黒）</para>
        /// </summary>
        private static Dictionary<string, int> m_JosekiBlackDic;

        /// <summary>
        /// <para>定石リスト（白）</para>
        /// </summary>
        private static Dictionary<string, int> m_JosekiWhiteDic;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public JosekiEvaluator()
        {
            this.m_logic = new JosekiEvaluatorLogic();
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
            // 現在までの打ち筋を取得
            StopWatchLogger.StartEventWatch("JosekiEvaluator-GetRotateLogStack");
            string pattern = Board.GetRotateLogStack();
            StopWatchLogger.StopEventWatch("JosekiEvaluator-GetRotateLogStack");

            // 定石スコアを取得
            StopWatchLogger.StartEventWatch("JosekiEvaluator-GetJosekiScore");
            SetJoseki();
            int ret = 0;
            while (pattern.Length > 3)
            {
                if (TurnKeeper.NowTurnColor == Color.Black)
                {
                    if (m_JosekiBlackDic.ContainsKey(pattern))
                    {
                        ret = m_JosekiBlackDic[pattern];
                        break;
                    }
                }
                else
                {
                    if (m_JosekiWhiteDic.ContainsKey(pattern))
                    {
                        ret = m_JosekiWhiteDic[pattern];
                        break;
                    }
                }
                pattern = pattern.Substring(0, pattern.Length - 2);
            }

            StopWatchLogger.StopEventWatch("JosekiEvaluator-GetJosekiScore");
            
            return ret;
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>定石データをセットする</para>
        /// </summary>
        public static void SetJoseki()
        {
            DataTable dt;
            if (m_JosekiBlackDic == null)
            {
                m_JosekiBlackDic = new Dictionary<string, int>();
                dt = JosekiEvaluatorLogic.GetJosekiViewData(Color.Black);
                foreach (DataRow row in dt.Rows)
                {
                    m_JosekiBlackDic.Add(row["PATTERN"].ToString(), int.Parse(row["SCORE"].ToString()));
                }
            }

            if (m_JosekiWhiteDic == null)
            {
                m_JosekiWhiteDic = new Dictionary<string, int>();
                dt = JosekiEvaluatorLogic.GetJosekiViewData(Color.White);
                foreach (DataRow row in dt.Rows)
                {
                    m_JosekiWhiteDic.Add(row["PATTERN"].ToString(), int.Parse(row["SCORE"].ToString()));
                }
            }
        }
        #endregion
        #endregion
    }
}
