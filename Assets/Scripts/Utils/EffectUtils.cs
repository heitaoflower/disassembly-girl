using UnityEngine;
using System.Collections;
using System;
using Manager;
using Effect.Component;

namespace Utils
{
    public class EffectUtils
    {
        public static void Drop(GameObject go, Vector2 hitPosition)
        {
            ComponentDefaultEffect componentEffect = go.AddComponent<ComponentDefaultEffect>();
            componentEffect.spinEnabled = false;
            componentEffect.minForce = 0.5f;
            componentEffect.maxForce = 1.0f;
            componentEffect.gravity = -6.0f;
            componentEffect.minSpeed = 0.4f;
            componentEffect.maxSpeed = 0.6f;

            if (hitPosition.x > go.transform.position.x)
            {
                componentEffect.angle = UnityEngine.Random.Range(90, 180);
            }
            else if (hitPosition.x < go.transform.position.x)
            {
                componentEffect.angle = UnityEngine.Random.Range(0, 90);
            }
            else
            {
                componentEffect.angle = 90;
            }
        }

        public static Task FadeOut(tk2dSprite sprite, float speed, Action OnFinished = null)
        {
            return new Task(_FadeOut(sprite, speed, OnFinished));
        }

        public static Task FadeIn(tk2dSprite sprite, float speed, Action OnFinished = null)
        {
            return new Task(_FadeIn(sprite, speed, OnFinished));
        }

        public static void Visible(tk2dSprite sprite, bool value)
        {
            if (value)
            {
                sprite.color = new Color(255, 255, 255, 1f);
            }
            else
            {
                sprite.color = new Color(255, 255, 255, 0f);
            }
        }

        private static IEnumerator _FadeOut(tk2dSprite sprite, float speed, Action OnFinished = null)
        {
            float alpha = sprite.color.a;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * speed;
                sprite.color = new Color(255, 255, 255, alpha);
                yield return null;
            }

            if (OnFinished != null) { OnFinished(); }
        }

        private static IEnumerator _FadeIn(tk2dSprite sprite, float speed, Action OnFinished = null)
        {
            float alpha = .0f;

            while (alpha < 1.0f)
            {
                alpha += Time.deltaTime * speed;
                sprite.color = new Color(255, 255, 255, alpha);
                yield return null;
            }

            if (OnFinished != null) { OnFinished(); }
        }
    }
}