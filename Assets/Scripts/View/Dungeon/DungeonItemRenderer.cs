using UnityEngine;
using System.Collections.Generic;
using Manager;
using Orange.TransitionKit;
using Utils;

namespace View.Dungeon
{
    public class DungeonItemRenderer : MonoBehaviour
    {
        public static string GetPrefabPath()
        {
            return GlobalDefinitions.RESOURCE_PATH_UI + "Dungeon/DungeonItemRenderer";
        }

        public UIButton iconBtn = null;

        private KeyValuePair<int, string> data = default(KeyValuePair<int, string>);

        public void Initialize(KeyValuePair<int, string> data)
        {
            this.data = data;

            iconBtn.normalSprite = data.Value;

            if (this.data.Key <= UserManager.GetInstance().user.dungeonIndex)
            {
                iconBtn.gameObject.SetActive(true);
                UIEventListener.Get(iconBtn.gameObject).onClick = (GameObject go)=> 
                {
                    DungeonManager.GetInstance().currentDungeonIndex = data.Key;

                    SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_PRESS.AsAudioClip());

                    LayerManager.GetInstance().RemovePopUpView<DungeonSelectWindow>();

                    #region Transtion Logic
                    FadeTransition transition = new FadeTransition()
                    {
                        nextScene = GlobalDefinitions.SCENE_DUNGEON_INDEX,
                        fadedDelay = 0.3f,
                        fadeToColor = Color.black
                    };

                    TransitionEngine.instance.transitionWithDelegate(transition);
                    #endregion
                };
            }
            else
            {
                iconBtn.gameObject.SetActive(false);
            }
        }
    }
}
