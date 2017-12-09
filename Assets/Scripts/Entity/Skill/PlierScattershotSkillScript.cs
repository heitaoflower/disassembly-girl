using UnityEngine;
using System.Collections;
using Utils;

namespace Entity.Skill
{
    public class PlierScattershotSkillScript : AbstractSkillScript
    {
        private int shotAmount = 5;

        private int shotDegree = 5;

        protected override void Activate()
        {
            base.Activate();

            Vector2 startPoint = new Vector2(-0.0405f, 1.175f);

            skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
            skillUIElement.IgnoreCollisionMap(true);
            skillUIElement.transform.SetParent(owner.componentsHolder.transform);
            skillUIElement.transform.localPosition = startPoint;
            skillUIElement.transform.localScale = Vector2.one;
            skillUIElement.Visible(false);
            skillUIElement.FadeIn(4.0f);
            
        }

        private void OnAnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIndex)
        {
            animator.AnimationEventTriggered -= OnAnimationEventTriggered;

            tk2dSpriteAnimationFrame frame = clip.GetFrame(frameIndex);

            if (frame.eventInfo == AnimationTriggerDefs.SkillApply.ToString())
            {
                Vector3 startPoint = this.skillUIElement.transform.position;

                float degree = Mathf.Atan2(targetPosition.y - startPoint.y, targetPosition.x - startPoint.x) * Mathf.Rad2Deg;

                float angle = degree * Mathf.Deg2Rad;
               
                this.skillUIElement.Apply(owner, data, angle);
                
                for (int index = 0; index < shotAmount; index++)
                {
                    SkillUI skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
                    skillUIElement.transform.SetParent(owner.componentsHolder.transform);
                    skillUIElement.IgnoreCollisionMap(true);
                    skillUIElement.transform.position = startPoint;
                    skillUIElement.transform.localScale = Vector2.one;
                    angle = (degree - shotDegree * (index + 1)) * Mathf.Deg2Rad;
                    skillUIElement.Apply(owner, data, angle);
                }
    
                for (int index = 0; index < shotAmount; index++)
                {
                    skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
                    skillUIElement.transform.SetParent(owner.componentsHolder.transform);
                    skillUIElement.IgnoreCollisionMap(true);
                    skillUIElement.transform.position = startPoint;
                    skillUIElement.transform.localScale = Vector2.one;
                    angle = (degree + shotDegree * (index + 1)) * Mathf.Deg2Rad;
                    skillUIElement.Apply(owner, data, angle);
                }

                Destroy(this);

                this.skillUIElement = null;
            }
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
            yield return StartCoroutine(TweenMove(skillUIElement.transform, new Vector2(-0.0405f, 0.175f), 0.5f));
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
    }
}