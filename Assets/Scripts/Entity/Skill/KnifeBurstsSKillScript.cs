using System.Collections;
using Utils;
using UnityEngine;
using Manager;

namespace Entity.Skill
{
    public class KnifeBurstsSKillScript : AbstractSkillScript
    {
        private Vector2[] startPoints = new Vector2[]
            {
                new Vector2(-1.5f, 0.9f),
                new Vector2(-1.5f, 0.8f),
                new Vector2(-1.5f, 0.7f),
                new Vector2(-1.5f, 0.6f),
                new Vector2(-1.5f, 0.5f),
                new Vector2(-1.5f, 0.4f),
                new Vector2(-1.5f, 0.3f),
                new Vector2(-1.5f, 0.2f),
                new Vector2(-1.5f, 0.1f),
                new Vector2(-1.5f, 0.0f),
                new Vector2(-1.5f, -0.1f),
                new Vector2(-1.5f, -0.2f),
                new Vector2(-1.5f, -0.3f),
                new Vector2(-1.5f, -0.4f),
                new Vector2(-1.5f, -0.5f),
                new Vector2(-1.5f, -0.6f),
            };

        private float interval = 0.2f;

        protected override void Activate()
        {
            base.Activate();
            
            StartCoroutine(Restore());
        }

        protected override void Deactivate()
        {
            base.Deactivate();

            Destroy(this);
        }

        protected override IEnumerator RunProcessor()
        {
            yield return base.RunProcessor();

            for (int index = 0; index < startPoints.Length; index++)
            {
                SkillUI skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
                skillUIElement.transform.position = startPoints[index];
                skillUIElement.transform.localScale = Vector3.one;
                skillUIElement.IgnoreCollisionMap(true);
                skillUIElement.Visible(false);
                skillUIElement.FadeIn(3.0f);
                skillUIElement.Apply(owner, data, 0);

                yield return new WaitForSeconds(interval);
            }
        
            Cancel();

        }

        private IEnumerator Restore()
        {
            yield return new WaitForSeconds(1.0f);

            GirlEntity2D girl = owner as GirlEntity2D;

            girl.componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Idle.ToString().ToLower());

            girl.weaponLauncher.Load(UserManager.GetInstance().user.GetActiveWeapon());

            EventBox.Send(CustomEvent.DUNGEON_SKILL_RESUME);
        }
    }
}