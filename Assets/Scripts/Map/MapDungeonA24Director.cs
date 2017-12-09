using UnityEngine;

namespace Map
{
    public class MapDungeonA24Director : AbstractMapDirector
    {
        private float cloud11Speed = 0.0035f;

        private float cloud07Speed = 0.0025f;

        private MapElement cloud11a = null;

        private MapElement cloud11b = null;

        private MapElement cloud07a = null;

        private MapElement cloud07b = null;

        protected override void RegisterElements()
        {
            if (transform.Find("Cloud11a") != null)
            {
                cloud11a = transform.Find("Cloud11a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Cloud11b") != null)
            {
                cloud11b = transform.Find("Cloud11b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Cloud07a") != null)
            {
                cloud07a = transform.Find("Cloud07a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Cloud07b") != null)
            {
                cloud07b = transform.Find("Cloud07b").gameObject.AddComponent<MapElement>();
            }
        }

        void Update()
        {
            cloud11a.transform.position = Vector3.Lerp(cloud11a.transform.position, cloud11a.transform.position + Vector3.right, cloud11Speed);
            cloud11b.transform.position = Vector3.Lerp(cloud11b.transform.position, cloud11b.transform.position + Vector3.right, cloud11Speed);

            if (cloud11a.transform.localPosition.x > 4.475f)
            {
                cloud11b.transform.localPosition = new Vector2(1.475f, cloud11b.transform.localPosition.y);
                cloud11a.transform.localPosition = new Vector2(-1.525f, cloud11a.transform.localPosition.y);
            }

            if (cloud11b.transform.localPosition.x > 4.475f)
            {
                cloud11a.transform.localPosition = new Vector2(1.475f, cloud11a.transform.localPosition.y);
                cloud11b.transform.localPosition = new Vector2(-1.525f, cloud11b.transform.localPosition.y);
            }

            cloud07a.transform.position = Vector3.Lerp(cloud07a.transform.position, cloud07a.transform.position + Vector3.right, cloud07Speed);
            cloud07b.transform.position = Vector3.Lerp(cloud07b.transform.position, cloud07b.transform.position + Vector3.right, cloud07Speed);

            if (cloud07a.transform.localPosition.x > 4.475f)
            {
                cloud07b.transform.localPosition = new Vector2(1.475f, cloud07b.transform.localPosition.y);
                cloud07a.transform.localPosition = new Vector2(-1.525f, cloud07a.transform.localPosition.y);
            }

            if (cloud07b.transform.localPosition.x > 4.475f)
            {
                cloud07a.transform.localPosition = new Vector2(1.475f, cloud07a.transform.localPosition.y);
                cloud07b.transform.localPosition = new Vector2(-1.525f, cloud07b.transform.localPosition.y);
            }
        }
    }
}