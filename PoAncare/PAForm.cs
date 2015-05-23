using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using ReversiCommon.Collections;
using ReversiCommon.Players;
using ReversiCommon.Utility;
using ReversiCommon.Strategys;
using PoAncareAnalytics;
using TKCommon.Debugger;

namespace PoAncare
{
    /// <summary>
    /// <para>PoAncareフォームクラス</para>
    /// </summary>
    public partial class PAForm : Form
    {
        #region "定数"
        /// <summary>
        /// <para>石イメージファイル名の接頭辞</para>
        /// </summary>
        private const string DISC_IMAGE_NAME_PREFIX = "pbxDisc";

        /// <summary>
        /// <para>ターンを表す文字列</para>
        /// </summary>
        private const string TURN_STR = "Turn";
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>プレイヤーリスト</para>
        /// </summary>
        private static Dictionary<ReversiCommon.Collections.Color, Player> m_PlayerList;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public PAForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "メソッド"
        #region "イベント"
        /// <summary>
        /// <para>フォームロード</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PAForm_Load(object sender, EventArgs e)
        {
            try
            {
                // フォームの初期化
                this.InitializeForm();

                // プレイヤーの作成
                this.SetPlayer();

                // 画面情報を更新する
                this.UpdateDisplay();

                // ゲーム開始
                this.RunGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// <para>石押下イベント</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void discPictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                StopWatchLogger.StartEventWatch("discPictureBox_Click");
                PictureBox pbx = (PictureBox)sender;
                int point = int.Parse(pbx.Name.Replace(DISC_IMAGE_NAME_PREFIX, ""));
                // 配置不可能なら何もしない
                if (!Board.IsReversibleThisPoint(point)) { return; }

                Debug.Assert(m_PlayerList[TurnKeeper.NowTurnColor] is PlayerHuman, "人間プレイヤー以外を操作しようとしました。");
                // 次に置く場所を確定する
                m_PlayerList[TurnKeeper.NowTurnColor].SetNextPointer(point);

                // ターンをまわす
                this.NextTurn();

                // 画面情報を更新する
                this.UpdateDisplay();

                StopWatchLogger.StopEventWatch("discPictureBox_Click");
                StopWatchLogger.DisplayAllEventTimes();

                // ゲーム続行
                this.RunGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// <para>Undoボタン押下イベント</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                // 戻せない場合は処理終了
                if (!Board.UndoReverse()) { return; };
                TurnKeeper.UndoTurn();
                while (m_PlayerList[TurnKeeper.NowTurnColor] is PlayerCpu)
                {
                    if (!Board.UndoReverse()) { break; };
                    TurnKeeper.UndoTurn();
                }
                this.UpdateDisplay();
                this.RunGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラーが発生しました。\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// <para>NewGameボタン押下イベント</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            // ターンの初期化
            TurnKeeper.InitializeTurnKeeper();
            
            // ボードの初期化
            Board.InitializeBoard();

            // フォームの初期化
            this.InitializeForm();

            // プレイヤーの作成
            this.SetPlayer();

            // 画面情報を更新する
            this.UpdateDisplay();

            // ゲーム開始
            this.RunGame();

        }

        /// <summary>
        /// 分析画面を表示する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            new AnalyticsForm().Show();
        }
        #endregion

        #region "内部処理"
        /// <summary>
        /// <para>フォーム初期化処理</para>
        /// </summary>
        private void InitializeForm()
        {
            this.lblInfo.Text = string.Empty;
        }

        /// <summary>
        /// <para>プレイヤーをセットする</para>
        /// </summary>
        private void SetPlayer()
        {
            m_PlayerList = new Dictionary<ReversiCommon.Collections.Color, Player>();
            //m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerHuman("まい", ReversiCommon.Collections.Color.White));
            m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerCpu("CPU", ReversiCommon.Collections.Color.White, new StrategyCpu()));
            //m_PlayerList.Add(ReversiCommon.Collections.Color.Black, new PlayerCpu("CPU", ReversiCommon.Collections.Color.Black, new StrategyCpu()));
            m_PlayerList.Add(ReversiCommon.Collections.Color.Black, new PlayerHuman("あなた", ReversiCommon.Collections.Color.Black));
            //m_PlayerList.Add(ReversiCommon.Collections.Color.White, new PlayerCpu("CPU_T", ReversiCommon.Collections.Color.White, new StrategyCpuTest()));

            // 名前ラベルの設定
            this.lblBlackUserName.Text = m_PlayerList[ReversiCommon.Collections.Color.Black].Name;
            this.lblWhiteUserName.Text = m_PlayerList[ReversiCommon.Collections.Color.White].Name;
        }

        /// <summary>
        /// <para>ターンをまわす</para>
        /// </summary>
        private void NextTurn()
        {
            // 石を置く位置を取得
            Disc pointer = m_PlayerList[TurnKeeper.NowTurnColor].GetNextPointer();

            // 石を置いて裏返す
            Board.Reverse(pointer.Point);

            // 初回は回転方法を決める
            if (TurnKeeper.NowTurnNumber == 1)
            {
                RotateUtility.SetRotateMethod(pointer.Point);
            }

            // ターンをまわす
            TurnKeeper.ChangeTurn();
        }

        /// <summary>
        /// <para>結果を表示する</para>
        /// </summary>
        private void DisplayResult()
        {
            if (int.Parse(lblBlackCount.Text) > int.Parse(lblWhiteCount.Text))
            {
                lblInfo.Text = m_PlayerList[ReversiCommon.Collections.Color.Black].Name + "の勝ちです。";
            }
            else if (int.Parse(lblBlackCount.Text) < int.Parse(lblWhiteCount.Text))
            {
                lblInfo.Text = m_PlayerList[ReversiCommon.Collections.Color.White].Name + "の勝ちです。";
            }
            else
            {
                lblInfo.Text = "引き分けです。";
            }
        }

        /// <summary>
        /// <para>画面情報を更新する</para>
        /// </summary>
        private void UpdateDisplay()
        {
            // 石数情報の更新
            this.lblBlackCount.Text = Board.GetBlackCount().ToString();
            this.lblWhiteCount.Text = Board.GetWhiteCount().ToString();

            // ターン情報の更新
            this.lblBlackTurn.Text = string.Empty;
            this.lblWhiteTurn.Text = string.Empty;
            if (TurnKeeper.NowTurnColor == ReversiCommon.Collections.Color.Black)
            {
                this.lblBlackTurn.Text = TURN_STR;
            }
            else
            {
                this.lblWhiteTurn.Text = TURN_STR;
            }

            foreach (Control c in this.Controls)
            {
                if (c.Name.Contains(DISC_IMAGE_NAME_PREFIX))
                {
                    PictureBox pbx = (PictureBox)c;
                    int point = int.Parse(c.Name.Replace(DISC_IMAGE_NAME_PREFIX, ""));
                    // イメージURLの更新
                    pbx.ImageLocation = Board.GetPointImageUrl(point);
                }
            }

            // 情報ラベルの更新
            this.lblInfo.Text = ReversiInformation.ToString();
        }

        /// <summary>
        /// <para>ゲームを実行する</para>
        /// </summary>
        private void RunGame()
        {
            if (!Board.IsGameEnd())
            {
                // 置けるところがなかったらパス
                if (Board.IsReversible())
                {
                    if (m_PlayerList[TurnKeeper.NowTurnColor] is PlayerHuman)
                    {
                        return;
                    }
                    else
                    {
                        // ターンをまわす
                        this.NextTurn();

                        // 画面情報を更新する
                        this.UpdateDisplay();

                        // ゲーム続行
                        this.RunGame();
                    }
                }
                else
                {
                    ReversiInformation.Add(m_PlayerList[TurnKeeper.NowTurnColor].Name + "はパスしました。");
                    
                    // ターンをまわす
                    TurnKeeper.ChangeOnlyColor();

                    // 画面情報を更新する
                    this.UpdateDisplay();

                    // ゲーム続行
                    this.RunGame();
                }
            }
            else
            {
                // 結果表示
                this.DisplayResult();
            }
        }
        #endregion
        #endregion



    }
}
