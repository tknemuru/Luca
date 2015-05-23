using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using SQL = TKCommon.Utility.MySqlUtility;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using TKCommon.Constant;

namespace Luca.Dumm.Logic
{
    /// <summary>
    /// <para>ダミーリバーシ実行ロジッククラス</para>
    /// </summary>
    public class DUM001Logic
    {
        #region "定数"
        /// <summary>
        /// 更新間隔
        /// </summary>
        private const int UPDATE_SPAN = 1200;

        /// <summary>
        /// 更新対象データの棋譜ID範囲
        /// </summary>
        private static readonly Tuple<int, int> SELECT_SPAN = new Tuple<int, int>(75001, 110000);
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// SQLコマンドリスト
        /// </summary>
        private List<string> m_CmdList;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DUM001Logic()
        {
            this.m_CmdList = new List<string>();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>生棋譜データをDBから取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetRawKifuData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM AR_RAW_KIFU AS ARK");
            sb.AppendLine(" WHERE NOT EXISTS(SELECT 1");
            sb.AppendLine("                    FROM AR_KIFU_INDEX AS AKI");
            sb.AppendLine("                   WHERE AKI.KIFU_ID = ARK.KIFU_ID)");

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>生棋譜データをDBから取得する</para>
        /// </summary>
        /// <returns></returns>
        public DataTable GetRawKifuDataForExtraData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("  FROM AR_RAW_KIFU AS ARK");
            sb.AppendLine(" WHERE NOT EXISTS(SELECT 1");
            sb.AppendLine("                    FROM AR_KIFU_TURN_EXTEND_INFO AS EXT");
            sb.AppendLine("                   WHERE EXT.KIFU_ID = ARK.KIFU_ID)");
            sb.AppendLine(string.Format("   AND ARK.KIFU_ID BETWEEN {0} AND {1}", SELECT_SPAN.Item1, SELECT_SPAN.Item2));

            return SQL.ReadDataAdapter(sb.ToString());
        }

        /// <summary>
        /// <para>インデックスをDBに記録する</para>
        /// </summary>
        public void RegisterIndex(string kifuId)
        {
            List<CostIndexGenerator> indexList = CostIndexGenerator.GetIndexList();
            foreach (CostIndexGenerator index in indexList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO AR_KIFU_INDEX");
                sb.AppendLine("     ( KIFU_ID");
                sb.AppendLine("     , TURN_NO");
                sb.AppendLine("     , INDEX_NAME");
                sb.AppendLine("     , INDEX_NO");
                sb.AppendLine("     )");
                sb.AppendLine("VALUES");
                sb.AppendLine("     ( " + kifuId);
                sb.AppendLine("     , " + TurnKeeper.NowTurnNumber.ToString());
                sb.AppendLine("     , " + STR.GetSqlString(index.Name));
                sb.AppendLine("     , " + index.Index);
                sb.AppendLine("     )");

                SQL.ExecuteCommand(sb.ToString());
            }
        }

        /// <summary>
        /// <para>インデックスのパリティ＆着手可能数をDBに記録する</para>
        /// </summary>
        public void SetParityAndMobilityCmd(string kifuId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AR_KIFU_TURN_EXTEND_INFO");
            sb.AppendLine("     ( KIFU_ID");
            sb.AppendLine("     , TURN_NO");
            sb.AppendLine("     , PARITY");
            sb.AppendLine("     , BLACK_MOBILITY");
            sb.AppendLine("     , WHITE_MOBILITY");
            sb.AppendLine("     )");
            sb.AppendLine("VALUES");
            sb.AppendLine("     ( " + kifuId);
            sb.AppendLine("     , " + TurnKeeper.NowTurnNumber.ToString());
            sb.AppendLine("     , " + this.Parity);
            sb.AppendLine("     , " + Board.GetAllPutablePointerCount(Color.Black).ToString());
            sb.AppendLine("     , " + Board.GetAllPutablePointerCount(Color.White).ToString());
            sb.AppendLine("     )");

            this.m_CmdList.Add(sb.ToString());
        }

        /// <summary>
        /// <para>インデックスのパリティ＆着手可能数をDBに記録する</para>
        /// </summary>
        public void InsertParityAndMobility(bool isRequireUpdate = false)
        {
            if (isRequireUpdate || (this.m_CmdList.Count >= UPDATE_SPAN))
            {
                SQL.ExecuteCommand(this.m_CmdList);
                this.m_CmdList = new List<string>();
            }
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// パリティ
        /// </summary>
        private string Parity
        {
            get
            {
                return (TurnKeeper.NowTurnColor == Color.Black) ? Const.SQL_FLG_YES : Const.SQL_FLG_NO;
            }
        }
        #endregion
        #endregion
    }
}
