using UnityEngine;
using Orange.TransitionKit;
using Utils;
using Manager;

namespace View.Dungeon
{
    [View(prefabPath ="UI/Dungeon/DungeonResultWindow", isSingleton =true, layer =ViewLayerTypes.Window)]
    public class DungeonResultWindow : BaseView
    {
        public UIButton homeBtn = null;

        public UIButton nextBtn = null;

        public UIButton replayBtn = null;

        public UIButton exitBtn = null;

        public UISprite winImg = null;

        public UISprite loseImg = null;

        public override void Show(object data = null)
        {
            base.Show();

            UIEventListener.Get(homeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(nextBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(replayBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(exitBtn.gameObject).onClick += OnClick;
    
            if ((BattleResult)data == BattleResult.Win)
            {
                winImg.gameObject.SetActive(true);
                nextBtn.gameObject.SetActive(true);
            }
            else
            {
                loseImg.gameObject.SetActive(true);
                exitBtn.gameObject.SetActive(true);

            }
        }

        private void OnClick(GameObject go)
        {
            if (go == homeBtn.gameObject)
            {
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
            else if (go == nextBtn.gameObject)
            {
                DungeonManager.GetInstance().currentDungeonIndex = DungeonManager.GetInstance().currentDungeonIndex + 1;

                LayerManager.GetInstance().RemovePopUpView(this);

                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_PRESS.AsAudioClip());

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
            else if (go == replayBtn.gameObject)
            {
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
            else if (go == exitBtn.gameObject)
            {
                SystemManager.GetInstance().Quit();
            }
        }
    }
}
