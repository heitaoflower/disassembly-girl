using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Utils;
using Prototype;

namespace Entity.Skill
{
    public class AbstractSkillScript : MonoBehaviour
    {
        public SkillData data = null;

        public SkillUI skillUIElement = null;

        public IList<SkillUI> skillUIElements = null;

        public BaseEntity2D owner = null;

        public Vector3 targetPosition = Vector3.zero;

        #region Base Method

        void OnDestroy()
        {
            if (skillUIElements != null)
            {
                skillUIElements.Clear();
                skillUIElements = null;
            }

            if (skillUIElement != null)
            {
                skillUIElement = null;
            }            
        }

        public void Run()
        {
            Activate();

            StartCoroutine(RunProcessor());
        }

        public void Cancel()
        {
            Deactivate();

            StopAllCoroutines();
        }

        protected virtual void Activate()
        {
            EventBox.Send(CustomEvent.DUNGEON_SKILL_PAUSE);
        }

        protected virtual void Deactivate()
        {
           
        }

        protected virtual IEnumerator RunProcessor()
        {
            yield return null;
        }

        #endregion

        protected IEnumerator TweenMove(Transform transform, Vector3 targetPosition, float time, Action OnTweenComplete = null)
        {
            Vector3 startPosition = transform.localPosition;
            for (float ut = 0; ut < time; ut += Time.deltaTime)
            {
                float nt = Mathf.SmoothStep(0, 1, Mathf.Clamp01(ut / time));
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, nt);
                yield return 0;
            }

            transform.localPosition = targetPosition;

            if (OnTweenComplete != null)
            {
                OnTweenComplete();
            }
        }

        private static IDictionary<int, Type> skillRegistry = new Dictionary<int, Type>()
        {
            { 1, typeof(GiantWrenchSkillScript)},
            { 2, typeof(GiantWrenchSkillScript)},
            { 3, typeof(GiantWrenchSkillScript)},
            { 4, typeof(GiantBombSkillScript)},
            { 5, typeof(GiantBombSkillScript)},
            { 6, typeof(GiantBombSkillScript)},
            { 7, typeof(PlierScattershotSkillScript)},
            { 8, typeof(PlierScattershotSkillScript)},
            { 9, typeof(PlierScattershotSkillScript)},
            { 10, typeof(HammerShockSkillScript)},
            { 11, typeof(HammerShockSkillScript)},
            { 12, typeof(HammerShockSkillScript)},
            { 13, typeof(KnifeBurstsSKillScript)},
            { 14, typeof(KnifeBurstsSKillScript)},
            { 15, typeof(KnifeBurstsSKillScript)},
            { 16, typeof(BentPoleCleavingSkillScript)},
            { 17, typeof(BentPoleCleavingSkillScript)},
            { 18, typeof(BentPoleCleavingSkillScript)},
            { 19, typeof(MeteorCrashSkillScript)},
            { 20, typeof(MeteorCrashSkillScript)},
            { 21, typeof(MeteorCrashSkillScript)},
        };

        public static void Apply(SkillData skillData, BaseEntity2D owner, Vector3 targetPosition)
        {
            Type skillType = null;

            if (skillRegistry.TryGetValue(skillData.id, out skillType))
            {
                AbstractSkillScript script = owner.gameObject.AddComponent(skillType) as AbstractSkillScript;
                script.data = skillData;
                script.owner = owner;
                script.targetPosition = targetPosition;
                script.Run();
            }
            else
            {
                LogUtils.LogWarning("Not fount Skill script by ID " + skillData.id);
            }

        }
    }
}