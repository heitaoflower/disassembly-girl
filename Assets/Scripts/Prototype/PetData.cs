using System.Collections.Generic;

namespace Prototype
{
    public class PetData : PrototypeObject
    {
        public int level = default(int);

        public string iconID = default(string);

        public int GP = default(int);

        public int RP = default(int);

        public string resourceID = default(string);

        public MissileData missileData = null;

        private PetData()
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

            DisassemblygirlPetConfig config = ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(id + 1);

            id = config.id;
            level = config.level;
            name = config.name;
            iconID = config.iconID;
            GP = config.GP;
            RP = config.RP;
            resourceID = config.resourceID;

            missileData.attributeBox.SetAttribute(AttributeKeys.ATK, AttributeSetTypes.BaseValue, config.ATK);
            missileData.attributeBox.SetAttribute(AttributeKeys.CRT, AttributeSetTypes.BaseValue, config.CRT);
            missileData.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);

            missileData.WOE = config.WOE;
            missileData.resourceID = config.missileResourceID;
            missileData.hitEffectID = config.missileHitEffectID;
            missileData.audioID = config.missileAudioID;
            missileData.physicsType = (PhysicsType)config.physicsType;
            missileData.effectors.Clear();

            foreach (string effectorID in config.effectors.Split('|'))
            {
                EffectorData effector = EffectorData.FromConfig(int.Parse(effectorID));

                if (effector != null)
                {
                    missileData.effectors.Add(effector);
                }
            }
            return true;
        }

        public static PetData FromConfig(DisassemblygirlPetConfig config)
        {
            PetData pet = new PetData();
            pet.id = config.id;
            pet.level = config.level;
            pet.name = config.name;
            pet.iconID = config.iconID;
            pet.GP = config.GP;
            pet.RP = config.RP;
            pet.resourceID = config.resourceID;
            
            pet.missileData = new MissileData();
            pet.missileData.attributeBox = AttributeBox.CreateDefault();
            pet.missileData.attributeBox.SetAttribute(AttributeKeys.ATK, AttributeSetTypes.BaseValue, config.ATK);
            pet.missileData.attributeBox.SetAttribute(AttributeKeys.CRT, AttributeSetTypes.BaseValue, config.CRT);
            pet.missileData.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD); 

            pet.missileData.WOE = config.WOE;
            pet.missileData.resourceID = config.missileResourceID;
            pet.missileData.hitEffectID = config.missileHitEffectID;
            pet.missileData.audioID = config.missileAudioID;
            pet.missileData.physicsType = (PhysicsType)config.physicsType;
            pet.missileData.effectors = new List<EffectorData>();

            foreach (string effectorID in config.effectors.Split('|'))
            {
                EffectorData effector = EffectorData.FromConfig(int.Parse(effectorID));

                if (effector != null)
                {
                    pet.missileData.effectors.Add(effector);
                }
            }

            return pet;
        }
    }
}
