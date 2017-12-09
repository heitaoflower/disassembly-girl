using System.Collections.Generic;

namespace Prototype
{
    public enum SkillVibrateType : int
    {
        None = 0,
        Hit,
        Apply,
        Count
    }
    public class SkillData : PrototypeObject
    {
        public string iconID = null;

        public float WOE = default(float);

        public float CD = default(float);

        public int type = default(int);

        public SkillVibrateType vibrateType = SkillVibrateType.None;

        public string resourceID = null;

        public string hitEffectID = null;

        public IList<EffectorData> effectors = null;

        public PhysicsType physicsType = PhysicsType.Default;

        public static SkillData FromConfig(int id)
        {
            return FromConfig(ConfigMgr.GetInstance().DisassemblygirlSkill.GetConfigById(id));
        }

        public static SkillData FromConfig(DisassemblygirlSkillConfig config)
        {
            SkillData skill = new SkillData();
            skill.id = config.id;
            skill.name = config.name;
            skill.iconID = config.iconID;
            skill.WOE = config.WOE;
            skill.CD = config.CD;
            skill.type = config.type;
            skill.vibrateType = (SkillVibrateType)config.vibrateType;
            skill.resourceID = config.resourceID;
            skill.hitEffectID = config.hitEffectID;
            skill.physicsType = (PhysicsType)config.physicsType;

            skill.attributeBox = AttributeBox.CreateDefault();
            skill.attributeBox.SetAttribute(AttributeKeys.ATK, AttributeSetTypes.BaseValue, config.ATK);
            skill.attributeBox.SetAttribute(AttributeKeys.CRT, AttributeSetTypes.BaseValue, config.CRT);
            skill.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);
     
            skill.effectors = new List<EffectorData>();

            foreach (string effectorID in config.effectors.Split('|'))
            {
                EffectorData effector = EffectorData.FromConfig(int.Parse(effectorID));

                if (effector != null)
                {
                    skill.effectors.Add(effector);
                }
            }
            return skill;
        }
    }
}