using UnityEngine;
using System.Collections.Generic;

namespace Map
{
    public class MapDungeonA14Director : AbstractMapDirector
    {
        private float skeleton01Speed = 0.0025f;

        private float platfond01Speed = 0.0025f;

        private float skeleton02Speed = 0.0020f;

        private float skeleton03Speed = 0.0020f;

        private float window01Speed = 0.0015f;

        private float fan02Speed = 0.0015f;

        private string[] skeleton02Names = new string[] { "Skeleton02a", "Skeleton02b", "Skeleton02c", "Skeleton02d", "Skeleton02e", "Skeleton02f", "Skeleton02g" };

        private string[] window01Names = new string[] { "Window01a", "Window01b", "Window01c", "Window01d", "Window01e", "Window01f", "Window01g", "Window01h", "Window01i", "Window01j", "Window01k", "Window01l" };

        private string[] fan02Names = new string[] { "Fan02a", "Fan02b", "Fan02c", "Fan02d", "Fan02e",
                                                     "Fan02f", "Fan02g", "Fan02h", "Fan02i", "Fan02j",
                                                     "Fan02k", "Fan02l", "Fan02m", "Fan02n", "Fan02o",
                                                     "Fan02p", "Fan02q", "Fan02r"};

        private MapElement skeleton01a = null;

        private MapElement skeleton01b = null;

        private MapElement platfond01a = null;

        private MapElement platfond01b = null;

        private IList<MapElement> skeleton02s = null;

        private MapElement skeleton03a = null;

        private MapElement skeleton03b = null;

        private IList<MapElement> window01s = null;

        private IList<MapElement> fan02s = null;

        protected override void RegisterElements()
        {
            if (transform.Find("Skeleton01a") != null)
            {
                skeleton01a = transform.Find("Skeleton01a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Skeleton01b") != null)
            {
                skeleton01b = transform.Find("Skeleton01b").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Platfond01a") != null)
            {
                platfond01a = transform.Find("Platfond01a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Platfond01b") != null)
            {
                platfond01b = transform.Find("Platfond01b").gameObject.AddComponent<MapElement>();
            }

            skeleton02s = new List<MapElement>();
            for (int index = 0; index < skeleton02Names.Length; index++)
            {
                if (transform.Find(skeleton02Names[index]) != null)
                {
                    MapElement skeleton02 = transform.Find(skeleton02Names[index]).gameObject.AddComponent<MapElement>();

                    skeleton02s.Add(skeleton02);
                }
            }

            if (transform.Find("Skeleton03a") != null)
            {
                skeleton03a = transform.Find("Skeleton03a").gameObject.AddComponent<MapElement>();
            }

            if (transform.Find("Skeleton03b") != null)
            {
                skeleton03b = transform.Find("Skeleton03b").gameObject.AddComponent<MapElement>();
            }

            window01s = new List<MapElement>();
            for (int index = 0; index < window01Names.Length; index++)
            {
                if (transform.Find(window01Names[index]) != null)
                {
                    MapElement window01 = transform.Find(window01Names[index]).gameObject.AddComponent<MapElement>();

                    window01s.Add(window01);
                }
            }

            fan02s = new List<MapElement>();
            for (int index = 0; index < fan02Names.Length; index++)
            {
                if (transform.Find(fan02Names[index]) != null)
                {
                    MapElement fan02 = transform.Find(fan02Names[index]).gameObject.AddComponent<MapElement>();

                    fan02s.Add(fan02);
                }
            }

        }

        void Update()
        {
            UpdateHook();
        }

        private void UpdateHook()
        {
            skeleton01a.transform.position = Vector3.Lerp(skeleton01a.transform.position, skeleton01a.transform.position + Vector3.left, skeleton01Speed);
            skeleton01b.transform.position = Vector3.Lerp(skeleton01b.transform.position, skeleton01b.transform.position + Vector3.left, skeleton01Speed);

            platfond01a.transform.position = Vector3.Lerp(platfond01a.transform.position, platfond01a.transform.position + Vector3.left, platfond01Speed);
            platfond01b.transform.position = Vector3.Lerp(platfond01b.transform.position, platfond01b.transform.position + Vector3.left, platfond01Speed);

            if (skeleton01a.transform.localPosition.x < -3f)
            {
                skeleton01a.transform.localPosition = new Vector2(3.225f, skeleton01a.transform.localPosition.y);
            }

            if (skeleton01b.transform.localPosition.x < -3f)
            {
                skeleton01b.transform.localPosition = new Vector2(3.225f, skeleton01b.transform.localPosition.y);
            }

            if (platfond01a.transform.localPosition.x < -1.525)
            {
                platfond01b.transform.localPosition = new Vector2(1.475f, platfond01b.transform.localPosition.y);
                platfond01a.transform.localPosition = new Vector2(4.475f, platfond01a.transform.localPosition.y);
            }

            if (platfond01b.transform.localPosition.x < -1.525)
            {
                platfond01a.transform.localPosition = new Vector2(1.475f, platfond01a.transform.localPosition.y);
                platfond01b.transform.localPosition = new Vector2(4.475f, platfond01b.transform.localPosition.y);
            }

            for (int index = 0; index < skeleton02s.Count; index++)
            {
                skeleton02s[index].transform.position = Vector3.Lerp(skeleton02s[index].transform.position, skeleton02s[index].transform.position + Vector3.left, skeleton02Speed);

                if (skeleton02s[index].transform.localPosition.x < -0.3f)
                {
                    skeleton02s[index].transform.localPosition = new Vector2(3.3f, skeleton02s[index].transform.localPosition.y);
                }
            }

            skeleton03a.transform.position = Vector3.Lerp(skeleton03a.transform.position, skeleton03a.transform.position + Vector3.left, skeleton03Speed);
            skeleton03b.transform.position = Vector3.Lerp(skeleton03b.transform.position, skeleton03b.transform.position + Vector3.left, skeleton03Speed);

            if (skeleton03a.transform.localPosition.x < -1.525)
            {
                skeleton03b.transform.localPosition = new Vector2(1.475f, skeleton03b.transform.localPosition.y);
                skeleton03a.transform.localPosition = new Vector2(4.475f, skeleton03a.transform.localPosition.y);
            }

            if (skeleton03b.transform.localPosition.x < -1.525)
            {
                skeleton03a.transform.localPosition = new Vector2(1.475f, skeleton03a.transform.localPosition.y);
                skeleton03b.transform.localPosition = new Vector2(4.475f, skeleton03b.transform.localPosition.y);
            }

            for (int index = 0; index < window01s.Count; index++)
            {
                window01s[index].transform.position = Vector3.Lerp(window01s[index].transform.position, window01s[index].transform.position + Vector3.left, window01Speed);

                if (window01s[index].transform.localPosition.x < -0.15f)
                {
                    window01s[index].transform.localPosition = new Vector2(3.15f, window01s[index].transform.localPosition.y);
                }
            }

            for (int index = 0; index < fan02s.Count; index++)
            {
                fan02s[index].transform.position = Vector3.Lerp(fan02s[index].transform.position, fan02s[index].transform.position + Vector3.left, fan02Speed);

                if (fan02s[index].transform.localPosition.x < -0.05f)
                {
                    fan02s[index].transform.localPosition = new Vector2(3.65f, fan02s[index].transform.localPosition.y);
                }
            }
        }
    }
}