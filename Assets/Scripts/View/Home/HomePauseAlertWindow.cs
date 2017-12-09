using UnityEngine;
using Manager;

namespace View.Home
{
    [View(prefabPath = "UI/Home/HomePauseAlertWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class HomePauseAlertWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIButton exitBtn = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(exitBtn.gameObject).onClick += OnClick;
        }

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                LayerManager.GetInstance().RemovePopUpView(this);

                SystemManager.GetInstance().Resume();
            }
            else if (go == exitBtn.gameObject)
            {
                SystemManager.GetInstance().Quit();
            }
        }
    }
}