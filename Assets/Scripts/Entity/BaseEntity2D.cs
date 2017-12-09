using UnityEngine;
using Prototype;
using System.Collections;
using System.Collections.Generic;
using System;
using Orange.EffectKit;

namespace Entity
{
    public enum AnimationDefs
    {
        Compose, Idle, Run, Attack, Sleep, Puzzle, Amaze, Wake, Apply, Action1, Action2, Action3, Focus, Dead, Explosion, BackDash, Count
    }

    public enum AnimationTriggerDefs
    {
        WeaponApply, SkillApply, Count
    }


    public enum GameObjectElements
    {
        Unknown = 0,
        HUDContainer,
        ComponentsHolder
    }

    [RequireComponent(typeof(Motor2D))]
    public abstract class BaseEntity2D : MonoBehaviour
    {
        public static Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right };
        #region Variables
        private static readonly float GRAVITY = -5.0f;

        private static readonly float FADE_TIME = 2f;
        [HideInInspector]
        private Vector3 detalMovement = Vector3.zero;
        [HideInInspector]
        protected float groundDamping = default(float);
        [HideInInspector]
        protected float airDamping = default(float);
        [HideInInspector]
        public bool gravityDisabled = false;
        [HideInInspector]
        public bool isRunning = false;
        [HideInInspector]
        public Vector2 direction = Vector2.zero;
        [HideInInspector]
        public PrototypeObject data = default(PrototypeObject);
        [HideInInspector]
        public Motor2D motor2D = null;
        [HideInInspector]
        public ComponentsHolder componentsHolder = null;
        [HideInInspector]
        public Transform hudContainer = null;
        [HideInInspector]
        public IList<BlinkEffectScript> blinkEffectScripts = null;
        #endregion
        public Action OnDispose = null;

        private IList<MountPoint2D> mountPoints = null;

        private static float DEPTH = default(float);

        private void Awake()
        {
            componentsHolder = gameObject.transform.Find(GameObjectElements.ComponentsHolder.ToString()).gameObject.AddComponent<ComponentsHolder>();
            componentsHolder.owner = this;
            componentsHolder.AddComponents();

            hudContainer = gameObject.transform.Find(GameObjectElements.HUDContainer.ToString()).transform;

            motor2D = GetComponent<Motor2D>();

            mountPoints = new List<MountPoint2D>();

            blinkEffectScripts = gameObject.GetComponentsInChildren<BlinkEffectScript>();

            foreach (MountPoint2D point in transform.GetComponentsInChildren<MountPoint2D>())
            {
                mountPoints.Add(point);
            }

            Initialize();           
        }

        private void Start()
        {
            transform.position += new Vector3(0, 0, DEPTH);
            DEPTH -= 0.001f;
        }

        public EntityComponent GetEntityComponent(ComponentDefs type)
        {
            return componentsHolder.GetComponent(type);
        }

        public virtual void LookAt(Transform target)
        {
            if (Mathf.Sign(target.position.x - transform.position.x) == -1)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }

            Flip();
        }

        public void Visible(bool value)
        {
            if (value)
            {
                foreach (EntityComponent component in componentsHolder.components)
                {
                    component.sprite.color = new Color(255, 255, 255, 1f);
                } 
            }
            else
            {
                foreach (EntityComponent component in componentsHolder.components)
                {
                    component.sprite.color = new Color(255, 255, 255, 0f);
                }
              
            }
        }

        public void FadeOut(Action OnFinished = null)
        {
            foreach (EntityComponent component in componentsHolder.components)
            {
                StartCoroutine(FadeOut(component.sprite, FADE_TIME, OnFinished));
            }
    
        }

        public void FadeIn(Action OnFinished = null)
        {
            foreach (EntityComponent component in componentsHolder.components)
            {
                StartCoroutine(FadeIn(component.sprite, FADE_TIME, OnFinished));
            }
        }

        public IEnumerator FadeIn(tk2dSprite sprite, float time, Action OnFinished = null)
        {
            float alpha = .0f;

            while (alpha < 1.0f)
            {
                alpha += Time.deltaTime * time;
                sprite.color = new Color(255, 255, 255, alpha);
                yield return null;
            }

            if (OnFinished != null) { OnFinished(); }
        }

        public IEnumerator FadeOut(tk2dSprite sprite, float time, Action OnFinished = null)
        {
            float alpha = sprite.color.a;

            while (alpha > 0f)
            {
                alpha -= Time.deltaTime * time;
                sprite.color = new Color(255, 255, 255, alpha);
                yield return null;
            }

            if (OnFinished != null) { OnFinished(); }
        }

        protected virtual void OnBecameInvisible()
        {
            
        }

        protected virtual void OnBecameVisible()
        {

        }

        protected virtual void OnTriggerEnter2D(Collider2D coll)
        {

        }

        protected virtual void OnCollisionEnter2D(Collision2D coll)
        {

        }

        public abstract void Initialize();

        public abstract void BindData(object data);

        public abstract bool ImmuneEffector(EffectorData effectorData);

        public virtual void Dispose()
        {
            Destroy(this.gameObject);

            if (OnDispose != null)
            {
                OnDispose();

                OnDispose = null;
            }
          
        }

        public void Play(string name, ComponentDefs type)
        {
            EntityComponent component = componentsHolder.GetComponent(type);

            if (component != null)
            {
                component.Play(name);
            }           
        }

        public void Play(string name)
        {
            foreach (EntityComponent component in componentsHolder.components)
            {
                component.Play(name);
            }
        }

        public void Resume()
        {
            foreach (EntityComponent component in componentsHolder.components)
            {
                component.Resume();
            }
        }

        public void Pause()
        {
            foreach (EntityComponent component in componentsHolder.components)
            {
                component.Pause();
            }
        }

        public Vector3 GetMountPointAtRandom(Space space)
        {
            if (space == Space.World)
            {
                return mountPoints[UnityEngine.Random.Range(0, mountPoints.Count)].transform.position;
            }

            return mountPoints[UnityEngine.Random.Range(0, mountPoints.Count)].transform.localPosition;
        }

        public Transform GetMountPoint(ComponentDefs type)
        {
            foreach (MountPoint2D point in mountPoints)
            {
                if (point.type == type)
                {
                   return point.transform;
                }
            }

            return null;
        }

        public Vector2 GetRandomDirection()
        {
            return BaseEntity2D.directions[UnityEngine.Random.Range(0, BaseEntity2D.directions.Length)];
        }

        public virtual void UpdateHUD()
        {

        }

        protected virtual void Update()
        {
            if (motor2D.isGrounded)
            {
                detalMovement.y = 0;
            }

            if (!gravityDisabled)
            {
                detalMovement.y += GRAVITY * Time.deltaTime;
            }
            
            float smoothedMovementFactor = motor2D.isGrounded ? groundDamping : airDamping;

            if (!isRunning)
            {
                detalMovement.x = Mathf.Lerp(detalMovement.x, 0, Time.deltaTime * smoothedMovementFactor);
                detalMovement.y = Mathf.Lerp(detalMovement.y, 0, Time.deltaTime * smoothedMovementFactor);
            }
            else
            {
                detalMovement.x = Mathf.Lerp(detalMovement.x, direction.x * data.attributeBox.GetAttribute(AttributeKeys.SPD), Time.deltaTime * smoothedMovementFactor);
                detalMovement.y = Mathf.Lerp(detalMovement.y, direction.y * data.attributeBox.GetAttribute(AttributeKeys.SPD), Time.deltaTime * smoothedMovementFactor);
            }

            motor2D.move(detalMovement * Time.deltaTime);

            detalMovement = motor2D.velocity;
        }

        public virtual void Flip()
        {
            Vector3 theScale = componentsHolder.transform.localScale;
            theScale.x *= -1;
            componentsHolder.transform.localScale = theScale;
        }

        public virtual void OnWeaponHit(BaseEntity2D sender, WeaponData weapon, Vector2 point)
        {

        }


        public virtual void OnSkillHit(BaseEntity2D sender, SkillData skill, Vector2 point)
        {

        }

        public virtual void OnMissileHit(BaseEntity2D sender, MissileData missile, Vector2 point)
        {

        }
        
    }
}