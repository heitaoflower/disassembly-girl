using Utils;

namespace Manager
{
    public abstract class SceneManager<T> : Singleton<T> where T : Singleton<T>
    {
        public abstract void EnterScene();

        public abstract void EnterSceneComplete();

        public abstract void ExitScene();
    }
}