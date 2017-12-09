using UnityEngine;

namespace Utils
{
    public class AudioBean
    {
        public string name = null;

        public AudioBean(string name)
        {
            this.name = name;
        }

        public AudioClip AsAudioClip()
        {
            return ResourceUtils.GetAudioClip(GlobalDefinitions.RESOURCE_PATH_AUDIO + name);
        }
    }

    public class AudioRepository
    {
        public static readonly AudioBean BG_HOME = new AudioBean("BGM/HOME");

        public static readonly AudioBean BG_LOGIN = new AudioBean("BGM/LOGIN");

        public static readonly AudioBean COMMON_BUTTON_PRESS = new AudioBean("UI/COMMON_BUTTON_PRESS");

        public static readonly AudioBean COMMON_BUTTON_CLOSE = new AudioBean("UI/COMMON_BUTTON_CLOSE");

        public static AudioClip LoadExplosionAudio(string name)
        {
            return ResourceUtils.GetAudioClip(GlobalDefinitions.RESOURCE_PATH_AUDIO + "EFFECT/EXPLOSION/" +  name);
        }

        public static AudioClip LoadBackgroundAudio(string name)
        {
            return ResourceUtils.GetAudioClip(GlobalDefinitions.RESOURCE_PATH_AUDIO + "BGM/" + name);
        }

    }
}