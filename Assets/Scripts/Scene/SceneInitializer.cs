using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Manager;

namespace Scene
{
    public class SceneInitializer : MonoBehaviour
    {
        void Awake()
        {
            LayerManager.GetInstance().RegisterSceneRoot();

            LayerManager.GetInstance().CloseAllViews();

            if (SceneManager.GetActiveScene().buildIndex == GlobalDefinitions.SCENE_HOME_INDEX)
            {
                HomeManager.GetInstance().EnterScene();
            }
            else
            {
                DungeonManager.GetInstance().EnterScene();
            }
       
        }
    }
}