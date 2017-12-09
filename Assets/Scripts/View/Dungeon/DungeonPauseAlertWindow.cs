using UnityEngine;
using Manager;
using Orange.TransitionKit;
using Utils;

namespace View.Dungeon
{
    [View(prefabPath ="UI/Dungeon/DungeonPauseAlertWindow", isSingleton =true, layer =ViewLayerTypes.Window)]
    public class DungeonPauseAlertWindow : BaseView
    {
        public UIButton exitBtn = null;

        public UIButton replayBtn = null;

        public UIButton homeBtn = null;

        public UIButton closeBtn = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(exitBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(replayBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(homeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;

            SystemManager.GetInstance().Resume();
        }

        private void OnClick(GameObject go)
        {
            if (go == exitBtn.gameObject)
            {
                SystemManager.GetInstance().Quit();
            }
            else if (go == replayBtn.gameObject)
            {
                SystemManager.GetInstance().Resume();

                LayerManager.GetInstance().RemovePopUpView(this);

                #region Transtion Logic
                FadeTransition transition = new FadeTransition()
                {
                    nextScene = GlobalDefinitions.SCENE_DUNGEON_INDEX,
                    fadedDelay = 0.3f,
                    fadeToColor = Color.black
                };

                TransitionEngine.instance.transitionWithDelegate(transition);
                #endregion
            }
            else if (go == homeBtn.gameObject)
            {
                SystemManager.GetInstance().Resume();

                LayerManager.GetInstance().RemovePopUpView(this);

                #region Transtion Logic
                FadeTransition transition = new FadeTransition()
                {
                    nextScene = GlobalDefinitions.SCENE_HOME_INDEX,
                    fadedDelay = 0.3f,
                    fadeToColor = Color.black
                };

                TransitionEngine.instance.transitionWithDelegate(transition);
                #endregion
            }
            else if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                SystemManager.GetInstance().Resume();

                LayerManager.GetInstance().RemovePopUpView(this);
            }
        }
    }
}