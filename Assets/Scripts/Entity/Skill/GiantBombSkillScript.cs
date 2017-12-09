using System.Collections;
using Utils;
using UnityEngine;
using Manager;
using View.Guide;

namespace Entity.Skill
{
    public class GiantBombSkillScript : AbstractSkillScript
    {
        protected override void Activate()
        {
            base.Activate();
            
            Vector2 startPoint = new Vector2(-0.0825f, 1.200f);

            skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
            skillUIElement.transform.SetParent(owner.componentsHolder.transform);
            skillUIElement.transform.localPosition = startPoint;
            skillUIElement.Visible(false);
            skillUIElement.FadeIn(4.0f);
            skillUIElement.IgnoreCollisionMap(false);        
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
            yield return StartCoroutine(TweenMove(skillUIElement.transform, new Vector2(-0.0825f, 0.200f), 0.5f));
            skillUIElement.transform.localPosition = new Vector3(-0.0825f, 0.1005f);
            skillUIElement.animator.Play(SkillState.Apply.ToString().ToLower());

            CameraUtils.Shake();
           
            EntityComponent component = owner.componentsHolder.GetComponent(ComponentDefs.Body);
            component.Play(AnimationDefs.Action3.ToString().ToLower());
            component.animator.AnimationEventTriggered += OnAnimationEventTriggered;
            component.animator.AnimationCompleted += OnAnimationCompleted;
            yield return new WaitForSeconds(2f);

            skillUIElement.Play();
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
                if (owner.componentsHolder.transform.localScale.x > 0f)
                {
                    skillUIElement.Apply(owner, data, Mathf.PI);
                }
                else
                {
                    skillUIElement.Apply(owner, data, 0);
                }

                GhostEffectScript ghost = skillUIElement.GetComponent<GhostEffectScript>();
                ghost.StartEffect();

                skillUIElement.OnDestroyed = () =>
                {
                    skillUIElement = null;

                    Destroy(this);
                };                
            }
        }

    }
}