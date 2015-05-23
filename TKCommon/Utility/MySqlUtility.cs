using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

namespace TKCommon.Utility
{
    /// <summary>
    /// <para>MySQL操作クラス</para>
    /// </summary>
    public static class MySqlUtility
    {
        #region "定数"
        /// <summary>
        /// <para>接続文字列</para>
        /// </summary>
        private const string CONNECTION_STRING = @"server=cent_reversi; user id=root; password=webas0118; database=REVERSI; pooling=false";

        /// <summary>
        /// タイムアウトの初期値
        /// </summary>
        private const int DEFAULT_TIME_OUT = 30;
        #endregion

        #region "メンバ変数"
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static MySqlUtility()
        {
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// <para>SQLコマンドを実行する</para>
        /// </summary>
        /// <param name="cmd"></param>
        public static void ExecuteCommand(List<string> cmdList)
        {
            MySqlTransaction tran;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    try
                    {
                        foreach (string cmd in cmdList)
                        {
                            new MySqlCommand(cmd, conn, tran).ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch (MySqlException sqlEx)
                    {
                        tran.Rollback();
                        throw sqlEx;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <para>SQLコマンドを実行する</para>
        /// </summary>
        /// <param name="cmd"></param>
        public static void ExecuteCommand(string cmd)
        {
            MySqlTransaction tran;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    try
                    {
                        new MySqlCommand(cmd, conn, tran).ExecuteNonQuery();
                        tran.Commit();
                    }
                    catch (MySqlException sqlEx)
                    {
                        tran.Rollback();
                        throw sqlEx;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <para>DataAdapterで読み込む</para>
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable ReadDataAdapter(string cmdText, int timeout = DEFAULT_TIME_OUT)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(cmdText, conn);
                    if (timeout != DEFAULT_TIME_OUT) { cmd.CommandTimeout = timeout; }
                    new MySqlDataAdapter(cmd).Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// <para>DataAdapterで読み込む</para>
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DataTable ReadDataAdapterNoTimeOut(string cmdText)
        {
            return ReadDataAdapter(cmdText, 0);
        }



        /// <summary>
        /// <para>DataAdapterで読み込む</para>
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static void ReadDataAdapter(string cmd,DataTable dt)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    new MySqlDataAdapter(cmd, conn).Fill(dt);
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}
