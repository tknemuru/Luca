using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace ReversiCommon.Searchs.Test
{
    /// <summary>
    /// NegaMax法探索基本動作テストクラス
    /// </summary>
    [TestFixture]
    public class NegaMaxBasicTest
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        //[TestCase()]
        //public void TestNegaMax()
        //{
        //    List<double> valueList = new List<double>() { -6, -1, -7, -4, -5, -2, -3, -8 };
        //    KeyValuePair<int, double> expectedValue = new KeyValuePair<int, double>(0, 6);
        //    Assert.That(new NegaMaxBasic(4, valueList).SearchBestValue(), Is.EqualTo(expectedValue));
        //}

        [TestCase()]
        public void TestNegaMax()
        {
            List<double> valueList = new List<double>() { 2, 4, 5, 7, 3, 5, 4, 3, 2, 6, 4, 1 };
            KeyValuePair<int, double> expectedValue = new KeyValuePair<int, double>(0, 5);
            Assert.That(new NegaMaxBasic(4, valueList).SearchBestValue(), Is.EqualTo(expectedValue));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
