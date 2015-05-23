using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LucaGraph.Logic;
using System.Data;
using TKCommon.Utility;

namespace LucaGraph
{
    public partial class FormGraph : Form
    {
        #region "定数"
        private static readonly Brush POINT_BRUSH = Brushes.Blue;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormGraph()
        {
            InitializeComponent();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// Startボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Draw();
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// グラフを描画する
        /// </summary>
        private void Draw()
        {
            //Graphics gr = this.pbxGraph.CreateGraphics();
            //gr.FillEllipse(POINT_BRUSH, 100, 100, 3, 3);
            //gr.FillEllipse(POINT_BRUSH, 101, 101, 3, 3);
            //gr.FillEllipse(POINT_BRUSH, 102, 102, 3, 3);

            GraphLogic logic = new GraphLogic();

            // 描画対象のデータを取得
            DataTable dt = logic.GetAnalysisData("hor_vert2", 13);

            // xの比率を取得する
            double xRatio = ((double)this.pbxGraph.Width / (double)MathUtility.GetPows(3, 8));

            // yの比率を取得する
            double yRatio = ((double)this.pbxGraph.Height / (double)64);

            // 比率に従って描画していく
            Graphics gr = this.pbxGraph.CreateGraphics();
            foreach (DataRow row in dt.Rows)
            {
                double x = (double)int.Parse(row["INDEX_NO"].ToString()) * xRatio;
                double y = (double)int.Parse(row["THEORY_BLACK_SCORE"].ToString()) * yRatio;

                gr.FillEllipse(POINT_BRUSH, (int)x, (int)y, 3, 3);
            }
        }
        #endregion
        #endregion

    }
}
