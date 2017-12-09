using System.Collections.Generic;

namespace Prototype
{
    public enum MonsterType : int
    {
        Normal = 1,
        Flying = 2,
        Count
    }
   
    public class MonsterData : PrototypeObject
    {
        public int level = default(int);

        public bool isBoss = false;

        public string resourceID = default(string);

        public string explosionID = default(string);

        public int type = default(int);
        
        public float groundDamping = default(float);

        public float airDamping = default(float);

        public MissileData missileData = null;
        
        public IList<EntityComponentData> componentDatas = null;

        public IList<int> immunityEffectors = null;

        public MonsterData()
        {
        }

        public static MonsterData FromConfig(DisassemblygirlMonsterConfig config)
        {
            MonsterData monster = new MonsterData();
            monster.id = config.id;
            monster.name = config.name;
            monster.level = config.level;
            monster.resourceID = config.resourceID;
            monster.explosionID = config.explosionID;
            monster.type = config.type;
            monster.groundDamping = config.groundDamping;
            monster.airDamping = config.airDamping;
            monster.componentDatas = new List<EntityComponentData>();

            monster.missileData = new MissileData();
            monster.missileData.resourceID = config.missileResourceID;

            if (!string.IsNullOrEmpty(config.components) && config.components != "0")
            {
                string[] components = config.components.Split('|');

                foreach (string component in components)
                {
                    EntityComponentData data = EntityComponentData.Parse(component);
                
                    monster.componentDatas.Add(data);
                }
            }      
 
            monster.attributeBox = AttributeBox.CreateDefault();
            monster.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);
            monster.attributeBox.SetAttribute(AttributeKeys.ANTI, AttributeSetTypes.BaseValue, config.ANTI);
            monster.attributeBox.SetAttribute(AttributeKeys.HP, AttributeSetTypes.BaseValue, config.HP);
            monster.attributeBox.SetAttribute(AttributeKeys.MaxHP, AttributeSetTypes.BaseValue, config.HP);
            monster.attributeBox.SetAttribute(AttributeKeys.DEF, AttributeSetTypes.BaseValue, config.DEF);
            monster.attributeBox.SetAttribute(AttributeKeys.RP, AttributeSetTypes.BaseValue, config.RP);
            monster.attributeBox.SetAttribute(AttributeKeys.GP, AttributeSetTypes.BaseValue, config.GP);

            monster.immunityEffectors = new List<int>();
            foreach (string effectorID in config.immunityEffectors.Split('|'))
            {
                if (effectorID != "0")
                {
                    monster.immunityEffectors.Add(int.Parse(effectorID));
                }               
            }

            return monster;
        }
    }
}