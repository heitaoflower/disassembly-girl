using UnityEngine;

namespace Map
{
    public class MapHome01Director : AbstractMapDirector {

        private MapElement cloud01 = null;

        private MapElement cloud02 = null;

        private MapElement cloud03 = null;
        
        private float cloudSpeed1 = 0.05f;

        private float cloudSpeed2 = 0.04f;

        override protected void RegisterElements()
        {
            if (transform.Find("Cloud01") != null)
            {
                cloud01 = transform.Find("Cloud01").gameObject.AddComponent<MapElement>();
                cloud01.OnInVisible = () => { cloud01.transform.localPosition = new Vector2(0f, cloud01.transform.localPosition.y); };
            }

            if (transform.Find("Cloud02") != null)
            {
                cloud02 = transform.Find("Cloud02").gameObject.AddComponent<MapElement>();
                cloud02.OnInVisible = () => { cloud02.transform.localPosition = new Vector2(0f, cloud02.transform.localPosition.y); };
            }

            if (transform.Find("Cloud03") != null)
            {
                cloud03 = transform.Find("Cloud03").gameObject.AddComponent<MapElement>();
                cloud03.OnInVisible = () => { cloud03.transform.localPosition = new Vector2(0f, cloud03.transform.localPosition.y); };
            }
        }

        void Update()
        {
            UpdateCloud();
        }

        private void UpdateCloud()
        {
            cloud01.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed1, Space.World);
            cloud02.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed2, Space.World);
            cloud03.transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed1, Space.World);
        }
    }
}