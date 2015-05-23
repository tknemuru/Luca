using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Stat.Logic;
using ReversiCommon.Collections;
using System.Data;
using ReversiCommon.Utility;
using TKCommon.Utility;

namespace Luca.Stat.Program
{
    /// <summary>
    /// <para>評価値統計プログラム</para>
    /// </summary>
    public class PG_STA001
    {
        #region "定数"
        /// <summary>
        /// <para>１ステージの範囲</para>
        /// </summary>
        private const int STAGE_RANGE = 4;

        /// <summary>
        /// <para>更新の度合い</para>
        /// </summary>
        private const double UPDATE_RATIO = 0.003;

        /// <summary>
        /// <para>１石あたりの評価値</para>
        /// </summary>
        private const double DISC_VALUE = 1000;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラス</para>
        /// </summary>
        private STA001Logic m_logic;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PG_STA001()
        {
            this.m_logic = new STA001Logic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>スコアインデックスデータの初期化を行う</para>
        /// <para>※時間がかかりすぎるため未使用※</para>
        /// </summary>
        public void InitializeScoreIndex()
        {
            DataTable dt = this.m_logic.GetTurnStageList();
            int limit = 0;

            for (int i = 4; i <= 8; i++)
            {
                limit = MathUtility.GetPows(3, i);
                for (int s = 0; s <= limit; s++)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.m_logic.InsertScoreIndex(s.ToString(), "diag" + i.ToString(), row["STAGE"].ToString());
                    }
                }
            }

            for (int i = 2; i <= 4; i++)
            {
                limit = MathUtility.GetPows(3, 8);
                for (int s = 0; s <= limit; s++)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.m_logic.InsertScoreIndex(s.ToString(), "hor_vert" + i.ToString(), row["STAGE"].ToString());
                    }
                }
            }

            limit = MathUtility.GetPows(3, 10);
            for (int s = 0; s <= limit; s++)
            {
                foreach (DataRow row in dt.Rows)
                {
                    this.m_logic.InsertScoreIndex(s.ToString(), "edge2X", row["STAGE"].ToString());
                }
            }

            limit = MathUtility.GetPows(3, 10);
            for (int s = 0; s <= limit; s++)
            {
                foreach (DataRow row in dt.Rows)
                {
                    this.m_logic.InsertScoreIndex(s.ToString(), "corner2X5", row["STAGE"].ToString());
                }
            }

            limit = MathUtility.GetPows(3, 9);
            for (int s = 0; s <= limit; s++)
            {
                foreach (DataRow row in dt.Rows)
                {
                    this.m_logic.InsertScoreIndex(s.ToString(), "corner3X3", row["STAGE"].ToString());
                }
            }
        }

        /// <summary>
        /// <para>ターンステージデータを作成する</para>
        /// </summary>
        public void CreateTurnStage()
        {
            // ターンステージテーブルをTRUNCATE
            this.m_logic.TruncateTurnStage();

            // 登録していく
            int rangeLimit = STAGE_RANGE;
            int stage = 1;
            for (int i = 1; i <= TurnKeeper.MAX_TURN; i++)
            {
                this.m_logic.InsertTurnStage(i, stage);
                if (i == rangeLimit)
                {
                    rangeLimit += STAGE_RANGE;
                    stage++;
                }
            }
        }

        /// <summary>
        /// <para>評価値の統計を行う</para>
        /// </summary>
        public void StatisticsExecute()
        {
            // 計算対象のデータを取得する
            DataTable dt = this.m_logic.GetKifuIndexUnit();

            // 計算していく
            foreach (DataRow row in dt.Rows)
            {
                string kifuId = row["KIFU_ID"].ToString();
                string turnNo = row["TURN_NO"].ToString();

                // 存在しないインデックスの挿入
                this.m_logic.InsertNotExistsScoreIndex(kifuId, turnNo);

                int theoryScore = int.Parse(row["SCORE"].ToString());
                int nowScore = this.m_logic.GetNowEvaluateScore(kifuId, turnNo);

                // 更新値の算出
                int updateScore = (int)((double)((theoryScore * DISC_VALUE) - nowScore) * UPDATE_RATIO);

                // 評価値の更新
                this.m_logic.UpdateScoreIndex(kifuId, turnNo, updateScore);
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
