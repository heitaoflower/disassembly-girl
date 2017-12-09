using Utils;
using System.Collections.Generic;
using System;
using Guide;
using Prototype;

namespace Manager
{
    public class GuideManager : Singleton<GuideManager>
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Release()
        {
            base.Release();
        }

        public void Trigger(GuideScriptID scriptID)
        {
            if (HasFinished(scriptID)) { return; }

            Type guideScriptType = null;

            if (guideScriptRegistry.TryGetValue((int)scriptID, out guideScriptType))
            {
                AbstractGuideScript script = (AbstractGuideScript)Activator.CreateInstance(guideScriptType);

                script.Start();
            }
            else
            {
                LogUtils.LogWarning("Not fount Guide script by ID " + scriptID);
            }
        }

        public bool HasFinished(GuideScriptID id)
        {
            UserData user = UserManager.GetInstance().user;

            return user.guides.Contains((int)id);
        }

        public void FinishGuide(GuideScriptID id)
        {
            UserData user = UserManager.GetInstance().user;
            user.guides.Add((int)id);
        }

        private static IDictionary<int, Type> guideScriptRegistry = new Dictionary<int, Type>()
        {
            { 1, typeof(GuideScript01)},
        };
    }
}