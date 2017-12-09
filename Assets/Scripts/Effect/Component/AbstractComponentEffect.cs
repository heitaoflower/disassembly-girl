using UnityEngine;
using System.Collections;
using Utils;

namespace Effect.Component
{
    public abstract class AbstractComponentEffect : MonoBehaviour
    {
        protected static readonly float GRAVITY = -2.0f;

        protected static readonly float SPIN = -10f;

        protected static readonly float MIN_FORCE = 0.5f;

        protected static readonly float MAX_FORCE = 1.0f;

        protected static readonly float MIN_SPEED = 0.2f;

        protected static readonly float MAX_SPEED = 0.4f;

        public float gravity = GRAVITY;
        public float spin = SPIN;
        public int angle = 0;

        public float minForce = MIN_FORCE;
        public float maxForce = MAX_FORCE;

        public float minSpeed = MIN_SPEED;
        public float maxSpeed = MAX_SPEED;

        public enum ComponentsEffectRegistry : int
        {
            C000,
            C001,
            C002,
            C003,
        }

        protected tk2dSprite sprite = null;

        protected tk2dSpriteAnimator animator = null;

        protected TrailRenderer trail = null;

        public bool spinEnabled = false;

        void Awake()
        {
            angle = Random.Range(0, 180);
            sprite = GetComponent<tk2dSprite>();
            animator = GetComponent<tk2dSpriteAnimator>();
            trail = GetComponentInChildren<TrailRenderer>();

            if (trail != null)
            {
                trail.GetComponent<Renderer>().sortingLayerName = SortingLayerDefines.MapObject.ToString();
            }

            if (animator != null)
            {
                animator.AnimationCompleted += OnAnimationCompleted;
                animator.AnimationEventTriggered += OnAnimationEventTriggered;
            }       
        }

        void Start()
        {
            OnAnimationStarted();
        }

        public static AbstractComponentEffect LoadRandomEffect()
        {
            ComponentsEffectRegistry[] effects = ComponentsEffectRegistry.GetValues(typeof(ComponentsEffectRegistry)) as ComponentsEffectRegistry[];
            ComponentsEffectRegistry item = effects[Random.Range(0, effects.Length)];
            
            GameObject prefab = ResourceUtils.GetPrefabObject(GlobalDefinitions.RESOURCE_PATH_EFFECT_COMPONET + item.ToString().Trim('C')) as GameObject;
            GameObject hit = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
     
            AbstractComponentEffect effect = hit.AddComponent<ComponentDefaultEffect>();
            effect.spinEnabled = true;

            return effect;
        }

        public abstract void Initaizlie(object data = null);

        private void OnAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int index)
        {
            StartCoroutine(TriggerTaskProcessor(animator, clip, index));
        }

        protected virtual IEnumerator TriggerTaskProcessor(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int index)
        {
            yield return null;
        }

        private void OnAnimationStarted()
        {
            StartCoroutine(StartTaskProcessor());
        }
        
        private void OnAnimationCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
        {
            StartCoroutine(CompleteTaskProcessor());
        }

        protected virtual IEnumerator CompleteTaskProcessor()
        {
            yield return null;
        }

        protected virtual IEnumerator StartTaskProcessor()
        {
            yield return null;
        }

        protected virtual void OnBecameInvisible()
        {
            Destroy(this.gameObject);
        }

        protected virtual void OnBecameVisible()
        {

        }

    }
}