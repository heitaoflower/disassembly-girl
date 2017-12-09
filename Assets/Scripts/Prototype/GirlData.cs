using System.Collections.Generic;
using System.Linq;
namespace Prototype
{
    public class GirlData : PrototypeObject
    {
        public int level = default(int);

        public string resourceID = default(string);

        public IList<WeaponData> weapons = null;

        private GirlData()
        {

        }

        public void AddWeapon(int weaponID)
        {
            DisassemblygirlWeaponConfig config = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(weaponID);

            if (config != null)
            {
                weapons.Add(WeaponData.FromConfig(config));
            }
        }

        public WeaponData GetWeapon(int id)
        {
            var query = from WeaponData weapon in weapons
                        where weapon.id == id
                        select weapon;

            foreach (WeaponData weapon in query)
            {
                return weapon;
            }

            return null;
        }

        public static GirlData FromConfig(DisassemblygirlGirlConfig config)
        {
            GirlData girl = new GirlData();
            girl.id = config.id;
            girl.name = config.name;
            girl.level = config.level;
            girl.resourceID = config.resourceID;

            girl.attributeBox = AttributeBox.CreateDefault();
            girl.attributeBox.SetAttribute(AttributeKeys.DEX, AttributeSetTypes.BaseValue, config.DEX);
            girl.attributeBox.SetAttribute(AttributeKeys.VIT, AttributeSetTypes.BaseValue, config.VIT);
            girl.attributeBox.SetAttribute(AttributeKeys.STR, AttributeSetTypes.BaseValue, config.STR);
            girl.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, config.SPD);
  
            girl.weapons = new List<WeaponData>();
            foreach (int weaponID in new int[] { config.weaponA, config.weaponB, config.weaponC })
            {
                girl.AddWeapon(weaponID);
            }       

            return girl;
        }
    }
}