using Utils;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Entity.AI;
using Effect.Component;

namespace Entity.Skill
{
    public class HammerShockSkillScript : AbstractSkillScript
    {
        protected override void Activate()
        {
            base.Activate();

            Vector2 startPoint = new Vector2(-0.028f, 1.197f);
            skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
            skillUIElement.IgnoreCollisionMap(true);
            skillUIElement.transform.SetParent(owner.componentsHolder.transform);
            skillUIElement.transform.localPosition = startPoint;
            skillUIElement.transform.localScale = Vector2.one;
            skillUIElement.Visible(false);
            skillUIElement.FadeIn(4.0f);
            skillUIElement.animator.Play(SkillState.Idle.ToString().ToLower());
            
        }

        protected override IEnumerator RunProcessor()
        {
            yield return StartCoroutine(TweenMove(skillUIElement.transform, new Vector2(-0.028f, 0.197f), 0.5f));
            skillUIElement.transform.localPosition = new Vector3(-0.1155f, 0.101f);

            EntityComponent component = owner.componentsHolder.GetComponent(ComponentDefs.Body);
            component.Play(AnimationDefs.Action2.ToString().ToLower());
            component.animator.AnimationCompleted += OnAnimationCompleted;

            skillUIElement.Apply(owner, data, (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>{ Cancel(); }, OnAnimationEventTriggered);
        }

        private void OnAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIndex)
        {
            CameraUtils.Shake();

            IList<MonsterEntity2D> monsters = DungeonManager.GetInstance().monsters;
          
            for (int index = monsters.Count - 1; index >= 0; index--)
            {
                MonsterEntity2D mosnter = monsters[index];

                if (mosnter.motor2D.isGrounded)
                {
                    mosnter.OnSkillHit(owner, data, owner.transform.position);

                    if (mosnter.machine.currentState.GetName() == StateTypes.Dead.ToString())
                    {
                        ComponentDefaultEffect componentEffect = mosnter.gameObject.AddComponent<ComponentDefaultEffect>();
                        componentEffect.spinEnabled = false;
                        componentEffect.minForce = 0.5f;
                        componentEffect.maxForce = 1.0f;
                        componentEffect.gravity = -6.0f;
                        componentEffect.minSpeed = 0.4f;
                        componentEffect.maxSpeed = 0.6f;
                    }

                }
            }
        }

        private void OnAnimationCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
        {
            animator.AnimationCompleted -= OnAnimationCompleted;

            EventBox.Send(CustomEvent.DUNGEON_SKILL_RESUME);
        }

        protected override void Deactivate()
        {
            base.Deactivate();

            Destroy(this);

            if (skillUIElement != null)
            {
                skillUIElement.animator.AnimationEventTriggered = null;
                skillUIElement.FadeOut(4.0f);
            }
        }
    }
}