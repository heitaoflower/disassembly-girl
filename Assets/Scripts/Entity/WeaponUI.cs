using UnityEngine;
using Manager;
using Utils;
using Prototype;
using Effect.Component;
using System.Collections.Generic;
using Entity.AI;
using Entity.Missile;

public enum WeaponState
{
    Idle, Apply, Run, Count
}

namespace Entity
{
    public class WeaponUI : MonoBehaviour
    {
        private tk2dSpriteAnimator animator = null;

        private Rigidbody2D body2D = null;

        private new Collider2D collider2D = null;

        private Vector3 velocity = Vector3.zero;

        private BaseEntity2D owner = null;

        private WeaponData data = null;

        public void Initialize(WeaponData weapon)
        {
            this.data = weapon;
        }

        void Awake()
        {
            animator = GetComponent<tk2dSpriteAnimator>();

            body2D = GetComponent<Rigidbody2D>();
            body2D.isKinematic = true;

            collider2D = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.WEAPON.id, LayerMaskDefines.MAP.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.WEAPON.id, LayerMaskDefines.WEAPON.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.WEAPON.id, LayerMaskDefines.GIRL.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.WEAPON.id, LayerMaskDefines.SKILL.id);
        }

        public void Config(BaseEntity2D owner, float fps)
        {
            this.owner = owner;

            tk2dSpriteAnimationClip clip = animator.GetClipByName(WeaponState.Apply.ToString().ToLower());

            if (clip != null)
            {
                clip.fps = fps;

                animator.AnimationCompleted = delegate (tk2dSpriteAnimator _animator, tk2dSpriteAnimationClip _clip)
                {
                    animator.AnimationCompleted = null;

                    animator.Play(WeaponState.Run.ToString().ToLower());
                };

                animator.Play(clip);
            }
        }

        public void Run(Vector3 targetPosition)
        {
            Vector3 distance = (targetPosition - this.transform.position).normalized;

            float angle = Mathf.Atan2(distance.y, distance.x);
            velocity.x = Mathf.Cos(angle);
            velocity.y = Mathf.Sin(angle);

            float factor = data.attributeBox.GetAttribute(AttributeKeys.SPD) + owner.data.attributeBox.GetAttribute(AttributeKeys.SPD) * 0.02f;

            velocity = velocity * factor;
            body2D.velocity = velocity;
            body2D.isKinematic = false;            
            collider2D.enabled = true;
        }

        void OnBecameInvisible()
        {
            Dispose();
        }

        void OnBecameVisible()
        {

        }

        public void Dispose()
        {
            Destroy(this.gameObject);
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            #region Hit Monster
            if (coll.gameObject.layer == LayerMaskDefines.MONSTER.id)
            {                
                MonsterEntity2D entity = coll.gameObject.GetComponent<MonsterEntity2D>();

                if (entity.machine.currentState.GetName() != StateTypes.Dead.ToString())
                {          
                    if (data.WOE == 0)
                    {
                        entity.OnWeaponHit(owner, data, coll.contacts[0].point);                       
                    }

                    ProcessCollision(coll.contacts[0].point);

                    ProcessPhsysicsType(coll.gameObject, data.physicsType);
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
                EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.DungeonKillMissile, 1));

                ProcessCollision(coll.contacts[0].point);

                ProcessPhsysicsType(coll.gameObject, data.physicsType);
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

            WeaponHit weaponHit = hit.AddComponent<WeaponHit>();
            weaponHit.weapon = data;
            weaponHit.sender = owner;            
        }

    }
}