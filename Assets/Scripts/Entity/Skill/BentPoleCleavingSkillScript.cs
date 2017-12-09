using System.Collections;
using Utils;
using UnityEngine;
using Effect.Component;

namespace Entity.Skill
{
    public class BentPoleCleavingSkillScript : AbstractSkillScript
    {
        private int cleavingAmount = 2;

        private int cleavingTimes = 5;

        protected override void Activate()
        {
            base.Activate();

            Vector2 startPoint = new Vector2(-0.028f, 1.1625f);

            skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
            skillUIElement.IgnoreCollisionMap(true);
            skillUIElement.transform.SetParent(owner.componentsHolder.transform);
            skillUIElement.transform.localPosition = startPoint;
            skillUIElement.transform.localScale = Vector2.one;
            skillUIElement.Visible(false);
            skillUIElement.FadeIn(4.0f);
            
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

        protected override IEnumerator RunProcessor()
        {
            yield return StartCoroutine(TweenMove(skillUIElement.transform, new Vector2(-0.028f, 0.1625f), 0.5f));
            skillUIElement.transform.localPosition = new Vector3(-0.02185f, 0.075f);
            skillUIElement.animator.Play(SkillState.Apply.ToString().ToLower());

            EntityComponent component = owner.componentsHolder.GetComponent(ComponentDefs.Body);
            component.Play(AnimationDefs.Action1.ToString().ToLower());
            component.animator.AnimationEventTriggered += OnAnimationEventTriggered;
            component.animator.AnimationCompleted += OnAnimationCompleted;
        }

        private void OnAnimationCompleted(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
        {
            animator.AnimationCompleted -= OnAnimationCompleted;

            EventBox.Send(CustomEvent.DUNGEON_SKILL_RESUME);
        }

        private void OnAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIndex)
        {
            animator.AnimationEventTriggered -= OnAnimationEventTriggered;

            tk2dSpriteAnimationFrame frame = clip.GetFrame(frameIndex);

            if (frame.eventInfo == AnimationTriggerDefs.SkillApply.ToString())
            {
                skillUIElement.Apply(owner, data, targetPosition);
                skillUIElement.OnHit = () => 
                {
                    Cleaving(skillUIElement, 1);

                    skillUIElement = null;

                    Destroy(this);
                };
            }
        }

        private void Cleaving(SkillUI skillUIElement, int times)
        {
            if (times >= cleavingTimes)
            {
                return;
            }

            for (int index = 0; index < cleavingAmount; index++)
            {
                SkillUI ghostSkillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
                ghostSkillUIElement.IgnoreCollisionMap(true);
   
                ghostSkillUIElement.transform.position = skillUIElement.transform.position;
                ghostSkillUIElement.positionLastFrame = skillUIElement.transform.position;
                ghostSkillUIElement.transform.localScale = Vector2.one;     
                ghostSkillUIElement.data = skillUIElement.data;
                ghostSkillUIElement.owner = skillUIElement.owner;
                ghostSkillUIElement.gameObject.AddComponent<ComponentDefaultEffect>().spinEnabled = false;
                ghostSkillUIElement.animator.Play(AnimationDefs.Run.ToString().ToLower());

                ghostSkillUIElement.OnUpdate = () => 
                {
                    if (ghostSkillUIElement.positionLastFrame.y > ghostSkillUIElement.transform.position.y)
                    {
                        ghostSkillUIElement.body2D.isKinematic = false;
                        ghostSkillUIElement.collider2D.enabled = true;
                        ghostSkillUIElement.OnHit = () => { Cleaving(ghostSkillUIElement, times + 1); };  
                    }
                };
            }
        }

    }
}