using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Collections;

namespace LucaStrengthMeasurement.Collections
{
    /// <summary>
    /// 対戦結果
    /// </summary>
    public class BattleResult
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 黒プレイヤ名
        /// </summary>
        private static string m_BlackPlayerName;

        /// <summary>
        /// 白プレイヤ名
        /// </summary>
        private static string m_WhitePlayerName;

        /// <summary>
        /// 結果リスト
        /// </summary>
        private static List<BattleResult> m_ResultList;

        /// <summary>
        /// 黒の勝ち回数
        /// </summary>
        private static int m_BlackWinCount;

        /// <summary>
        /// 白の勝ち回数
        /// </summary>
        private static int m_WhiteWinCount;

        /// <summary>
        /// 引き分け回数
        /// </summary>
        private static int m_DrawCount;

        /// <summary>
        /// 対戦ID
        /// </summary>
        private int m_BattleId;

        /// <summary>
        /// 黒のスコア
        /// </summary>
        private int m_BlackScore;

        /// <summary>
        /// 白のスコア
        /// </summary>
        private int m_WhiteScore;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blackScore"></param>
        /// <param name="whiteScore"></param>
        private BattleResult(int id, int blackScore, int whiteScore)
        {
            this.m_BattleId = id;
            this.m_BlackScore = blackScore;
            this.m_WhiteScore = whiteScore;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// プレイヤの名前をセットする
        /// </summary>
        /// <param name="blackPlayerName"></param>
        /// <param name="whitePlayerName"></param>
        public static void SetPlayerName(string blackPlayerName, string whitePlayerName)
        {
            m_BlackPlayerName = blackPlayerName;
            m_WhitePlayerName = whitePlayerName;
            m_BlackWinCount = 0;
            m_WhiteWinCount = 0;
            m_DrawCount = 0;
            m_ResultList = new List<BattleResult>();
        }

        /// <summary>
        /// 結果を記録する
        /// </summary>
        public static void Register(int id)
        {
            m_ResultList.Add(new BattleResult(id, Board.GetBlackCount(), Board.GetWhiteCount()));
            m_BlackWinCount += (Board.GetBlackCount() > Board.GetWhiteCount()) ? 1 : 0;
            m_WhiteWinCount += (Board.GetBlackCount() < Board.GetWhiteCount()) ? 1 : 0;
            m_DrawCount += (Board.GetBlackCount() == Board.GetWhiteCount()) ? 1 : 0;
        }

        /// <summary>
        /// 結果を文字列として返す
        /// </summary>
        /// <returns></returns>
        public static string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("--- 黒：{0}　白：{1} ---", m_BlackPlayerName, m_WhitePlayerName));
            sb.AppendLine(string.Format("【黒Win：{0}回　白Win：{1}回　引き分け：{2}回】", m_BlackWinCount, m_WhiteWinCount, m_DrawCount));
            foreach (BattleResult result in m_ResultList)
            {
                sb.AppendLine(string.Format("No.{0}　黒：{1}　白：{2}", result.m_BattleId, result.m_BlackScore, result.m_WhiteScore));
            }
            return sb.ToString();
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
