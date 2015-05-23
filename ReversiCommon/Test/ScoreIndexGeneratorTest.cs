using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using ReversiCommon.Utility;
using ReversiCommon.Collections;
using ReversiCommon.Evaluators;

namespace ReversiCommon.Test
{
    /// <summary>
    /// スコアインデックス生成テストクラス
    /// </summary>
    [TestFixture]
    public class ScoreIndexGeneratorTest
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
        /// スコアインデックスが正しく取得されることをテストする
        /// </summary>
        /// <param name="orgIndex"></param>
        /// <param name="expectIndex"></param>
        [TestCase("corner2X5_1", "corner2X5", 29551, 29497, -1, 2)]
        [TestCase("corner2X5_2", "corner2X5", 29551, 29497, -1, 8)]
        [TestCase("corner2X5_3", "corner2X5", 29497, 29497, 1, 8)]
        [TestCase("corner2X5_4", "corner2X5", 49234, 9814, -1, 8)]
        [TestCase("corner2X5_5", "corner2X5", 9814, 9814, 1, 8)]
        public void TestScoreIndexList(string statekey, string indexName, int indexNo, int expectedNormalizeIndexNo, int expectedNormalizeType, int expectedCount)
        {
            DebugBoard.Read(statekey);
            List<ScoreIndex> indexList = ScoreIndexGenerator.GetIndexList();
            List<CostIndexGenerator> oldIndexList = CostIndexGenerator.GetIndexList();
            int count = 0;
            foreach (ScoreIndex index in indexList)
            {
                if (index.Key == (ScoreIndexEvaluator.GetScoreIndexKey(indexName, expectedNormalizeIndexNo.ToString())))
                {
                    Assert.That(index.Value, Is.EqualTo(expectedNormalizeType));
                    count++;
                }
            }
            Assert.That(count, Is.EqualTo(expectedCount));

            count = 0;
            foreach (CostIndexGenerator index in oldIndexList)
            {
                if (index.Name == indexName && index.Index == indexNo)
                {
                    double normalizeType = NormalizeIndex.GetNormalizeType(index.Name, index.Index);
                    int normalizeIndex = NormalizeIndex.Get(index.Name, index.Index);
                    Assert.That(normalizeType, Is.EqualTo(expectedNormalizeType));
                    Assert.That(normalizeIndex, Is.EqualTo(expectedNormalizeIndexNo));
                    count++;
                }
            }
            Assert.That(count, Is.EqualTo(expectedCount));
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
