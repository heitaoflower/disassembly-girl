using UnityEngine;

namespace Utils
{
    public class CommonUtils
    {
        public void ChangeLayer(GameObject go)
        {
            Transform[] childs = go.GetComponentsInChildren<Transform>();
            for (int index = 0; index < childs.Length; index++)
            {
                childs[index].gameObject.layer = LayerMaskDefines.GUIDE.id;
            }
        }

    }
}