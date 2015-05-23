using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using TKCommon.Utility;

namespace TKCommon.Debugger
{
    /// <summary>
    /// <para>ログストップウォッチクラス</para>
    /// </summary>
    public static class StopWatchLogger
    {
        #region "定数"
        /// <summary>
        /// <para>デバッグモードの場合はTrue</para>
        /// </summary>
        public const bool IS_DEBUG = true;

        /// <summary>
        /// <para>コンソールに出力する場合はTrue</para>
        /// </summary>
        private const bool IS_CONSOLE = false;

        /// <summary>
        /// <para>詳細な計測時間を出力する場合はTrue</para>
        /// </summary>
        private const bool IS_DETAIL_TIME_DISPLAY = true;
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// <para>ストップウォッチディクショナリ</para>
        /// </summary>
        private static Dictionary<string, Stopwatch> m_StopWatchDic;

        /// <summary>
        /// <para>時間記録ディクショナリ</para>
        /// </summary>
        private static Dictionary<string, long> m_Times;

        /// <summary>
        /// <para>詳細な時間記録ディクショナリ</para>
        /// </summary>
        private static Dictionary<string, double> m_DetailTimes;
        #endregion

        #region "コンストラクタ"
        static StopWatchLogger()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            ClearAllEventTimes();
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>文字列をコンソールに出力する</para>
        /// </summary>
        /// <param name="str"></param>
        public static void OutputStringToConsole(string str)
        {
            if (!IS_DEBUG) { return; }
            Debug.WriteLine(str);
        }

        /// <summary>
        /// <para>イベント時間の計測を開始する</para>
        /// </summary>
        /// <param name="eventName"></param>
        public static void StartEventWatch(string eventName)
        {
            if (!IS_DEBUG) { return; }
            if (!m_StopWatchDic.ContainsKey(eventName))
            {
                m_StopWatchDic.Add(eventName, new Stopwatch());
                m_StopWatchDic[eventName].Start();
            }
            else
            {
                m_StopWatchDic[eventName].Start();
            }
        }

        /// <summary>
        /// <para>イベント時間の計測を終了する</para>
        /// </summary>
        /// <param name="eventName"></param>
        public static void StopEventWatch(string eventName)
        {
            if (!IS_DEBUG) { return; }
            Debug.Assert(m_StopWatchDic.ContainsKey(eventName), "計測が開始されていないイベントの計測を終了しようとしました。");
            m_StopWatchDic[eventName].Stop();
            AddEventTimes(eventName);
        }

        /// <summary>
        /// <para>イベントの時間を記録する</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="ms"></param>
        private static void AddEventTimes(string eventName)
        {
            if (!IS_DEBUG) { return; }
            Debug.Assert(m_StopWatchDic.ContainsKey(eventName) || !(m_StopWatchDic[eventName].IsRunning), "計測が完了していないイベントの計測を記録しようとしました。");
            if (!m_Times.ContainsKey(eventName))
            {
                m_Times.Add(eventName, m_StopWatchDic[eventName].ElapsedMilliseconds);
                m_DetailTimes.Add(eventName, (double)m_StopWatchDic[eventName].ElapsedTicks / (double)Stopwatch.Frequency);
            }
            else
            {
                m_Times[eventName] = m_Times[eventName] + m_StopWatchDic[eventName].ElapsedMilliseconds;
                m_DetailTimes[eventName] = m_DetailTimes[eventName] + ((double)m_StopWatchDic[eventName].ElapsedTicks / (double)Stopwatch.Frequency);
            }
            m_StopWatchDic[eventName].Reset();
        }

        /// <summary>
        /// <para>記録したすべてのイベントの時間を出力する</para>
        /// </summary>
        public static void DisplayAllEventTimes()
        {
            if (!IS_DEBUG) { return; }
            //foreach(KeyValuePair<string, long> time in m_Times)
            //{
            //    Debug.WriteLine(time.Key + "：" + time.Value.ToString());
            //}

            if (IS_DETAIL_TIME_DISPLAY)
            {
                foreach (KeyValuePair<string, double> dTime in m_DetailTimes)
                {
                    Debug.WriteLine(dTime.Key + "：" + dTime.Value.ToString());
                }
            }
        }

        /// <summary>
        /// <para>記録したすべてのイベントの時間を出力する</para>
        /// </summary>
        public static void WriteAllEventTimes(string file)
        {
            foreach (KeyValuePair<string, double> dTime in m_DetailTimes)
            {
                FileUtility.WriteLine((dTime.Key + "：" + dTime.Value.ToString()), file);
            }
        }

        /// <summary>
        /// <para>イベント時間記録ディクショナリをクリアする</para>
        /// </summary>
        public static void ClearAllEventTimes()
        {
            if (!IS_DEBUG) { return; }
            m_Times = new Dictionary<string, long>();
            m_StopWatchDic = new Dictionary<string, Stopwatch>();
            m_DetailTimes = new Dictionary<string, double>();
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
