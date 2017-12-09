using Utils;
using UnityEngine;
using System.Collections;

namespace Entity.Skill
{
    public class GiantWrenchSkillScript : AbstractSkillScript
    {
        protected override void Activate()
        {
            base.Activate();

            Vector2 startPoint = new Vector2(-0.025f, 1.19f);

            skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
            skillUIElement.IgnoreCollisionMap(true);
            skillUIElement.transform.SetParent(owner.componentsHolder.transform);
            skillUIElement.transform.localPosition = startPoint;
            skillUIElement.transform.localScale = Vector2.one;
            skillUIElement.Visible(false);
            skillUIElement.FadeIn(4.0f);
            
        }

        protected override IEnumerator RunProcessor()
        {
            yield return StartCoroutine(TweenMove(skillUIElement.transform, new Vector2(-0.025f, 0.19f), 0.5f));
            skillUIElement.transform.localPosition = new Vector3(-0.0345f, 0.1f);
            skillUIElement.animator.Play(SkillState.Apply.ToString().ToLower());

            EntityComponent component = owner.componentsHolder.GetComponent(ComponentDefs.Body);
            component.Play(AnimationDefs.Action1.ToString().ToLower());
            component.animator.AnimationEventTriggered += OnAnimationEventTriggered;
            component.animator.AnimationCompleted += OnAnimationCompleted;
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

                skillUIElement = null;

                Destroy(this);
            }
        }
    }
}