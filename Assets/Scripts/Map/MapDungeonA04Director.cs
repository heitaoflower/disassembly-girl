using UnityEngine;
using System.Collections;

namespace Map
{
    public class MapDungeonA04Director : AbstractMapDirector
    {
        private MapElement cloud03a = null;

        private MapElement cloud04a = null;

        private MapElement cloud04b = null;

        private MapElement cloud04c = null;

        private MapElement crow01a = null;

        private float cloudSpeed1 = 0.05f;

        private float cloudSpeed2 = 0.04f;

        private float crowSpeed1 = 0.06f;

        override protected void RegisterElements()
        {
            if (transform.Find("Cloud03a") != null)
            {
                cloud03a = transform.Find("Cloud03a").gameObject.AddComponent<MapElement>();
                cloud03a.OnInVisible = () => { cloud03a.transform.localPosition = new Vector2(0f, cloud03a.transform.localPosition.y); };
            }

            if (transform.Find("Cloud04a") != null)
            {
                cloud04a = transform.Find("Cloud04a").gameObject.AddComponent<MapElement>();
                cloud04a.OnInVisible = () => { cloud04a.transform.localPosition = new Vector2(0f, cloud04a.transform.localPosition.y); };
            }

            if (transform.Find("Cloud04b") != null)
            {
                cloud04b = transform.Find("Cloud04b").gameObject.AddComponent<MapElement>();
                cloud04b.OnInVisible = () => { cloud04b.transform.localPosition = new Vector2(0f, cloud04b.transform.localPosition.y); };
            }

            if (transform.Find("Cloud04c") != null)
            {
                cloud04c = transform.Find("Cloud04c").gameObject.AddComponent<MapElement>();
                cloud04c.OnInVisible = () => { cloud04c.transform.localPosition = new Vector2(0f, cloud04c.transform.localPosition.y); };
            }

            if (transform.Find("Crow01a") != null)
            {
                crow01a = transform.Find("Crow01a").gameObject.AddComponent<MapElement>();
                crow01a.OnInVisible = () => { crow01a.transform.localPosition = new Vector2(0f, crow01a.transform.localPosition.y); };
            }
        }

        void Update()
        {
            UpdateCloud();
        }

        private void UpdateCloud()
        {
            cloud03a.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed1, Space.World);
            cloud04a.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed2, Space.World);
            cloud04b.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed2, Space.World);
            cloud04c.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed2, Space.World);

            crow01a.transform.Translate(Vector3.right * Time.deltaTime * crowSpeed1, Space.World);
        }

    }
}