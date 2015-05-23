using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PoAncare.Common.Index;
using PoAncare.Common.Collections;
using PoAncare.Common.Administrator;
using PoAncare.Common.Utility;
using JSN = PoAncare.Common.Utility.JsonUtility;

namespace PoAncare
{
    /// <summary>
    /// <para>盤クラス</para>
    /// </summary>
    public static class Board
    {
        #region "定数"
        /// <summary>
        /// <para>黒で初期化する座標位置</para>
        /// </summary>
        private static readonly List<int> INITIALIZE_BLACK_POINT = new List<int> { 45, 54 };
        
        /// <summary>
        /// <para>白で初期化する座標位置</para>
        /// </summary>
        private static readonly List<int> INITIALIZE_WHITE_POINT = new List<int> { 44, 55 };

        /// <summary>
        /// <para>イメージファイルURL：配置可能</para>
        /// </summary>
        private const string IMAGE_FILE_URL_MOBILITY = @"C:\visualstudio\PoAncare\Image\Mobility.gif";
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>マスタインデックスリスト</para>
        /// </summary>
        private static Dictionary<string, MasterIndex> m_MasterIndexList = new Dictionary<string, MasterIndex>();

        /// <summary>
        /// <para>ログスタック</para>
        /// </summary>
        private static Stack<List<ReverseLog>> m_ReverseLogs = new Stack<List<ReverseLog>>();

        /// <summary>
        /// <para>画面上に出力する盤情報</para>
        /// </summary>
        private static Dictionary<int, Disc> m_DiscList;

        /// <summary>
        /// <para>配置可能な座標のリスト</para>
        /// </summary>
        private static List<Disc> m_MobilityList;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// <para>コンストラクタ</para>
        /// </summary>
        static Board()
        {
            InitializeBoard();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>ゲームが終了したかどうかを判断する</para>
        /// </summary>
        /// <returns></returns>
        public static bool IsGameEnd()
        {
            return false;
        }

        /// <summary>
        /// <para>石を裏返す</para>
        /// </summary>
        public static void Reverse(Disc disc)
        {
            List<ReverseLog> reversLog = new List<ReverseLog>();

            foreach (KeyValuePair<string, MasterIndex> index in m_MasterIndexList)
            {
                if (index.Value.MobilityDiscs.Contains(disc.Point))
                {
                    // ログスタック格納用変数にログ情報を追加
                    reversLog.Add(new ReverseLog(index.Key, index.Value.NowIndex));
                    // 置いた座標の更新
                    m_MasterIndexList[index.Key].NowIndex = m_MasterIndexList[index.Key].GetPointResult(disc.Point);
                    m_MasterIndexList[index.Key].SetResultInfo(TurnKeeper.NowTurnColor.OppositeColor);
                    foreach (MasterIndex.Infulence infulIndex in m_MasterIndexList[index.Key].GetPointInfulenceList(disc.Point))
                    {
                        if (infulIndex.IndexName != "dummy")
                        {
                            // ログスタック格納用変数にログ情報を追加
                            reversLog.Add(new ReverseLog(infulIndex.IndexName, m_MasterIndexList[infulIndex.IndexName].NowIndex));
                            // 影響を受けるインデックスの更新
                            m_MasterIndexList[infulIndex.IndexName].NowIndex = infulIndex.ResultValue;
                            m_MasterIndexList[infulIndex.IndexName].SetResultInfo(TurnKeeper.NowTurnColor.OppositeColor);
                        }
                    }
                }
                else
                {
                    m_MasterIndexList[index.Key].SetResultInfo(TurnKeeper.NowTurnColor.OppositeColor);
                }
            }

            // ログスタックに追加
            m_ReverseLogs.Push(reversLog);

            // ターンをまわす
            TurnKeeper.ChangeTurn();
        }

        /// <summary>
        /// <para>手を戻す</para>
        /// </summary>
        public static void UndoReverse()
        {
            // スタックからログを取得
            List<ReverseLog> reversLogList = m_ReverseLogs.Pop();

            foreach (ReverseLog reversLog in reversLogList)
            {
                // インデックス情報を更新前に戻す
                m_MasterIndexList[reversLog.IndexName].NowIndex = reversLog.NowIndex;
                m_MasterIndexList[reversLog.IndexName].SetResultInfo(TurnKeeper.NowTurnColor.OppositeColor);
            }

            // ターンを戻す
            TurnKeeper.UndoTurn();
        }

        /// <summary>
        /// <para>全ての着手可能な手を返す</para>
        /// </summary>
        /// <returns></returns>
        public static List<Disc> GetAllPutablePointer()
        {
            // 配置可能な座標の一覧を取得する
            var mobilityList = from a in m_MasterIndexList.Values
                               where a.MobilityDiscs.Count > 0
                               select a.MobilityDiscs;

            List<int> retDiscNumbers = new List<int>();
            foreach (List<int> mobilitySet in mobilityList)
            {
                // 和集合を求める
                retDiscNumbers = retDiscNumbers.Union(mobilitySet).ToList();
            }

            List<Disc> retDiscs = new List<Disc>();
            foreach (int point in retDiscNumbers)
            {
                // 石クラスに変換する
                retDiscs.Add(new Disc(point, TurnKeeper.NowTurnColor));
            }

            return retDiscs;
        }

        /// <summary>
        /// <para>出力用石リストを更新する</para>
        /// </summary>
        public static void UpdateDiscList()
        {
            // 更新に必要なインデックスを取得する
            // TODO あとでちゃんとやるかも
            var indexList = from a in m_MasterIndexList.Values
                            where a.IndexName.Substring(0, 2) == "Al"
                            select a;

            foreach (var index in indexList)
            {
                foreach (int point in index.ContainsPoints)
                {
                    // 石を更新していく
                    m_DiscList[point] = new Disc(point, index.GetPointColor(point));
                }
            }

            // 配置可否情報を更新する
            m_MobilityList = GetAllPutablePointer();
        }

        /// <summary>
        /// <para>指定した座標のイメージファイルURLを取得する</para>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static string GetPointImageUrl(int point)
        {
            return (IsMobility(point)) ? IMAGE_FILE_URL_MOBILITY : m_DiscList[point].Color.ImageUrl;
        }

        /// <summary>
        /// <para>配置可能な座標ならtrueを返す</para>
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsMobility(int point)
        {
            var mobilitys = from a in m_MobilityList
                            where a.Point == point
                            select a;

            return (mobilitys.Count() > 0);
        }
        #endregion

        #region "内部メソッド"
        /// <summary>
        /// <para>ボードを初期化する</para>
        /// </summary>
        private static void InitializeBoard()
        {
            m_DiscList = new Dictionary<int,Disc>();
            for (int h = 1; h <= 8; h++)
            {
                for (int w = 1; w <= 8; w++)
                {
                    int point = ((h * 10) + w);
                    m_DiscList.Add(point, new Disc(point, GetInitializeColor(point)));
                }
            }

            foreach (IndexMetaData indexMetaData in Indexer.IndexMetaDataList)
            {
                int i = 0;
                int nowIndex = 0;
                foreach(int element in indexMetaData.Element)
                {
                    nowIndex += MathUtility.GetTernaryNotationPointIndex(m_DiscList[element].Color, i);
                    i++;
                }

                // マスタインデックスに初期値をセット
                m_MasterIndexList.Add(indexMetaData.IndexName, Indexer.GetMasterIndex(indexMetaData.IndexName, nowIndex, TurnKeeper.NowTurnColor));
            }
        }

        /// <summary>
        /// <para>初期化する色を返す</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static Color GetInitializeColor(int key)
        {
            if (INITIALIZE_BLACK_POINT.Contains(key))
            {
                return Color.Black;
            }
            else if (INITIALIZE_WHITE_POINT.Contains(key))
            {
                return Color.White;
            }
            else
            {
                return Color.Space;
            }
        }

        /// <summary>
        /// <para>着手可能ならばtrueを返す</para>
        /// </summary>
        /// <returns>true：着手可能／false：着手不可能</returns>
        private static bool IsMobility(Disc disc)
        {
            return true;
        }
        #endregion
        #endregion
    }
}
