using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Luca.Dumm.Logic;
using System.Data;
using ReversiCommon.Collections;
using STR = TKCommon.Utility.StringUtility;
using System.Diagnostics;
using ReversiCommon.Utility;

namespace Luca.Dumm.Program
{
    /// <summary>
    /// <para>ダミーリバーシ実行プログラムクラス</para>
    /// </summary>
    public class PG_DUM001
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ロジッククラス</para>
        /// </summary>
        private DUM001Logic m_logic;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PG_DUM001()
        {
            this.m_logic = new DUM001Logic();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>ダミーリバーシを実行する</para>
        /// </summary>
        public void DummyExecute()
        {
            // ThorファイルをDBから読み込む
            //DataTable dt = this.m_logic.GetRawKifuData();
            DataTable dt = this.m_logic.GetRawKifuDataForExtraData();

            // 対局していく
            foreach (DataRow row in dt.Rows)
            {
                TurnKeeper.InitializeTurnKeeper();
                Board.InitializeBoard();
                List<string> list = STR.ToListSplitCount(row["KIFU"].ToString(), 2);
                foreach (string point in list)
                {
                    DebugUtility.OutputStringToConsole(point + "：" + TurnKeeper.NowTurnColor.ColorId + "：" + TurnKeeper.NowTurnNumber.ToString() + "ターン目");
                    DebugUtility.OutputBoardToConsole();

                    if (Board.IsReversible())
                    {
                        // 追加情報を記録
                        this.m_logic.SetParityAndMobilityCmd(row["KIFU_ID"].ToString());

                        // 裏返す
                        Board.Reverse(int.Parse(point));

                        // ターンごとにインデックスを取得し、DBに書き込む
                        //this.m_logic.RegisterIndex(row["KIFU_ID"].ToString());

                        // ターンをまわす
                        TurnKeeper.ChangeTurn();
                    }
                    else
                    {
                        // パス
                        TurnKeeper.ChangeOnlyColor();

                        // パスの場合は色を変えた後に追加情報を記録
                        this.m_logic.SetParityAndMobilityCmd(row["KIFU_ID"].ToString());

                        // 自分自身は裏返せる？
                        if (Board.IsReversible())
                        {
                            // 裏返す
                            Board.Reverse(int.Parse(point));

                            // ターンごとにインデックスを取得し、DBに書き込む
                            //this.m_logic.RegisterIndex(row["KIFU_ID"].ToString());

                            // ターンをまわす
                            TurnKeeper.ChangeTurn();
                        }
                        else
                        {
                            // ゲーム終了
                            break;
                        }
                    }
                }

                // ゲームが終了したらDBに登録
                this.m_logic.InsertParityAndMobility();
            }

            // 最後は強制的にDBに登録
            this.m_logic.InsertParityAndMobility(true);
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
