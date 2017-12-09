using UnityEngine;

namespace Map
{
    public class MapDungeonA22Director : AbstractMapDirector
    {
        private float cloud08Speed = 0.0035f;

        private float cloud07Speed = 0.0025f;

        private MapElement cloud08a = null;

        private MapElement cloud08b = null;

        private MapElement cloud07a = null;

        private MapElement cloud07b = null;

        protected override void RegisterElements()
        {
            if (transform.Find("Cloud08a") != null)
            {
                cloud08a = transform.Find("Cloud08a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Cloud08b") != null)
            {
                cloud08b = transform.Find("Cloud08b").gameObject.AddComponent<MapElement>();
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
            cloud08a.transform.position = Vector3.Lerp(cloud08a.transform.position, cloud08a.transform.position + Vector3.right, cloud08Speed);
            cloud08b.transform.position = Vector3.Lerp(cloud08b.transform.position, cloud08b.transform.position + Vector3.right, cloud08Speed);

            if (cloud08a.transform.localPosition.x > 4.475f)
            {
                cloud08b.transform.localPosition = new Vector2(1.475f, cloud08b.transform.localPosition.y);
                cloud08a.transform.localPosition = new Vector2(-1.525f, cloud08a.transform.localPosition.y);
            }

            if (cloud08b.transform.localPosition.x > 4.475f)
            {
                cloud08a.transform.localPosition = new Vector2(1.475f, cloud08a.transform.localPosition.y);
                cloud08b.transform.localPosition = new Vector2(-1.525f, cloud08b.transform.localPosition.y);
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