using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using TKCommon.Utility;
using TKCommon.Collections;

namespace TKCommon.Utility.Test
{
    /// <summary>
    /// SparseVectorテストクラス
    /// </summary>
    [TestFixture]
    public class SparseVectorTest
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
        /// SparseVectorの基本的な使用をテストする
        /// </summary>
        /// <param name="vectorX"></param>
        /// <param name="matrixA"></param>
        /// <param name="width"></param>
        /// <param name="expectedVectorY"></param>
        [TestCase()]
        public void TestSparseVectorNormal()
        {
            SparseVector<double> vector = new SparseVector<double>(0);
            vector.Add(1);
            vector.Add(2);
            vector.Add(3);
            List<double> expectedVector = new List<double> { 1, 2, 3 };
            int i = 0;
            foreach (double value in vector)
            {
                Assert.AreEqual(expectedVector[i], value, 0.001);
                i++;
            }

            vector = new SparseVector<double>(0) { 1, 2, 3 };
            i = 0;
            foreach (double value in vector)
            {
                Assert.AreEqual(expectedVector[i], value, 0.001);
                i++;
            }

            vector = new SparseVector<double>(0) { 1, 0, 3, 0, 0, 0, 8 };
            expectedVector = new List<double> { 1, 0, 3, 0, 0, 0, 8 };
            i = 0;
            foreach (double value in vector)
            {
                Assert.AreEqual(expectedVector[i], value, 0.001);
                i++;
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
