using System.Collections.Generic;

namespace Prototype
{
    public class DungeonData : PrototypeObject
    {
        public string iconID = null;

        public string resourceID = null;

        public string audioID = null;

        public int directorID = default(int);

        public MonsterGroupData currentGroup = null;

        public IList<MonsterGroupData> monsterGroups = null;

        public int groupCounter = 0;

        public bool ActiveMonsterGroup()
        {
            if (HasMonsterGroup())
            {
                groupCounter++;

                currentGroup = monsterGroups[0];

                monsterGroups.RemoveAt(0);

                return true;
            }

            return false;
        }


        public bool HasMonsterGroup()
        {
            return monsterGroups.Count != 0;
        }

        public static DungeonData FromConfig(DisassemblygirlDungeonConfig config)
        {
            DungeonData dungeon = new DungeonData();
            dungeon.id = config.id;
            dungeon.name = config.name;
            dungeon.resourceID = config.resourceID;
            dungeon.audioID = config.audioID;
            dungeon.directorID = config.directorID;

            IList<int> monsterGroupIDs = new List<int>();                    

            foreach (string groupID in config.monsterGroups.Split('|'))
            {
                monsterGroupIDs.Add(int.Parse(groupID));
            }

            dungeon.monsterGroups = new List<MonsterGroupData>(monsterGroupIDs.Count);

            // Generate Monsters By GroupID
            foreach (int groupID in monsterGroupIDs)
            {
                MonsterGroupData group = MonsterGroupData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlMonstergroup.GetConfigById(groupID));

                dungeon.monsterGroups.Add(group);
            }
            
            return dungeon;
        }

    }
}