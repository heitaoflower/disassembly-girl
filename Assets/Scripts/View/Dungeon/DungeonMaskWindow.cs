namespace View.Dungeon
{
    [View(prefabPath = "UI/Dungeon/DungeonMaskWindow", isSingleton = true, layer = ViewLayerTypes.Scene)]
    public class DungeonMaskWindow : BaseView
    {
        public UISprite maskLayerUI = null;

        public override void Show(object data = null)
        {
            base.Show(data);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void FadeIn(float duration = 0.0f, EventDelegate.Callback OnFadeInFinished = null)
        {
            TweenAlpha tween = maskLayerUI.GetComponent<TweenAlpha>();
            tween.enabled = true;
            tween.from = 0f;
            tween.to = 1f;
            tween.ResetToBeginning();
            tween.duration = duration;
            tween.PlayForward();
            tween.onFinished.Clear();

            if (OnFadeInFinished != null)
            {
                EventDelegate.Add(tween.onFinished, OnFadeInFinished);
            }
        }

        public void FadeOut(EventDelegate.Callback OnFadeOutFinished = null)
        {
            TweenAlpha tween = maskLayerUI.GetComponent<TweenAlpha>();
            tween.enabled = true;
            tween.from = 1.0f;
            tween.to = 0.0f;
            tween.ResetToBeginning();
            tween.duration = 0.5f;
            tween.PlayForward();
            tween.onFinished.Clear();

            if (OnFadeOutFinished != null)
            {
                EventDelegate.Add(tween.onFinished, OnFadeOutFinished);
            }
        }
    }
}