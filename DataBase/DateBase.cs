///
/// 封装基本数据库操作，方便上层使用
///

using SQLitePCL;

namespace DataBase
{
    /// <summary>
    /// 数据库类，可进行数据库的链接，数据项查询，数据项增加
    /// </summary>
    public sealed class DataBase
    {
        //private SQLiteConnection conn;

        public DataBase(string dataBaseName)
        {
            SQLitePrepare.InitialDataBase(dataBaseName);
            //conn = SQLitePrepare.GetConnetion(dataBaseName);
        }

        // 通过url查找对应的xml字符串
        public string select(string url)
        {
            //string sql = @"SELECT * FROM XmlCache WHERE Url = ?";
            //using (var statement = conn.Prepare(sql))
            //{
            //    statement.Bind(1, url);
            //    if (statement.Step() == SQLiteResult.ROW)
            //    {
            //        return (string)statement[1];
            //    }
            //}
            return string.Empty;
        }

        // 添加新项，url对应xml
        public void add(string url, string xml)
        {
            //string sql = @"INSERT INTO XmlCache (Url, Xml)
            //                           Values (?, ?)";
            //using (var statement = conn.Prepare(sql))
            //{
            //    statement.Bind(1, url);
            //    statement.Bind(2, xml);
            //    statement.Step();
            //}
        }
    }
}
