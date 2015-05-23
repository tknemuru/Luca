using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReversiCommon.Searchs;

namespace ReversiCommon.Searchs.Test
{
    /// <summary>
    /// NegaMax法探索基本動作クラス
    /// </summary>
    public class NegaMaxBasic : NegaMaxBase<int>
    {
        #region "定数"
        /// <summary>
        /// キーの初期値
        /// </summary>
        private const int DEFAULT_KEY = 99;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 評価リスト
        /// </summary>
        private List<double> m_ValueList;

        /// <summary>
        /// 現在の評価リストインデックス
        /// </summary>
        private int m_NowValueIndex;

        /// <summary>
        /// 現在の深さ
        /// </summary>
        private int m_NowDepth;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        public NegaMaxBasic(int limit, List<double> valueList) : base(limit)
        {
            this.m_ValueList = valueList;
            this.m_NowDepth = 1;
            this.m_NowValueIndex = 0;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// 深さ制限に達した場合にはTrueを返す
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected override bool IsLimit(int limit)
        {
            this.m_NowDepth = limit;
            return (limit == this.m_LimitDepth);
        }

        /// <summary>
        /// 評価値を取得する
        /// </summary>
        /// <returns></returns>
        protected override KeyValuePair<int, double> GetEvaluate()
        {
            KeyValuePair<int, double> ret = new KeyValuePair<int, double>(DEFAULT_KEY, (this.m_ValueList[this.m_NowValueIndex] * -1));
            this.m_NowValueIndex++;
            return ret;
        }

        /// <summary>
        /// 全てのリーフを取得する
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<int, double> GetAllLeaf()
        {
            //int limit = (int)Math.Pow(2, this.m_NowDepth);

            Dictionary<int, double> retDic = new Dictionary<int, double>();

            if (this.m_NowDepth == (this.m_LimitDepth - 1))
            {
                for (int i = 0; i < 3; i++)
                {
                    retDic.Add(i, 0);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    retDic.Add(i, 0);
                }
            }

            return retDic;
        }

        /// <summary>
        /// 最大評価値の初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected override KeyValuePair<int, double> GetDefaultMaxKeyValue()
        {
            return new KeyValuePair<int, double>(DEFAULT_KEY, DEFAULT_ALPHA);
        }

        /// <summary>
        /// キーの初期値を取得する
        /// </summary>
        /// <returns></returns>
        protected override int GetDefaultKey()
        {
            return DEFAULT_KEY;
        }

        /// <summary>
        /// 探索の前処理を行う
        /// </summary>
        protected override void SearchSetUp(KeyValuePair<int, double> leaf)
        {
        }

        /// <summary>
        /// 探索の後処理を行う
        /// </summary>
        protected override void SearchTearDown()
        {
        }

        /// <summary>
        /// パスの前処理を行う
        /// </summary>
        protected override void PassSetUp()
        {
        }

        /// <summary>
        /// パスの後処理を行う
        /// </summary>
        protected override void PassTearDown()
        {
        }
        #endregion
        #endregion
    }
}
