using System.Collections.Generic;
using UnityEngine;
namespace Map
{
    public class MapDungeonA06Director : AbstractMapDirector
    {
        private IList<MapElement> hook01s = null;

        private float hook01Speed = 0.005f;

        private string[] hook01Names = new string[] { "Hook01a", "Hook01b", "Hook01c", "Hook01d", "Hook01e", "Hook01f", "Hook01g" , "Hook01h" };

        protected override void RegisterElements()
        {
            hook01s = new List<MapElement>();

            for (int index = 0; index < hook01Names.Length; index++)
            {
                if (transform.Find(hook01Names[index]) != null)
                {
                    MapElement hook01 = transform.Find(hook01Names[index]).gameObject.AddComponent<MapElement>();
 
                    hook01s.Add(hook01);
                }
            }

        }

        void Update()
        {
            UpdateHook();
        }

        private void UpdateHook()
        {
            for (int index = 0; index < hook01s.Count; index++)
            {
                hook01s[index].transform.position = Vector3.Lerp(hook01s[index].transform.position, hook01s[index].transform.position + Vector3.right, hook01Speed);

                if (hook01s[index].transform.localPosition.x > 3.225f)
                {
                    hook01s[index].transform.localPosition = new Vector2(-0.725f, hook01s[index].transform.localPosition.y);
                }
            }
        }

    }
}