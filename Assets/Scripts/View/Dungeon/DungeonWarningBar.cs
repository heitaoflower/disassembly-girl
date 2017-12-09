using UnityEngine;
using System;
using Manager;

namespace View.Dungeon
{
    [View(prefabPath = "UI/Dungeon/DungeonWarningBar", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class DungeonWarningBar : BaseView
    {
        public UISprite warningImg = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            TweenPosition tween = warningImg.gameObject.AddComponent<TweenPosition>();
            tween.to = Vector3.zero;
            tween.from = warningImg.transform.localPosition;
            tween.steeperCurves = true;
            tween.style = UITweener.Style.Once;
            tween.delay = 0.5f;
            tween.duration = 0.2f;
            tween.ignoreTimeScale = false;
            EventDelegate.Add(tween.onFinished, () =>
            {
                EventDelegate.Add(tween.onFinished, () => 
                {
                    Action callback = data as Action ;            
                    if (callback != null) { callback(); }
                    LayerManager.GetInstance().RemovePopUpView(this);
                }, true);

                tween.to = new Vector3((Screen.width + warningImg.width), 0, 0);
                tween.from = Vector3.zero;
                tween.style = UITweener.Style.Once;
                tween.duration = 0.2f;
                tween.delay = 1.5f;                
                tween.ResetToBeginning();
                tween.PlayForward();
            }, true);

            tween.PlayForward();
        }

    }
}