using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace JskyUwpLibs.Tool
{
    public sealed class LogFile
    {
        static string filePath = null;

        public static async void InitialFilePath(string name)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            filePath = file.Path.ToString();
        }


        public static bool WriteLog(string str)
        {
            if (String.IsNullOrEmpty(filePath)) return false;

            try
            {
                string time = DateTime.Now.ToString("hh:mm:ss.fff ");
                string result = time + str + Environment.NewLine;
                File.AppendAllText(filePath, result);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }


    }
}
