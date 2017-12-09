using UnityEngine;
using Entity.AI;
using Utils;
using Prototype;
using System.Collections.Generic;
using Effect.Component;
using Manager;

namespace Entity.Missile
{
    public class PetMissileUI : AbstractMissileUI
    {
        protected void OnCollisionEnter2D(Collision2D coll)
        {
            #region Hit Monster
            if (coll.gameObject.layer == LayerMaskDefines.MONSTER.id)
            {
                MonsterEntity2D entity = coll.gameObject.GetComponent<MonsterEntity2D>();

                if (entity.machine.currentState.GetName() != StateTypes.Dead.ToString())
                {
                    if (data.WOE == 0)
                    {
                        entity.OnMissileHit(owner, data, coll.contacts[0].point);
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
                ProcessPhsysicsType(coll.gameObject, data.physicsType);
            }
            #endregion
        }

        public override void Run(Vector3 targetPosition)
        {
            base.Run(targetPosition);

            body2D.isKinematic = false;
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

        override public void ProcessCollision(Vector2 point)
        {
            GameObject prefab = Resources.Load(GlobalDefinitions.RESOURCE_PATH_EFFECT_HIT + data.hitEffectID) as GameObject;

            GameObject hit = Instantiate(prefab, point, Quaternion.identity) as GameObject;
            LayerManager.GetInstance().AddObject(hit.transform);

            MissileHit missileHit = hit.AddComponent<MissileHit>();
            missileHit.missile = data;
            missileHit.sender = owner;
        }
    }

}
