using UnityEngine;

namespace Map
{
    public class MapDungeonA07Director : AbstractMapDirector
    {
        private float background01Speed = 0.0025f;

        private float background02Speed = 0.00125f;

        private float fan02Speed = 0.000525f;

        private MapElement background01a = null;

        private MapElement background01b = null;

        private MapElement background01c = null;

        private MapElement background01d = null;

        private MapElement background02a = null;

        private MapElement background02b = null;

        private MapElement background02c = null;

        private MapElement background02d = null;


        private MapElement fan02a = null;

        private MapElement fan02b = null;

        private MapElement fan02c = null;

        private MapElement fan02d = null;

        private MapElement fan02e = null;

        private MapElement fan02f = null;

        protected override void RegisterElements()
        {
            if (transform.Find("Background01a") != null)
            {
                background01a = transform.Find("Background01a").gameObject.AddComponent<MapElement>();               
            }

            if (transform.Find("Background01b") != null)
            {
                background01b = transform.Find("Background01b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background01c") != null)
            {
                background01c = transform.Find("Background01c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background01d") != null)
            {
                background01d = transform.Find("Background01d").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background02a") != null)
            {
                background02a = transform.Find("Background02a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background02b") != null)
            {
                background02b = transform.Find("Background02b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background02c") != null)
            {
                background02c = transform.Find("Background02c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Background02d") != null)
            {
                background02d = transform.Find("Background02d").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02a") != null)
            {
                fan02a = transform.Find("Fan02a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02b") != null)
            {
                fan02b = transform.Find("Fan02b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02c") != null)
            {
                fan02c = transform.Find("Fan02c").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02d") != null)
            {
                fan02d = transform.Find("Fan02d").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02e") != null)
            {
                fan02e = transform.Find("Fan02e").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Fan02f") != null)
            {
                fan02f = transform.Find("Fan02f").gameObject.AddComponent<MapElement>();
            }
        }

        void Update()
        {
            background01a.transform.position = Vector3.Lerp(background01a.transform.position, background01a.transform.position + Vector3.up, background01Speed);
            background01b.transform.position = Vector3.Lerp(background01b.transform.position, background01b.transform.position + Vector3.up, background01Speed);
            background01c.transform.position = Vector3.Lerp(background01c.transform.position, background01c.transform.position + Vector3.up, background01Speed);
            background01d.transform.position = Vector3.Lerp(background01d.transform.position, background01d.transform.position + Vector3.up, background01Speed);

            if (background01a.transform.localPosition.y > 2.975)
            {
                background01b.transform.localPosition = new Vector2(background01b.transform.localPosition.x, 0.975f);
                background01a.transform.localPosition = new Vector2(background01a.transform.localPosition.x, -1.025f);
            }

            if (background01b.transform.localPosition.y > 2.975)
            {
                background01a.transform.localPosition = new Vector2(background01a.transform.localPosition.x, 0.975f);
                background01b.transform.localPosition = new Vector2(background01b.transform.localPosition.x, -1.025f);
            }

            if (background01c.transform.localPosition.y > 2.975)
            {
                background01d.transform.localPosition = new Vector2(background01d.transform.localPosition.x, 0.975f);
                background01c.transform.localPosition = new Vector2(background01c.transform.localPosition.x, -1.025f);
            }

            if (background01d.transform.localPosition.y > 2.975)
            {
                background01c.transform.localPosition = new Vector2(background01c.transform.localPosition.x, 0.975f);
                background01d.transform.localPosition = new Vector2(background01d.transform.localPosition.x, -1.025f);
            }

            background02a.transform.position = Vector3.Lerp(background02a.transform.position, background02a.transform.position + Vector3.up, background02Speed);
            background02b.transform.position = Vector3.Lerp(background02b.transform.position, background02b.transform.position + Vector3.up, background02Speed);
            background02c.transform.position = Vector3.Lerp(background02c.transform.position, background02c.transform.position + Vector3.up, background02Speed);
            background02d.transform.position = Vector3.Lerp(background02d.transform.position, background02d.transform.position + Vector3.up, background02Speed);

            if (background02a.transform.localPosition.y > 2.975)
            {
                background02b.transform.localPosition = new Vector2(background02b.transform.localPosition.x, 0.975f);
                background02a.transform.localPosition = new Vector2(background02a.transform.localPosition.x, -1.025f);
            }

            if (background02b.transform.localPosition.y > 2.975)
            {
                background02a.transform.localPosition = new Vector2(background02a.transform.localPosition.x, 0.975f);
                background02b.transform.localPosition = new Vector2(background02b.transform.localPosition.x, -1.025f);
            }

            if (background02c.transform.localPosition.y > 2.975)
            {
                background02d.transform.localPosition = new Vector2(background02d.transform.localPosition.x, 0.975f);
                background02c.transform.localPosition = new Vector2(background02c.transform.localPosition.x, -1.025f);
            }

            if (background02d.transform.localPosition.y > 2.975)
            {
                background02c.transform.localPosition = new Vector2(background02c.transform.localPosition.x, 0.975f);
                background02d.transform.localPosition = new Vector2(background02d.transform.localPosition.x, -1.025f);
            }

            fan02a.transform.position = Vector3.Lerp(fan02a.transform.position, fan02a.transform.position + Vector3.up, fan02Speed);
            fan02b.transform.position = Vector3.Lerp(fan02b.transform.position, fan02b.transform.position + Vector3.up, fan02Speed);
            fan02c.transform.position = Vector3.Lerp(fan02c.transform.position, fan02c.transform.position + Vector3.up, fan02Speed);
            fan02d.transform.position = Vector3.Lerp(fan02d.transform.position, fan02d.transform.position + Vector3.up, fan02Speed);
            fan02e.transform.position = Vector3.Lerp(fan02e.transform.position, fan02e.transform.position + Vector3.up, fan02Speed);
            fan02f.transform.position = Vector3.Lerp(fan02f.transform.position, fan02f.transform.position + Vector3.up, fan02Speed);

            if (fan02a.transform.localPosition.y > 2)
            {
                fan02a.transform.localPosition = new Vector2(fan02a.transform.localPosition.x, -0.05f);
            }

            if (fan02b.transform.localPosition.y > 2)
            {
                fan02b.transform.localPosition = new Vector2(fan02b.transform.localPosition.x, -0.05f);
            }

            if (fan02c.transform.localPosition.y > 2)
            {
                fan02c.transform.localPosition = new Vector2(fan02c.transform.localPosition.x, -0.05f);
            }

            if (fan02d.transform.localPosition.y > 2)
            {
                fan02d.transform.localPosition = new Vector2(fan02d.transform.localPosition.x, -0.05f);
            }

            if (fan02e.transform.localPosition.y > 2)
            {
                fan02e.transform.localPosition = new Vector2(fan02e.transform.localPosition.x, -0.05f);
            }

            if (fan02f.transform.localPosition.y > 2)
            {
                fan02f.transform.localPosition = new Vector2(fan02f.transform.localPosition.x, -0.05f);
            }
        }
    }
}