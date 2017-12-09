using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance = null;

        public static T GetInstance()
        {
            return _instance;
        }

        public void SetInstance(T instance)
        {
            if (_instance == null)
            {
                _instance = instance;
            }
        }

        public virtual void Initialize()
        {
            LogUtils.LogDebug(GetType().FullName + " initialize success");
        }

        public virtual void Release()
        {
            LogUtils.LogDebug(GetType().FullName + " Release success");
        }
    }
}