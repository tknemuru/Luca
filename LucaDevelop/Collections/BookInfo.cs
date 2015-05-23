using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucaDevelop.Collections
{
    /// <summary>
    /// 定石情報クラス
    /// </summary>
    public class BookInfo
    {
        #region "定数"
        #endregion

        #region "メンバ変数" 
        /// <summary>
        /// パターン
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 各値
        /// </summary>
        public List<double> Values { get; set; }

        /// <summary>
        /// 評価値の合計
        /// </summary>
        public double SumValue
        {
            get
            {
                double sum = 0;
                foreach (double value in this.Values)
                {
                    sum += value;
                }
                return sum;
            }
        }

        /// <summary>
        /// 測定された回数
        /// </summary>
        public double SumCount
        {
            get
            {
                return this.Values.Count();
            }
        }

        /// <summary>
        /// 平均値
        /// </summary>
        public double Avg
        {
            get
            {
                return ((double)this.SumValue / (double)this.SumCount);
            }
        }

        /// <summary>
        /// 分散
        /// </summary>
        public double Disperse
        {
            get
            {
                double avg = this.Avg;
                double disp = 0D;
                foreach (double value in this.Values)
                {
                    disp += Math.Pow((value - avg), 2);
                }
                return (disp / (double)this.Values.Count);
            }
        }

        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pattern"></param>
        public BookInfo(string pattern, double score)
        {
            this.Pattern = pattern;
            this.Values = new List<double>();
            this.Values.Add(score);
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
