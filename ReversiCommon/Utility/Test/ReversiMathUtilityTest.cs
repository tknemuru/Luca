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
    /// ReversiMathUtilityテストクラス
    /// </summary>
    [TestFixture]
    public class ReversiMathUtilityTest
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
        [TestCase(4, 80)]
        [TestCase(5, 242)]
        [TestCase(6, 728)]
        [TestCase(7, 2186)]
        [TestCase(8, 6560)]
        [TestCase(9, 19682)]
        [TestCase(10, 59048)]
        public void TestGetContinuityPows(int digit, int expectNumber)
        {
            int a = ReversiMathUtility.GetMaxIndex(digit);
            Assert.That(a, Is.EqualTo(expectNumber));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
