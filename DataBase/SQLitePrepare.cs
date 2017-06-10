using SQLitePCL;

namespace DataBase
{
    /// <summary>
    /// 数据库静态基类，用于创建数据库，进行链接
    /// </summary>
    internal static class SQLitePrepare
    {
        public static SQLiteConnection conn;

        public static SQLiteConnection GetConnetion()
        {
            conn = new SQLiteConnection("xml.db");

            string sql = @"DROP TABLE IF EXISTS XmlCache";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }

            sql = @"CREATE TABLE XmlCache (Url VARCHAR( 100 ) PRIMARY KEY NOT NULL,
                                           Xml VARCHAR( 300000 ))";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }

            // Turn on Foreign Key constraints
            sql = @"PRAGMA foreign_keys = ON";
            using (var statement = conn.Prepare(sql))
            {
                statement.Step();
            }
            return conn;
        }
    }
}
