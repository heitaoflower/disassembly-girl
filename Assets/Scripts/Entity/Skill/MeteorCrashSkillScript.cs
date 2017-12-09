using Utils;
using UnityEngine;
using Manager;
using System.Collections;
using View.Guide;
namespace Entity.Skill
{
    public class MeteorCrashSkillScript : AbstractSkillScript
    {
        private Vector2 startPoint = new Vector2(-1.2f, 0.67f);

        private int meteorAmount = 1;

        private float radius = 0.5f;

        private int waveAmount = 15;
    
        protected override void Activate()
        {
            base.Activate();

            LayerManager.GetInstance().AddPopUpView<GuideWindow>();

        }

        protected override IEnumerator RunProcessor()
        {
            for (int index = 0; index < waveAmount; index++)
            {
                Fire();

                yield return new WaitForSeconds(0.2f);

                if (index == 3)
                {
                    GirlEntity2D girl = owner as GirlEntity2D;

                    girl.componentsHolder.GetComponent(ComponentDefs.Body).Play(AnimationDefs.Idle.ToString().ToLower());

                    girl.weaponLauncher.Load(UserManager.GetInstance().user.GetActiveWeapon());

                    LayerManager.GetInstance().RemovePopUpView<GuideWindow>();
                   
                    EventBox.Send(CustomEvent.DUNGEON_SKILL_RESUME);
                }
            }

            Cancel();

        }

        protected override void Deactivate()
        {
            base.Deactivate();

            Destroy(this);
        }

        private void Fire()
        {
            for (int index = 0; index < meteorAmount; index++)
            {
                Vector2 position = (Random.insideUnitCircle * radius) + startPoint;

                SkillUI skillUIElement = ResourceUtils.AddAndGetComponent<SkillUI>(GlobalDefinitions.RESOURCE_PATH_SKILL + data.resourceID);
                skillUIElement.transform.position = position;
                skillUIElement.transform.localScale = Vector3.one;
                skillUIElement.IgnoreCollisionMap(true);
                skillUIElement.Visible(false);
                skillUIElement.FadeIn(3.0f);
                skillUIElement.Apply(owner, data, -Mathf.PI / 6.0f);
            }
        }
    }
}