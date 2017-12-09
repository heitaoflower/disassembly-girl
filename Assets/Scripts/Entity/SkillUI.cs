using UnityEngine;
using Prototype;
using Utils;
using Manager;
using Effect.Component;
using System.Collections.Generic;
using System;
using Entity.AI;

public enum SkillState
{
    Idle, Apply, Run, Count
}

namespace Entity
{
    public class SkillUI : MonoBehaviour
    {
        public tk2dSpriteAnimator animator = null;

        public Rigidbody2D body2D = null;

        public new Collider2D collider2D = null;

        public BaseEntity2D owner = null;

        public SkillData data = null;

        public Vector3 positionLastFrame = Vector3.zero;

        private Vector3 velocity = Vector3.zero;

        private Task fadeInTask = null;

        private Task fadeOutTask = null;

        public Action OnRun = null;

        public Action OnDestroyed = null;

        public Action OnHit = null;

        public Action OnUpdate = null;

        void Awake()
        {
            animator = GetComponent<tk2dSpriteAnimator>();

            body2D = GetComponent<Rigidbody2D>();
            body2D.isKinematic = true;

            collider2D = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.SKILL.id, LayerMaskDefines.SKILL.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.SKILL.id, LayerMaskDefines.WEAPON.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.SKILL.id, LayerMaskDefines.GIRL.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.SKILL.id, LayerMaskDefines.PET_MISSILE.id);

        }

        public void Play()
        {
            ProcessCollision(transform.position);

            if (data.vibrateType == SkillVibrateType.Hit)
            {
                CameraUtils.Shake();
            }

            Destroy(gameObject);
        }

        public void Apply(BaseEntity2D owner, SkillData skill, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> OnAnimationCompleted, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> OnAnimationEventTriggered)
        {
            tk2dSpriteAnimationClip clip = animator.GetClipByName(SkillState.Apply.ToString().ToLower());

            if (clip != null)
            {
                animator.Play(clip);

                animator.AnimationCompleted = OnAnimationCompleted;
                animator.AnimationEventTriggered = OnAnimationEventTriggered;
            }            
        }

        public void Apply(BaseEntity2D owner, SkillData skill, Vector3 targetPosition)
        {
            Vector3 distance = (targetPosition - this.transform.position).normalized;

            float angle = Mathf.Atan2(distance.y, distance.x);

            Apply(owner, skill, angle);
        }

        public void Apply(BaseEntity2D owner, SkillData skill, float angle)
        {
            this.owner = owner;

            this.data = skill;

            Run(angle);
        }

        private void Run(float angle)
        {
            velocity.x = Mathf.Cos(angle);
            velocity.y = Mathf.Sin(angle);

            float factor = data.attributeBox.GetAttribute(AttributeKeys.SPD) + owner.data.attributeBox.GetAttribute(AttributeKeys.SPD) * 0.01f;

            velocity = velocity * factor;
            body2D.velocity = velocity;
            body2D.isKinematic = false;
            collider2D.enabled = true;
    
            animator.Play(SkillState.Run.ToString().ToLower());

            LayerManager.GetInstance().AddObject(this.transform);

            if (OnRun != null)
            {
                OnRun();
            }
        }

        void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }

            positionLastFrame = transform.position;
        }

        private void OnBecameInvisible()
        {
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            if (fadeInTask != null)
            {
                fadeInTask.Stop();
                fadeInTask = null;
            }

            if (fadeOutTask != null)
            {
                fadeOutTask.Stop();
                fadeOutTask = null;
            }

            if (OnDestroyed != null)
            {
                OnDestroyed();
            }
        }

        private void OnBecameVisible()
        {

        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            #region Hit Monster
            if (coll.gameObject.layer == LayerMaskDefines.MONSTER.id)
            {
                MonsterEntity2D entity = coll.gameObject.GetComponent<MonsterEntity2D>();

                if (entity.machine.currentState.GetName() != StateTypes.Dead.ToString())
                {
                    if (OnHit != null)
                    {
                        OnHit();
                    }

                    if (data.WOE == 0)
                    {
                        entity.OnSkillHit(owner, data, coll.contacts[0].point);
                    }

                    ProcessCollision(coll);

                    ProcessPhsysicsType(coll.gameObject, data.physicsType);

                    if (data.vibrateType == SkillVibrateType.Hit)
                    {
                        CameraUtils.Shake();
                    }
                }
                else
                {
                    body2D.velocity = velocity;

                    Collider2D[] colliders = coll.gameObject.GetComponents<Collider2D>();

                    for (int index = 0; index < colliders.Length; index++)
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), colliders[index]);
                    }                  
                }
            }
            #endregion

            #region Hit Missile
            if (coll.gameObject.layer == LayerMaskDefines.MONSTER_MISSILE.id)
            {
                ProcessCollision(coll);

                ProcessPhsysicsType(coll.gameObject, data.physicsType);

                if (data.vibrateType == SkillVibrateType.Hit)
                {
                    CameraUtils.Shake();
                }

            }
            #endregion
        }

        private void ProcessPhsysicsType(GameObject target, PhysicsType type)
        {
            if (type == PhysicsType.BounceAll)
            {
                IList<ComponentDefaultEffect> effects = gameObject.GetComponents<ComponentDefaultEffect>();

                for (int index = 0; index < effects.Count; index++)
                {
                    Destroy(effects[index]);
                }
                     
                Collider2D[] colliders = target.GetComponents<Collider2D>();

                for (int index = 0; index < colliders.Length; index++)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), colliders[index]);
                }

                gameObject.AddComponent<ComponentDefaultEffect>().spinEnabled = false;
            }
            else if (type == PhysicsType.BounceOne)
            {
                IList<ComponentDefaultEffect> effects = gameObject.GetComponents<ComponentDefaultEffect>();

                for (int index = 0; index < effects.Count; index++)
                {
                    Destroy(effects[index]);
                }

                collider2D.enabled = false;

                gameObject.AddComponent<ComponentDefaultEffect>().spinEnabled = false;
            }
            else if (type == PhysicsType.PenetrationAll)
            {
                Collider2D[] colliders = target.GetComponents<Collider2D>();

                for (int index = 0; index < colliders.Length; index++)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), colliders[index]);
                }

                body2D.velocity = velocity;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void ProcessCollision(Vector2 point)
        {
            GameObject prefab = Resources.Load(GlobalDefinitions.RESOURCE_PATH_EFFECT_HIT + data.hitEffectID) as GameObject;

            GameObject hit = Instantiate(prefab, point, Quaternion.identity) as GameObject;
            LayerManager.GetInstance().AddObject(hit.transform);

            SkillHit skillHit = hit.AddComponent<SkillHit>();
            skillHit.skill = data;
            skillHit.sender = owner;
        }

        private void ProcessCollision(Collision2D coll)
        {
            ProcessCollision(coll.contacts[0].point);
        }

        public SkillUI Visible(bool value)
        {
            EffectUtils.Visible(GetComponent<tk2dSprite>(), value);

            return this;
        }

        public SkillUI FadeIn(float speed)
        {
            fadeInTask = EffectUtils.FadeIn(GetComponent<tk2dSprite>(), speed);

            return this;
        }

        public SkillUI FadeOut(float speed)
        {
            fadeOutTask = EffectUtils.FadeOut(GetComponent<tk2dSprite>(), speed, ()=> { Destroy(gameObject); });

            return this;
        }

        public SkillUI IgnoreCollisionMap(bool value)
        {
            Collider2D[] colliders = DungeonManager.GetInstance().mapWrapper.map.renderData.GetComponentsInChildren<Collider2D>();

            for (int index = 0; index < colliders.Length; index++)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), colliders[index], value);
            }

            return this;
        }
    }
}