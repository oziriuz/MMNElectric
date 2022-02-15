using System.Configuration;

namespace Helpers
{
    public class Helper
    {
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        // date time formats
        public const string datePattern = "dd.MM.yyyy";
        public const string timePattern = "HH:mm:ss";
        public const string timeLogPattern = "HHmmss";
        public const string dateStampPattern = "yyyy-MM-dd";
        public const string dateLogPattern = "yyyyMMdd";

        // errorlog file
        public static string pathErrorLog = ConfigurationManager.AppSettings["PathToErrorLogs"];
        public static string errorLog = $"ErrLog{System.DateTime.Now.ToString(dateLogPattern)}{System.DateTime.Now.ToString(timeLogPattern)}";

    }
}
