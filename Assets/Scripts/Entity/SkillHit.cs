using UnityEngine;
using Prototype;
using System.Collections.Generic;

namespace Entity
{
    public class SkillHit : MonoBehaviour
    {
        private tk2dSpriteAnimator animator = null;

        public SkillData skill = null;

        public BaseEntity2D sender = null;

        void Awake()
        {
            animator = GetComponent<tk2dSpriteAnimator>();
        }

        void Start()
        {
            animator.AnimationCompleted = delegate (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
            {
                animator.AnimationCompleted = null;

                skill = null;

                Destroy(gameObject);
            };

            if (skill.WOE != 0)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, this.skill.WOE);

                IList<GameObject> hitedCache = new List<GameObject>();
                foreach (Collider2D hit in hits)
                {
                    MonsterEntity2D entity = hit.gameObject.GetComponent<MonsterEntity2D>();

                    if (entity != null && !hitedCache.Contains(entity.gameObject))
                    {
                        entity.OnSkillHit(sender, skill, this.transform.position);

                        hitedCache.Add(hit.gameObject);
                    }
                }

                hitedCache.Clear();
            }

        }

        void Update()
        {
            if (skill != null && skill.WOE != 0)
            {
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x + this.skill.WOE, this.transform.position.y), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x - this.skill.WOE, this.transform.position.y), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y - this.skill.WOE), Color.blue);
                Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y + this.skill.WOE), Color.blue);
            }
        }
    }
}
