using UnityEngine;
namespace Map
{
    public class MapDungeonA21Director : AbstractMapDirector
    {
        private float background03Speed = 0.0015f;

        private float background04Speed = 0.0015f;

        private float background05Speed = 0.0015f;

        private float background06Speed = 0.0025f;

        private MapElement background03a = null;

        private MapElement background03b = null;

        private MapElement background03c = null;

        private MapElement background03d = null;

        private MapElement background04a = null;

        private MapElement background04b = null;

        private MapElement background04c = null;

        private MapElement background04d = null;

        private MapElement background05a = null;

        private MapElement background05b = null;

        private MapElement background05c = null;

        private MapElement background05d = null;

        private MapElement background06a = null;

        private MapElement background06b = null;

        private MapElement background06c = null;

        private MapElement background06d = null;

        protected override void RegisterElements()
        {
            if (transform.Find("Background05a") != null)
            {
                background05a = transform.Find("Background05a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background05b") != null)
            {
                background05b = transform.Find("Background05b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background05c") != null)
            {
                background05c = transform.Find("Background05c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background05d") != null)
            {
                background05d = transform.Find("Background05d").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background04a") != null)
            {
                background04a = transform.Find("Background04a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background04b") != null)
            {
                background04b = transform.Find("Background04b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background04c") != null)
            {
                background04c = transform.Find("Background04c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background04d") != null)
            {
                background04d = transform.Find("Background04d").gameObject.AddComponent<MapElement>();
            }
            
            if (transform.Find("Background03a") != null)
            {
                background03a = transform.Find("Background03a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background03b") != null)
            {
                background03b = transform.Find("Background03b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background03c") != null)
            {
                background03c = transform.Find("Background03c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background03d") != null)
            {
                background03d = transform.Find("Background03d").gameObject.AddComponent<MapElement>();
            }

            //
            if (transform.Find("Background06a") != null)
            {
                background06a = transform.Find("Background06a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background06b") != null)
            {
                background06b = transform.Find("Background06b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background06c") != null)
            {
                background06c = transform.Find("Background06c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background06d") != null)
            {
                background06d = transform.Find("Background06d").gameObject.AddComponent<MapElement>();
            }
        }

        public void Update()
        {
            background05a.transform.position = Vector3.Lerp(background05a.transform.position, background05a.transform.position + Vector3.down, background05Speed);
            background05b.transform.position = Vector3.Lerp(background05b.transform.position, background05b.transform.position + Vector3.down, background05Speed);
            background05c.transform.position = Vector3.Lerp(background05c.transform.position, background05c.transform.position + Vector3.down, background05Speed);
            background05d.transform.position = Vector3.Lerp(background05d.transform.position, background05d.transform.position + Vector3.down, background05Speed);

            if (background05a.transform.localPosition.y < -1f)
            {
                background05b.transform.localPosition = new Vector2(background05b.transform.localPosition.x, 1f);
                background05a.transform.localPosition = new Vector2(background05a.transform.localPosition.x, 3f);
            }

            if (background05b.transform.localPosition.y < -1f)
            {
                background05a.transform.localPosition = new Vector2(background05a.transform.localPosition.x, 1f);
                background05b.transform.localPosition = new Vector2(background05b.transform.localPosition.x, 3f);
            }

            if (background05c.transform.localPosition.y < -1f)
            {
                background05d.transform.localPosition = new Vector2(background05d.transform.localPosition.x, 1f);
                background05c.transform.localPosition = new Vector2(background05c.transform.localPosition.x, 3f);
            }

            if (background05d.transform.localPosition.y < -1f)
            {
                background05c.transform.localPosition = new Vector2(background05c.transform.localPosition.x, 1f);
                background05d.transform.localPosition = new Vector2(background05d.transform.localPosition.x, 3f);
            }

            
            background04a.transform.position = Vector3.Lerp(background04a.transform.position, background04a.transform.position + Vector3.down, background04Speed);
            background04b.transform.position = Vector3.Lerp(background04b.transform.position, background04b.transform.position + Vector3.down, background04Speed);
            background04c.transform.position = Vector3.Lerp(background04c.transform.position, background04c.transform.position + Vector3.down, background04Speed);
            background04d.transform.position = Vector3.Lerp(background04d.transform.position, background04d.transform.position + Vector3.down, background04Speed);

            if (background04a.transform.localPosition.y < -1f)
            {
                background04b.transform.localPosition = new Vector2(background04b.transform.localPosition.x, 1f);
                background04a.transform.localPosition = new Vector2(background04a.transform.localPosition.x, 3f);
            }

            if (background04b.transform.localPosition.y < -1f)
            {
                background04a.transform.localPosition = new Vector2(background04a.transform.localPosition.x, 1f);
                background04b.transform.localPosition = new Vector2(background04b.transform.localPosition.x, 3f);
            }

            if (background04c.transform.localPosition.y < -1f)
            {
                background04d.transform.localPosition = new Vector2(background04d.transform.localPosition.x, 1f);
                background04c.transform.localPosition = new Vector2(background04c.transform.localPosition.x, 3f);
            }

            if (background04d.transform.localPosition.y < -1f)
            {
                background04c.transform.localPosition = new Vector2(background04c.transform.localPosition.x, 1f);
                background04d.transform.localPosition = new Vector2(background04d.transform.localPosition.x, 3f);
            }

            
            background03a.transform.position = Vector3.Lerp(background03a.transform.position, background03a.transform.position + Vector3.down, background03Speed);
            background03b.transform.position = Vector3.Lerp(background03b.transform.position, background03b.transform.position + Vector3.down, background03Speed);
            background03c.transform.position = Vector3.Lerp(background03c.transform.position, background03c.transform.position + Vector3.down, background03Speed);
            background03d.transform.position = Vector3.Lerp(background03d.transform.position, background03d.transform.position + Vector3.down, background03Speed);

            if (background03a.transform.localPosition.y < -1f)
            {
                background03b.transform.localPosition = new Vector2(background03b.transform.localPosition.x, 1f);
                background03a.transform.localPosition = new Vector2(background03a.transform.localPosition.x, 3f);
            }

            if (background03b.transform.localPosition.y < -1f)
            {
                background03a.transform.localPosition = new Vector2(background03a.transform.localPosition.x, 1f);
                background03b.transform.localPosition = new Vector2(background03b.transform.localPosition.x, 3f);
            }

            if (background03c.transform.localPosition.y < -1f)
            {
                background03d.transform.localPosition = new Vector2(background03d.transform.localPosition.x, 1f);
                background03c.transform.localPosition = new Vector2(background03c.transform.localPosition.x, 3f);
            }

            if (background03d.transform.localPosition.y < -1f)
            {
                background03c.transform.localPosition = new Vector2(background03c.transform.localPosition.x, 1f);
                background03d.transform.localPosition = new Vector2(background03d.transform.localPosition.x, 3f);
            }

            //
            background06a.transform.position = Vector3.Lerp(background06a.transform.position, background06a.transform.position + Vector3.down, background06Speed);
            background06b.transform.position = Vector3.Lerp(background06b.transform.position, background06b.transform.position + Vector3.down, background06Speed);
            background06c.transform.position = Vector3.Lerp(background06c.transform.position, background06c.transform.position + Vector3.down, background06Speed);
            background06d.transform.position = Vector3.Lerp(background06d.transform.position, background06d.transform.position + Vector3.down, background06Speed);

            if (background06a.transform.localPosition.y < -1f)
            {
                background06b.transform.localPosition = new Vector2(background06b.transform.localPosition.x, 1f);
                background06a.transform.localPosition = new Vector2(background06a.transform.localPosition.x, 3f);
            }

            if (background06b.transform.localPosition.y < -1f)
            {
                background06a.transform.localPosition = new Vector2(background06a.transform.localPosition.x, 1f);
                background06b.transform.localPosition = new Vector2(background06b.transform.localPosition.x, 3f);
            }

            if (background06c.transform.localPosition.y < -1f)
            {
                background06d.transform.localPosition = new Vector2(background06d.transform.localPosition.x, 1f);
                background06c.transform.localPosition = new Vector2(background06c.transform.localPosition.x, 3f);
            }

            if (background06d.transform.localPosition.y < -1f)
            {
                background06c.transform.localPosition = new Vector2(background06c.transform.localPosition.x, 1f);
                background06d.transform.localPosition = new Vector2(background06d.transform.localPosition.x, 3f);
            }
        }


    }
}
