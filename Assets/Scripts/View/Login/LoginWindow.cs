using UnityEngine;
using Utils;
using Orange.TransitionKit;
using Manager;

namespace View.Login
{
    [View(prefabPath ="UI/Login/LoginWindow", isSingleton =true, layer =ViewLayerTypes.Window)]
    public class LoginWindow : BaseView
    {
        public UIButton newBtn = null;

        public UIButton exitBtn = null;

        public UIButton loadBtn = null;

        public UISprite logoImg = null;

        void Start()
        {
            UserManager.GetInstance();

            UIEventListener.Get(newBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(loadBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(exitBtn.gameObject).onClick += OnClick;
        }

        public override void Show(object data = null)
        {
            base.Show(data);

            logoImg.gameObject.SetActive(true);
            TweenAlpha tween = logoImg.GetComponent<TweenAlpha>();
            tween.ResetToBeginning();
            tween.enabled = true;
            tween.PlayForward();

            if (!UserManager.GetInstance().HasUser())
            {
                loadBtn.isEnabled = false;
            }

            SoundManager.GetInstance().PlayBackgroundMusic(AudioRepository.BG_LOGIN.AsAudioClip(), 1.0f);
        }

        private void OnClick(GameObject go)
        {
            if (go == newBtn.gameObject)
            {
                UserManager.GetInstance().CreateUser();

                EnterGame(GlobalDefinitions.SCENE_HOME_INDEX);

                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == loadBtn.gameObject)
            {
                UserManager.GetInstance().LoadUser();

                EnterGame(GlobalDefinitions.SCENE_HOME_INDEX);

                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == exitBtn.gameObject)
            {
                SystemManager.GetInstance().Quit();
            }
        }

        private void EnterGame(int sceneIndex)
        {
            #region Transtion Logic
            FadeTransition transition = new FadeTransition()
            {
                nextScene = sceneIndex,
                fadedDelay = 0.3f,
                fadeToColor = Color.black
            };

            TransitionEngine.instance.transitionWithDelegate(transition);
            #endregion
        }

    }
}
