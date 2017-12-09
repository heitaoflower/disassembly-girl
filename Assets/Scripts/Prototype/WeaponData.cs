using System.Collections.Generic;

namespace Prototype
{
    public enum WeaponType : int
    {
        A, B, C, Count
    }

    public class WeaponData : PrototypeObject
    {
        public int level = default(int);

        public string iconID = default(string);

        public float WOE = default(float);

        public float CD = default(float);

        public int GP = default(int);

        public int RP = default(int);

        public string resourceID = default(string);

        public string hitEffectID = default(string);

        public string audioID = default(string);

        public float offsetX = default(float);

        public float offsetY = default(float);

        public SkillData skillData = null;

        public PhysicsType physicsType = PhysicsType.Default;

        public IList<EffectorData> effectors = null;

        private WeaponData()
        {

        }

        public bool LevelMax()
        {
            return level == 0 ? true : false;
        }

        public bool LevelUP()
        {
            if (LevelMax())
            {
                return false;
            }

            DisassemblygirlWeaponConfig config = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(id + 1);

            id = config.id;
            level = config.level;
            name = config.name;
            iconID = config.iconID;
        
            attributeBox.SetAttribute(AttributeKeys.ATK, AttributeSetTypes.BaseValue, config.ATK);
            attributeBox.SetAttribute(AttributeKeys.CRT, AttributeSetTypes.BaseValue, config.CRT);
            attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);

            WOE = config.WOE;
            GP = config.GP;
            RP = config.RP;
            CD = config.CD;

            resourceID = config.resourceID;
            hitEffectID = config.hitEffectID;
            audioID = config.audioID;

            offsetX = config.offsetX;
            offsetY = config.offsetY;
            physicsType = (PhysicsType)config.physicsType;

            if (config.skillID != 0)
            {
                skillData = SkillData.FromConfig(config.skillID);
            }

            effectors.Clear();

            foreach (string effectorID in config.effectors.Split('|'))
            {
                EffectorData effector = EffectorData.FromConfig(int.Parse(effectorID));

                if (effector != null)
                {
                    effectors.Add(effector);
                }
            }
            return true;
        }

        public static WeaponData FromConfig(DisassemblygirlWeaponConfig config)
        {
            WeaponData weapon = new WeaponData();
            weapon.id = config.id;
            weapon.level = config.level;
            weapon.name = config.name;
            weapon.iconID = config.iconID;
            weapon.attributeBox = AttributeBox.CreateDefault();
            weapon.attributeBox.SetAttribute(AttributeKeys.ATK, AttributeSetTypes.BaseValue, config.ATK);
            weapon.attributeBox.SetAttribute(AttributeKeys.CRT, AttributeSetTypes.BaseValue, config.CRT);
            weapon.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);
  
            weapon.WOE = config.WOE;
            weapon.CD = config.CD;
            weapon.GP = config.GP;
            weapon.RP = config.RP;

            weapon.resourceID = config.resourceID;
            weapon.hitEffectID = config.hitEffectID;
            weapon.audioID = config.audioID;
            weapon.offsetX = config.offsetX;
            weapon.offsetY = config.offsetY;
            weapon.physicsType = (PhysicsType)config.physicsType;

            if (config.skillID != 0)
            {
                weapon.skillData = SkillData.FromConfig(config.skillID);
            }

            weapon.effectors = new List<EffectorData>();
            
            foreach (string effectorID in config.effectors.Split('|'))
            {
                EffectorData effector = EffectorData.FromConfig(int.Parse(effectorID));

                if (effector != null)
                {
                    weapon.effectors.Add(effector);
                }
            }         

            return weapon;
        }
    }
}
