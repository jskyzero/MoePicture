///
/// 用于创建“标准化”的数据库链接
///

using Microsoft.Data.Sqlite;

namespace DataBase
{
    /// <summary>
    /// 数据库静态基类，用于创建数据库，进行链接
    /// </summary>
    internal class SQLitePrepare
    {
        public static void InitialDataBase(string dataBaseName)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=" + dataBaseName))
            {
                db.Open();
                string sql = @"DROP TABLE IF EXISTS XmlCache;
                               CREATE TABLE XmlCache (Url VARCHAR( 100 ) PRIMARY KEY NOT NULL,
                                                      Xml VARCHAR( 300000 ));
                               PRAGMA foreign_keys = ON;";

                SqliteCommand createTable = new SqliteCommand(sql, db);
                createTable.ExecuteReader();
            }
        }
    }
}
