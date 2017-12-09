using System;
namespace Prototype
{
    public enum TrophyConditionType : int
    {
        Unkonw = 0,
        Value = 1,
        Frequency
    }
    public enum TrophyType : int
    {
        Unknown = 0,
        UserGP,
        GirlDead,
        GirlSkill,
        GirlAttack,
        WeaponLevelMax,
        PetLevelMax,
        ShopLevelMax,
        GoldHit,
        GoldBet,
        DungeonClear,
        DungeonKillMonster,
        DungeonKillMissile
    }

    public class TrophyData : PrototypeObject
    {
        public TrophyType type = default(TrophyType);

        public TrophyConditionType conditionType = default(TrophyConditionType);

        public float value = default(float);

        public int threshold = default(int);

        public bool isCompleted = false;

        public string iconID = null;

        public static TrophyData FromConfig(DisassemblygirlTrophyConfig config)
        {
            TrophyData data = new TrophyData();
            data.id = config.id;
            data.name = config.name;
            data.threshold = config.threshold;
            data.type = (TrophyType)Enum.Parse(typeof(TrophyType), config.type);
            data.conditionType = (TrophyConditionType)Enum.Parse(typeof(TrophyConditionType), config.conditionType);
            data.iconID = config.iconID;
            return data;
        }
    }

}