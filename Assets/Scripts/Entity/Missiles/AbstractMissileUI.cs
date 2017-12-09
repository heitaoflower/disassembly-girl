using UnityEngine;
using Utils;
using System;
using Prototype;

namespace Entity.Missile
{
    public enum MissileAnimationDefs : int
    {
        Run = 0,
        Hit,
        Count
    }

    public abstract class AbstractMissileUI : MonoBehaviour
    {
        protected static readonly float SPEED = -0.5f;        

        protected Vector3 velocity = Vector3.zero;

        protected Rigidbody2D body2D = null;

        protected BoxCollider2D box2D = null;

        protected new Collider2D collider2D = null;

        protected MissileData data = null;

        public Action OnDispose = null;

        public BaseEntity2D owner = null;
        
        public void Config(MissileData missile, BaseEntity2D owner)
        {
            this.data = missile;
            this.owner = owner;
        }

        void Awake()
        {
            body2D = GetComponent<Rigidbody2D>();
            box2D = GetComponent<BoxCollider2D>();
            
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.PET_MISSILE.id, LayerMaskDefines.MAP.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.PET_MISSILE.id, LayerMaskDefines.PET_MISSILE.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.PET_MISSILE.id, LayerMaskDefines.PET.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.PET_MISSILE.id, LayerMaskDefines.WEAPON.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.MONSTER_MISSILE.id, LayerMaskDefines.MAP.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.MONSTER_MISSILE.id, LayerMaskDefines.PET.id);

            Physics2D.IgnoreLayerCollision(LayerMaskDefines.MONSTER_MISSILE.id, LayerMaskDefines.MONSTER.id);
            
            Physics2D.IgnoreLayerCollision(LayerMaskDefines.MONSTER_MISSILE.id, LayerMaskDefines.MONSTER_MISSILE.id);
        }

        public void Resume()
        {
            body2D.velocity = velocity;
            GetComponent<tk2dSpriteAnimator>().Resume();
        }

        public void Pause()
        {
            body2D.velocity = Vector2.zero;
            GetComponent<tk2dSpriteAnimator>().Pause();
        }

        public virtual void Run(Vector3 targetPosition)
        {           
            Vector3 distance = (this.transform.position - targetPosition).normalized;

            float angle = Mathf.Atan2(distance.y, distance.x);
            velocity.x = Mathf.Cos(angle);
            velocity.y = Mathf.Sin(angle);
            velocity = velocity * SPEED;

            box2D.enabled = true;

            body2D.velocity = velocity;
        }

        public abstract void ProcessCollision(Vector2 point);

        void OnBecameInvisible()
        {
            Dispose();
        }

        void OnBecameVisible()
        {

        }

        void OnDestroy()
        {
            EventBox.Send(CustomEvent.DUNGEON_MISSILE_DESTROY, this);
        }

        public void Dispose()
        {
            if (OnDispose != null)
            {
                OnDispose();

                OnDispose = null;
            }

            Destroy(this.gameObject);
        }
    }
}