using UnityEngine;
using Manager;
using Utils;

namespace View.Dungeon
{
    [View(prefabPath = "UI/Dungeon/DungeonReadyBar", isSingleton =true, layer =ViewLayerTypes.Window)]
    public class DungeonReadyBar : BaseView
    {
        public UISprite readyImg = null;

        public UISprite goImg = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            EventBox.Add(CustomEvent.ENTER_SCENE_COMPLETE, OnEnterSceneComplete);
        }

        public override void Dispose()
        {
            EventBox.RemoveAll(this);
        }

        private void OnEnterSceneComplete(object data)
        {
            TweenPosition tween = readyImg.gameObject.AddComponent<TweenPosition>();
            tween.to = Vector3.zero;
            tween.from = readyImg.transform.localPosition;
            tween.steeperCurves = true;
            tween.style = UITweener.Style.Once;
            tween.delay = 0.5f;
            tween.duration = 0.2f;
            tween.ignoreTimeScale = false;
            EventDelegate.Add(tween.onFinished, () =>
            {
                tween.to = new Vector3((Screen.width + readyImg.width), 0, 0);
                tween.from = Vector3.zero;
                tween.style = UITweener.Style.Once;
                tween.duration = 0.2f;
                tween.delay = 0.5f;
                tween.ResetToBeginning();
                tween.PlayForward();
                TweenGoImage();

            }, true);
            tween.PlayForward();
        }

        private void TweenGoImage()
        {
            TweenPosition tween = goImg.gameObject.AddComponent<TweenPosition>();
            tween.to = Vector3.zero;
            tween.from = goImg.transform.localPosition;
            tween.style = UITweener.Style.Once;
            tween.delay = 0.5f;
            tween.duration = 0.2f;
            tween.ignoreTimeScale = false;
            EventDelegate.Add(tween.onFinished, () =>
            {
                tween.to = new Vector3((Screen.width + goImg.width), 0, 0);
                tween.from = Vector3.zero;
                tween.style = UITweener.Style.Once;
                tween.duration = 0.2f;
                tween.delay = 0.5f;            
                tween.ResetToBeginning();
                EventDelegate.Add(tween.onFinished, ()=> 
                {
                    LayerManager.GetInstance().RemovePopUpView(this);
                    DungeonManager.GetInstance().StartBattle();
                },true);

                tween.PlayForward();
            }, true);
            tween.PlayForward();
        }
    }
}