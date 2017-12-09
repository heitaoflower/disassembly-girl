using UnityEngine;

namespace Utils
{
    public class ResourceUtils
    {
        public static T GetComponent<T>(string path) where T : Component
        {
            GameObject prototype = GetPrefabObject(path);
 
            T component = GameObject.Instantiate(prototype).GetComponent<T>();

            return component;
        }

        public static T GetComponent<T>(string path, Transform parent) where T : Component
        {
            T component = GetComponent<T>(path);

            component.transform.SetParent(parent);
            component.transform.localPosition = Vector3.zero;

            return component;
        }

        public static T AddAndGetComponent<T>(string path) where T : Component
        {
            GameObject prototype = GetPrefabObject(path);
            T component = GameObject.Instantiate(prototype).AddComponent<T>();

            return component;
        }

        public static AudioClip GetAudioClip(string path)
        {
            AudioClip audioClip = Resources.Load(path) as AudioClip;

            if (audioClip == null)
            {
                Debug.Log(string.Format("Not found AudioClip resource by Path {0}", path));
            }
            return audioClip;
        }

        public static GameObject GetPrefabObject(string path)
        {
            GameObject prefabObject = Resources.Load(path) as GameObject;

            if (prefabObject == null)
            {
                Debug.Log(string.Format("Not found Prefab resource by Path {0}", path));
            }
            return prefabObject;
        }
    }
}