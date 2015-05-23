using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using TKCommon.Collections;

namespace TKCommon.Collections.Test
{
    /// <summary>
    /// SparseMatrixテストクラス
    /// </summary>
    [TestFixture]
    public class SparseMatrixTest
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
        /// インデクサのテスト
        /// </summary>
        [TestCase()]
        public void TestIndexer()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            Assert.That(matrix[0, 1], Is.EqualTo(1));
            Assert.That(matrix[1, 2], Is.EqualTo(2));
            Assert.That(matrix[2, 0], Is.EqualTo(3));
            Assert.That(matrix[3, 2], Is.EqualTo(4));

            Assert.That(matrix[0, 0], Is.EqualTo(0));
            Assert.That(matrix[2, 1], Is.EqualTo(0));
            Assert.That(matrix[3, 1], Is.EqualTo(0));
        }

        /// <summary>
        /// 疎な部分を除いた列挙子のテスト
        /// </summary>
        [TestCase()]
        public void TestNoSparseKeyValues()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            Dictionary<int, double> dic = matrix.NoSparseKeyValues;

            Assert.That(dic.Count, Is.EqualTo(4));

            Assert.That(dic[1], Is.EqualTo(1));
            Assert.That(dic[5], Is.EqualTo(2));
            Assert.That(dic[6], Is.EqualTo(3));
            Assert.That(dic[11], Is.EqualTo(4));
        }

        /// <summary>
        /// 疎な部分を除いた特定の行のテスト
        /// </summary>
        [TestCase()]
        public void TestNoSparseKeyValuesOneRow()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            foreach (KeyValuePair<int, double> pair in matrix.NoSparseKeyValuesOneRow(2))
            {
                switch (pair.Key)
                {
                    case 6 :
                        Assert.That(pair.Value, Is.EqualTo(3));
                        break;
                    default :
                        Assert.That(pair.Value, Is.EqualTo(-1));
                        break;
                }
            }
        }

        /// <summary>
        /// 疎な部分を除いた特定の列のテスト
        /// </summary>
        [TestCase()]
        public void TestNoSparseKeyValuesOneColumn()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            foreach (KeyValuePair<int, double> pair in matrix.NoSparseKeyValuesOneColumn(2))
            {
                switch (pair.Key)
                {
                    case 5:
                        Assert.That(pair.Value, Is.EqualTo(2));
                        break;
                    case 11 :
                        Assert.That(pair.Value, Is.EqualTo(4));
                        break;
                    default:
                        Assert.That(pair.Value, Is.EqualTo(-1));
                        break;
                }
            }
        }

        /// <summary>
        /// XY合算の座標からXの座標を取得するテスト
        /// </summary>
        [TestCase()]
        public void TestDeserializeX()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            Assert.That(matrix.DeserializeX(5), Is.EqualTo(2));
            Assert.That(matrix.DeserializeX(6), Is.EqualTo(0));
        }

        /// <summary>
        /// XY合算の座標からYの座標を取得するテスト
        /// </summary>
        [TestCase()]
        public void TestDeserializeY()
        {
            SparseMatrix<double> matrix = new SparseMatrix<double>(4, 3, 0);

            // 0 1 0
            // 0 0 2
            // 3 0 0
            // 0 0 4
            matrix[0, 1] = 1;
            matrix[1, 2] = 2;
            matrix[2, 0] = 3;
            matrix[3, 2] = 4;

            Assert.That(matrix.DeserializeY(5), Is.EqualTo(1));
            Assert.That(matrix.DeserializeY(6), Is.EqualTo(2));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
