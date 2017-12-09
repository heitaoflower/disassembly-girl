using UnityEngine;
using Prototype;
using Manager;
using Utils;
using System.Collections.Generic;

namespace Entity
{
    public class WeaponHit : MonoBehaviour
    {
        private tk2dSpriteAnimator animator = null;

        public WeaponData weapon = null;

        public BaseEntity2D sender = null;

        void Awake()
        {
            animator = GetComponent<tk2dSpriteAnimator>();
        }

        void Start()
        {
            SoundManager.GetInstance().PlayOneShot(AudioRepository.LoadExplosionAudio(weapon.audioID));

            animator.AnimationCompleted = delegate (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
            {
                animator.AnimationCompleted = null;

                weapon = null;

                Destroy(gameObject);
            };

            if (weapon.WOE != 0)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, this.weapon.WOE);

                IList<GameObject> hitedCache = new List<GameObject>();
                foreach (Collider2D hit in hits)
                {
                    MonsterEntity2D entity = hit.gameObject.GetComponent<MonsterEntity2D>();

                    if (entity != null && !hitedCache.Contains(entity.gameObject))
                    {
                        entity.OnWeaponHit(sender, weapon, this.transform.position);

                        hitedCache.Add(hit.gameObject);
                    }
                }

                hitedCache.Clear();
            }
              
        }

        void Update()
        {
            if (weapon != null && weapon.WOE != 0)
            {
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x + this.weapon.WOE, this.transform.position.y), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x - this.weapon.WOE, this.transform.position.y), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y - this.weapon.WOE), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y + this.weapon.WOE), Color.blue);
            }
        }
    }
}