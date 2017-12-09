using UnityEngine;
using Utils;
using Prototype;

namespace Entity.Missile
{
    public class MonsterMissileUI : AbstractMissileUI
    {
        protected void OnTriggerEnter2D(Collider2D coll)
        {
            ProcessCollision(coll.transform.position);

            if (coll.gameObject.layer == LayerMaskDefines.GIRL.id)
            {
                MonsterData data = owner.data as MonsterData;
                BaseEntity2D entity = coll.gameObject.GetComponent<BaseEntity2D>();
                entity.OnMissileHit(owner, data.missileData, coll.transform.position);
            }
        }

        public override void Run(Vector3 targetPosition)
        {
            base.Run(targetPosition);

            body2D.isKinematic = true;

        }
        public override void ProcessCollision(Vector2 point)
        {
            box2D.enabled = false;
            body2D.velocity = Vector3.zero;      
            body2D.simulated = false;
            GetComponent<tk2dSpriteAnimator>().AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
            {
                animator.AnimationCompleted = null;
                Dispose();
            };

            GetComponent<tk2dSpriteAnimator>().Play(MissileAnimationDefs.Hit.ToString().ToLower());
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            ProcessCollision(collision.contacts[0].point);
        }


    }
}

