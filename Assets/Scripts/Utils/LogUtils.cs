using UnityEngine;

using System;

namespace Utils
{
    public class LogUtils
    {

        public enum LogLevel
        {
            DEBUG = 0,
            INFO,
            WARNING,
            ERROR,
            NOLOG,
        }

        private static LogLevel _currentLevel = LogLevel.DEBUG;

        public LogLevel level
        {
            get { return _currentLevel; }
            set { _currentLevel = value; }
        }

        public static void LogDebug(params object[] msgParams)
        {
            if (_currentLevel <= LogLevel.DEBUG)
            {
                Debug.Log(GetLogStr(LogLevel.DEBUG, msgParams));
            }
        }

        public static void LogInfo(params object[] msgParams)
        {
            if (_currentLevel <= LogLevel.INFO)
            {
                Debug.Log(GetLogStr(LogLevel.INFO, msgParams));
            }
        }

        public static void LogWarning(params object[] msgParams)
        {
            if (_currentLevel <= LogLevel.WARNING)
            {
                Debug.LogWarning(GetLogStr(LogLevel.WARNING, msgParams));
            }
        }

        public static void LogError(params object[] msgParams)
        {
            if (_currentLevel <= LogLevel.ERROR)
            {
                Debug.LogError(GetLogStr(LogLevel.ERROR, msgParams));
            }
        }

        public static string GetLogStr(LogLevel level, params object[] msgParams)
        {
            string str = string.Format("[{0}] [{1}]", DateTime.Now.ToLongTimeString(), level);

            foreach (var msg in msgParams)
            {
                str += string.Format(" {0} ", msg);
            }

            return str;
        }
    }
}
