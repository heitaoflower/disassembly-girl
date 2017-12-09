using UnityEngine;
using System.Collections;
using System;

namespace View
{
    public class BaseView : MonoBehaviour
    {
        public virtual void Show(object data = null)
        {

        }

        public virtual void Hide()
        {

        }

        void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
                
        }

        protected IEnumerator TweenTransformTo(Transform transform, float time, Vector3 toPos, Vector3 toScale, float toRotation, Action OnTweenComplete)
        {
            Vector3 fromPos = transform.localPosition;
            Vector3 fromScale = transform.localScale;
            Vector3 euler = transform.localEulerAngles;
            float fromRotation = euler.z;

            for (float t = 0; t < time; t += tk2dUITime.deltaTime)
            {
                float nt = Mathf.Clamp01(t / time);
                nt = Mathf.Sin(nt * Mathf.PI * 0.5f);

                transform.localPosition = Vector3.Lerp(fromPos, toPos, nt);
                transform.localScale = Vector3.Lerp(fromScale, toScale, nt);
                euler.z = Mathf.Lerp(fromRotation, toRotation, nt);
                transform.localEulerAngles = euler;
                yield return null;
            }

            euler.z = toRotation;
            transform.localPosition = toPos;
            transform.localScale = toScale;
            transform.localEulerAngles = euler;

            if (OnTweenComplete != null)
            {
                OnTweenComplete();
            }
        }
    }
}
