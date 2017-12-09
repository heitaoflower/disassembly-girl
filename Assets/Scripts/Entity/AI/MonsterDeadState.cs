using Orange.StateKit;
using UnityEngine;
using Utils;
using Manager;
using System.Collections;
using Effect.Component;
using Prototype;
using Entity.Effector;

namespace Entity.AI
{
    public class MonsterDeadState : State<MonsterEntity2D>
    {
        private Task explosionTask = null;

        private ValidatePayload payload = null;

        public override void Begin(object data = null)
        {
            base.Begin();

            payload = data as ValidatePayload;

            GetContext().hudContainer.DestroyChildren();

            MonsterData monster = GetContext().data as MonsterData;

            foreach (AbstractApplierEffector effector in GetContext().GetComponents<AbstractApplierEffector>())
            {
                effector.Cancel();
            }

            if (int.Parse(monster.explosionID) > 0)
            { 
                explosionTask = new Task(ExplosionProcessor());
            }
            else
            {
                DeadProcessor();
            }              
        }

        public override void End()
        {
            base.End();

            if (explosionTask != null)
            {
                explosionTask.Stop();
                explosionTask = null;
            }
        }


        private void DeadProcessor()
        {
            GetContext().Play(AnimationDefs.Dead.ToString().ToLower());

            GetContext().motor2D.platformMask = 0;
            GetContext().motor2D.triggerMask = 0;

            Collider2D[] colliders = GetContext().GetComponentsInChildren<Collider2D>();

            for (int index = 0; index < colliders.Length; index++)
            {
                Physics2D.IgnoreCollision(colliders[index], DungeonManager.GetInstance().girlEntity.motor2D.boxCollider);
            }
            
            EffectUtils.Drop(GetContext().gameObject, payload.hitPoint);
        }

        private IEnumerator ExplosionProcessor()
        {
            bool isCompleted = false;

            GetContext().Play(AnimationDefs.Explosion.ToString().ToLower());

            MonsterData monster = GetContext().data as MonsterData;         
            tk2dSpriteAnimator explosionAnimator = ResourceUtils.GetComponent<tk2dSpriteAnimator>(GlobalDefinitions.RESOURCE_PATH_EFFECT_EXPLOSION + monster.explosionID);
            explosionAnimator.transform.SetParent(GetContext().transform);
            explosionAnimator.transform.position = GetContext().GetMountPoint(ComponentDefs.Body).position;

            explosionAnimator.AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
            {
                animator.AnimationCompleted = null;
                GameObject.Destroy(animator.gameObject);

                DeadProcessor();

                isCompleted = true;
            };

            float time = default(float);
           
            while (!isCompleted)
            {
                if (time > 0.5)
                {
                    AbstractComponentEffect effect = AbstractComponentEffect.LoadRandomEffect();
                    effect.transform.position = GetContext().GetMountPointAtRandom(Space.World);
                    effect.Initaizlie();

                    LayerManager.GetInstance().AddObject(effect.transform);

                    time = default(float);
                }

                time += Time.deltaTime;         

                yield return null;
            }

            for (int index = 0, amount = Random.Range(3, 5); index < amount; index++)
            {
                AbstractComponentEffect effect = AbstractComponentEffect.LoadRandomEffect();
                effect.transform.position = GetContext().GetMountPointAtRandom(Space.World);
                effect.Initaizlie();

                LayerManager.GetInstance().AddObject(effect.transform);
            }
        }

        public override string GetName()
        {
            return StateTypes.Dead.ToString();
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
