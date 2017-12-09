using Utils;
using UnityEngine;

namespace Manager
{
    public class SystemManager : Singleton<SystemManager>
    {

        [HideInInspector]
        public bool isPause = false;
        [HideInInspector]
        public bool sceneTouchEnabled = false;

        public override void Initialize()
        {
            base.Initialize();

            sceneTouchEnabled = true;
        }

        public override void Release()
        {
            base.Release();
        }


        public void Resume()
        {
            if (!isPause)
            {
                Time.timeScale = 0;
                isPause = true;
            }
            else
            {
                Time.timeScale = 1;
                isPause = false;
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}