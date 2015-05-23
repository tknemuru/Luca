using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Jose.Logic;
using System.Data;

namespace Luca.Jose.Program
{
    /// <summary>
    /// <para>定石作成プログラム</para>
    /// </summary>
    public class PG_JOS001
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラス</para>
        /// </summary>
        private JOS001Logic m_logic;

        /// <summary>
        /// <para>更新の度合い</para>
        /// </summary>
        private const double UPDATE_RATIO = 0.003;

        /// <summary>
        /// <para>１石あたりの評価値</para>
        /// </summary>
        private const double DISC_VALUE = 1000;

        /// <summary>
        /// <para>１手目の座標</para>
        /// </summary>
        private const string FIRST_POINT = "56";
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PG_JOS001()
        {
            this.m_logic = new JOS001Logic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>定石の評価値分析を行う</para>
        /// </summary>
        public void StatisticsExecute()
        {
            // 生の棋譜データを取得する
            DataTable dt = this.m_logic.GetRawKifuData();

            // 計算していく
            foreach(DataRow row in dt.Rows)
            {
                for (int i = 2; i < (JOS001Logic.END_TURN_NO * 2); i += 2)
                {
                    string pattern = row["KIFU"].ToString().Substring(0, i);

                    // 一手目は評価対象外
                    if(pattern == FIRST_POINT) { continue; }

                    int theoryScore = int.Parse(row["THEORY_BLACK_SCORE"].ToString());
                    int nowScore = m_logic.GetNowJosekiScore(pattern);

                    // 更新値の算出
                    int updateScore = (int)((double)((theoryScore * DISC_VALUE) - nowScore) * UPDATE_RATIO);

                    // 評価値の更新
                    this.m_logic.MergeIntoJoseki(pattern, updateScore.ToString());
                }

                // 更新履歴の記録
                string kifuId = row["KIFU_ID"].ToString();
                this.m_logic.InsertJosekiRireki(kifuId);
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
