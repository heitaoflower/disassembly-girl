using UnityEngine;

namespace Map
{
    public class MapDungeonA23Director : AbstractMapDirector
    {

        private float cloud08Speed = 0.0035f;

        private float cloud07Speed = 0.0025f;

        private float tree02Speed = 0.0035f;

        private float tree03Speed = 0.0025f;

        private MapElement cloud08a = null;

        private MapElement cloud08b = null;

        private MapElement cloud07a = null;

        private MapElement cloud07b = null;

        private MapElement tree02a = null;

        private MapElement tree02b = null;

        private MapElement tree03a = null;

        private MapElement tree03b = null;

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

            if (transform.Find("Tree02a") != null)
            {
                tree02a = transform.Find("Tree02a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Tree02b") != null)
            {
                tree02b = transform.Find("Tree02b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Tree03a") != null)
            {
                tree03a = transform.Find("Tree03a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Tree03b") != null)
            {
                tree03b = transform.Find("Tree03b").gameObject.AddComponent<MapElement>();
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

            tree02a.transform.position = Vector3.Lerp(tree02a.transform.position, tree02a.transform.position + Vector3.right, tree02Speed);
            tree02b.transform.position = Vector3.Lerp(tree02b.transform.position, tree02b.transform.position + Vector3.right, tree02Speed);

            if (tree02a.transform.localPosition.x > 4.475f)
            {
                tree02b.transform.localPosition = new Vector2(1.475f, tree02b.transform.localPosition.y);
                tree02a.transform.localPosition = new Vector2(-1.525f, tree02a.transform.localPosition.y);
            }

            if (tree02b.transform.localPosition.x > 4.475f)
            {
                tree02a.transform.localPosition = new Vector2(1.475f, tree02a.transform.localPosition.y);
                tree02b.transform.localPosition = new Vector2(-1.525f, tree02b.transform.localPosition.y);
            }

            tree03a.transform.position = Vector3.Lerp(tree03a.transform.position, tree03a.transform.position + Vector3.right, tree03Speed);
            tree03b.transform.position = Vector3.Lerp(tree03b.transform.position, tree03b.transform.position + Vector3.right, tree03Speed);

            if (tree03a.transform.localPosition.x > 4.475f)
            {
                tree03b.transform.localPosition = new Vector2(1.475f, tree03b.transform.localPosition.y);
                tree03a.transform.localPosition = new Vector2(-1.525f, tree03a.transform.localPosition.y);
            }

            if (tree03b.transform.localPosition.x > 4.475f)
            {
                tree03a.transform.localPosition = new Vector2(1.475f, tree03a.transform.localPosition.y);
                tree03b.transform.localPosition = new Vector2(-1.525f, tree03b.transform.localPosition.y);
            }
        }
    }
}