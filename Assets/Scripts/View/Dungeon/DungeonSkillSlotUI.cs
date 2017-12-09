using Prototype;
using UnityEngine;
using System.Collections;
using Utils;

namespace View.Dungeon
{
    public class DungeonSkillSlotUI : BaseView
    {
        public UISprite iconImg = null;

        public UISprite activeImg = null;

        public UISprite maskImg = null;

        private SkillData skillData = null;

        public override void Show(object data = null)
        {
            skillData = data as SkillData;

            iconImg.spriteName = skillData.iconID;
            iconImg.gameObject.SetActive(true);
            activeImg.gameObject.SetActive(true);

            EventBox.Add(CustomEvent.USER_APPLY_SKILL, OnUserApplySkill);
        }

        public override void Hide()
        {
            iconImg.gameObject.SetActive(false);
            activeImg.gameObject.SetActive(false);
        }

        public override void Dispose()
        {
            base.Dispose();

            EventBox.RemoveAll(this);

            if (skillData != null)
            {
                skillData.state = PrototypeState.Normal;
            }
        }

        private void OnUserApplySkill(object data)
        {
            if (skillData == data)
            {
                skillData.state = PrototypeState.Frozen;

                StartCoroutine(CDTimerProcessor());
            }
        }

        private IEnumerator CDTimerProcessor()
        {
            maskImg.gameObject.SetActive(true);

            float time = skillData.CD;

            while (time >= 0)
            {
                time -= Time.deltaTime;

                maskImg.fillAmount = time / skillData.CD;

                yield return null;
            }

            maskImg.gameObject.SetActive(false);

            skillData.state = PrototypeState.Normal;
        }
    }
}