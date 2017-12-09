using System.Collections.Generic;

namespace Prototype
{
    public class UserData : PrototypeObject
    {
        public GirlData girl = null;

        public int girlID = default(int);

        public int dungeonIndex = default(int);

        public IList<FunctionData> functions = null;

        public ICollection<int> guides = null;

        public WeaponData weaponSlotI = null;

        public WeaponData weaponSlotII = null;

        public WeaponData weaponSlotIII = null;

        public IList<PetData> petDatas = null;

        public IList<TrophyData> trophyDatas = null;

        public int activePetID = default(int);

        public SlotIndex activeSlotIndex = SlotIndex.None;

        public ShopData shopData = null;

        public PetData GetActivePet()
        {
            foreach (PetData petData in petDatas)
            {
                if (petData.id == activePetID)
                {
                    return petData;
                }
            }

            return null;
        }

        public WeaponData GetActiveWeapon()
        {
            if (activeSlotIndex == SlotIndex.I)
            {
                return weaponSlotI;
            }
            else if (activeSlotIndex == SlotIndex.II)
            {
                return weaponSlotII;
            }
            else if (activeSlotIndex == SlotIndex.III)
            {
                return weaponSlotIII;
            }

            return null;
        }

        public static UserData LoadFromJson(string json)
        {
            UserData user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserData>(json);

            if (user.weaponSlotI != null)
            {
                user.weaponSlotI = user.girl.GetWeapon(user.weaponSlotI.id);

                if (user.weaponSlotI.skillData != null)
                {
                    user.weaponSlotI.skillData.state = PrototypeState.Normal;
                }
            }

            if (user.weaponSlotII != null)
            {
                user.weaponSlotII = user.girl.GetWeapon(user.weaponSlotII.id);

                if (user.weaponSlotII.skillData != null)
                {
                    user.weaponSlotII.skillData.state = PrototypeState.Normal;
                }
            }

            if (user.weaponSlotIII != null)
            {
                user.weaponSlotIII = user.girl.GetWeapon(user.weaponSlotIII.id);

                if (user.weaponSlotIII.skillData != null)
                {
                    user.weaponSlotIII.skillData.state = PrototypeState.Normal;
                }
            }

            return user;
        }

        public static UserData CreateDefault()
        {
            UserData user = new UserData();
            user.dungeonIndex = 26;
            user.girlID = 1;
            user.functions = new List<FunctionData>();
            user.guides = new HashSet<int>();
            user.petDatas = new List<PetData>();

            user.attributeBox = AttributeBox.CreateDefault();
            user.attributeBox.SetAttribute(AttributeKeys.GP, AttributeSetTypes.BaseValue, 999999);
            user.attributeBox.SetAttribute(AttributeKeys.RP, AttributeSetTypes.BaseValue, 999999);

            // Attach Pets
            user.petDatas.Add(PetData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(1)));
            user.petDatas.Add(PetData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(7)));
            user.petDatas.Add(PetData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(13)));

            DisassemblygirlGirlConfig config = ConfigMgr.GetInstance().DisassemblygirlGirl.GetConfigById(user.girlID);
            user.girl = GirlData.FromConfig(config);

            // Attach Weapons
            user.girl.AddWeapon(7);
            user.girl.AddWeapon(13);
            user.girl.AddWeapon(19);
            user.girl.AddWeapon(25);
            user.girl.AddWeapon(31);
            user.girl.AddWeapon(37);

            // Shop Items
            user.shopData = new ShopData();
            user.shopData.attributeBox = AttributeBox.CreateDefault();
            user.shopData.attributeBox.SetAttribute(AttributeKeys.STR, AttributeSetTypes.BaseValue, 0f);
            user.shopData.attributeBox.SetAttribute(AttributeKeys.VIT, AttributeSetTypes.BaseValue, 0f);
            user.shopData.attributeBox.SetAttribute(AttributeKeys.DEX, AttributeSetTypes.BaseValue, 0f);
            user.shopData.attributeBox.SetAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, 0f);

            // Trophies
            user.trophyDatas = new List<TrophyData>();
            foreach (DisassemblygirlTrophyConfig trophyConfig in ConfigMgr.GetInstance().DisassemblygirlTrophy.configs.Values)
            {
                user.trophyDatas.Add(TrophyData.FromConfig(trophyConfig));
            }
            return user;
        }
    }
}