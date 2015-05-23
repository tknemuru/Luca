using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using ReversiCommon.Utility;

namespace ReversiCommon.Test
{
    /// <summary>
    /// インデックス正規化テストクラス
    /// </summary>
    [TestFixture]
    public class NormalizationTest
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
        /// 色変換が正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase(48359, 10, 10689)]
        [TestCase(5373, 8, 1187)]
        [TestCase(999, 8, 5561)]
        public void TestChangeColor(int orgIndex, int digit, int expectIndex)
        {
            int newIndex = NormalizationUtility.ChangeColor(orgIndex, digit);
            Assert.That(newIndex, Is.EqualTo(expectIndex));
        }

        /// <summary>
        /// リバースが正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase(5373, 8, 95)]
        [TestCase(1187, 8, 6465)]
        public void TestReverse(int orgIndex, int digit, int expectIndex)
        {
            int newIndex = NormalizationUtility.Reverse(orgIndex, digit);
            Assert.That(newIndex, Is.EqualTo(expectIndex));
        }

        /// <summary>
        /// 色変換＆リバースが正しく行われることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase(5373, 8, 6465)]
        public void TestChangeColorAndReverse(int orgIndex, int digit, int expectIndex)
        {
            int newIndex = NormalizationUtility.ChangeColorReverse(orgIndex, digit);
            Assert.That(newIndex, Is.EqualTo(expectIndex));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
