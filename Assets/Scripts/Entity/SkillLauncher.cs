using UnityEngine;
using Prototype;
using Entity.Skill;

namespace Entity
{
    public class SkillLauncher : AbstractLauncher
    {
        private BaseEntity2D owner = null;

        private SkillData skillData = null;

        public override void Bind(BaseEntity2D owner)
        {
            this.owner = owner;
        }

        public override void Unload()
        {
            AbstractSkillScript skillScript = owner.GetComponent<AbstractSkillScript>();

            if (skillScript != null)
            {
                skillScript.Cancel();
            }
        }

        public override void Load(PrototypeObject data)
        {
            this.skillData = data as SkillData;
        }

        public override void Apply(Vector3 targetPosition)
        {
            AbstractSkillScript.Apply(this.skillData, owner, targetPosition);
        }

        public override void LateApply()
        {
           
        }
    }
}