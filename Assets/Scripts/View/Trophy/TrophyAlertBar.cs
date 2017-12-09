using UnityEngine;
using System;
using Manager;
using Prototype;

namespace View.Trophy
{
    [View(prefabPath = "UI/Trophy/TrophyAlertBar", isSingleton = true, layer = ViewLayerTypes.Alert)]
    public class TrophyAlertBar : BaseView
    {
        public UISprite alertImg = null;

        public UISprite iconImg = null;

        public override void Hide()
        {
            base.Hide();

            if (TrophyManager.GetInstance().alertTrophyDatas.Count != 0)
            {
                LayerManager.GetInstance().AddPopUpView<TrophyAlertBar>(TrophyManager.GetInstance().alertTrophyDatas.Dequeue());
            }
        }
        public override void Show(object data = null)
        {
            base.Show(data);

            iconImg.spriteName = ((TrophyData)data).iconID;

            TweenPosition tween = alertImg.gameObject.AddComponent<TweenPosition>();
            tween.to = new Vector3(0, (Screen.height - alertImg.height) / 2, 0);
            tween.from = alertImg.transform.localPosition;
            tween.steeperCurves = true;
            tween.style = UITweener.Style.Once;
            tween.delay = 0f;
            tween.duration = 0.5f;
            tween.ignoreTimeScale = false;
            EventDelegate.Add(tween.onFinished, () =>
            {  
                EventDelegate.Add(tween.onFinished, () =>
                {
                    Action callback = data as Action;
                    if (callback != null) { callback(); }
                    LayerManager.GetInstance().RemovePopUpView(this);     

                }, true );

                tween.delay = 2f;
                tween.PlayReverse();                
            }, true);

            tween.PlayForward();
        }

    }
}