using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using ReversiCommon.Utility;

namespace ReversiCommon.Utility.Test
{
    /// <summary>
    /// リバーシ用変換ユーティリティテスト
    /// </summary>
    [TestFixture]
    public class ReversiConvertUtilityTest
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// GetContinuityPowsが正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase("D3", "34")]
        [TestCase("C4", "43")]
        [TestCase("B4", "42")]
        [TestCase("e3", "35")]
        [TestCase("f5", "56")]
        public void TestConvertToThorPoint(string point, string expectPoint)
        {
            string a = ReversiConvertUtility.ConvertToThorPoint(point);
            Assert.That(a, Is.EqualTo(expectPoint));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
