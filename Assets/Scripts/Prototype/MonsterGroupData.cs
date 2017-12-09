using System.Collections.Generic;
using System;
using System.Linq;
using Map;

namespace Prototype
{
    public class MonsterGroupData
    {
        public int id = default(int);

        public bool isBoss = false;

        public MonsterData boss = null;

        public IList<MonsterData> monstersA = null;

        public IList<MonsterData> monstersB = null;

        public IList<MonsterData> monstersC = null;

        public IList<MonsterData> monstersD = null;

        public IList<MonsterData> monstersE = null;

        public float jointInterval = default(float);

        public int joinCount = default(int);

        public bool HasMonsters()
        {
            return (monstersA.Count != 0 | monstersB.Count != 0 | monstersC.Count != 0 | monstersD.Count != 0 | monstersE.Count != 0);
        }

        public IDictionary<SpwanPointIds, IList<MonsterData>> GetAllJoinMonsters()
        {
            IDictionary< SpwanPointIds, IList < MonsterData >> result = new Dictionary<SpwanPointIds, IList<MonsterData>>();

            foreach (SpwanPointIds id in Enum.GetValues(typeof(SpwanPointIds)))
            {
                result.Add(id, GetJoinMonsters(id));
            }

            return result;
        }

        public IList<MonsterData> GetJoinMonsters(SpwanPointIds pointID)
        {
            Func<IList<MonsterData>, int, IList<MonsterData>> TakeMonsters = (IList<MonsterData> monsters, int joinCount) =>
            {
                IList<MonsterData> results = monsters.Take<MonsterData>(joinCount).ToList();

                foreach (MonsterData monster in results)
                {
                    monsters.Remove(monster);
                }

                return results;
            };

            List<MonsterData> joins = new List<MonsterData>();

            if (pointID == SpwanPointIds.A)
            {
                joins.AddRange(TakeMonsters(monstersA, joinCount));
            }
            else if (pointID == SpwanPointIds.B)
            {
                joins.AddRange(TakeMonsters(monstersB, joinCount));
            }
            else if (pointID == SpwanPointIds.C)
            {
                joins.AddRange(TakeMonsters(monstersC, joinCount));
            }
            else if (pointID == SpwanPointIds.D)
            {
                joins.AddRange(TakeMonsters(monstersD, joinCount));
            }
            else if (pointID == SpwanPointIds.E)
            {
                joins.AddRange(TakeMonsters(monstersE, joinCount));
            }

            return joins;
        }

        public static MonsterGroupData FromConfig(DisassemblygirlMonstergroupConfig config)
        {
            Action<IList<int>> RandomShuffle = (IList<int> input) =>
            {
                Random random = new Random();

                int currentIndex = 0;
                int tempValue = 0;

                for (int index = 0; index < input.Count; index++)
                {
                    currentIndex = random.Next(0, input.Count - index);
                    tempValue = input[currentIndex];
                    input[currentIndex] = input[input.Count - 1 - index];
                    input[input.Count - 1 - index] = tempValue;
                }
            };

            Func<string, IList<MonsterData>> WrapperMonsters = (string monsters)=>
            {
                IList<MonsterData> monsterDatas = new List<MonsterData>();

                if (monsters == "0")
                {
                    return monsterDatas;
                }

                IList<int> sourceIds = new List<int>();

                foreach (string monsterConfig in monsters.Split('|'))
                {
                    string[] parameters = monsterConfig.Split(':');

                    for (int index = 0, length = int.Parse(parameters[1]); index < length; index++)
                    {
                        sourceIds.Add(int.Parse(parameters[0]));
                    }
                }

                RandomShuffle(sourceIds);

                foreach (int monsterID in sourceIds)
                {
                    MonsterData monster = MonsterData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlMonster.GetConfigById(monsterID));
                    monsterDatas.Add(monster);
                }

                return monsterDatas;
            };

            MonsterGroupData group = new MonsterGroupData();
            group.id = config.id;
            group.jointInterval = config.joinInterval;
            group.joinCount = config.joinCount;
            group.isBoss = config.isBoss == 0 ? false : true;

            group.monstersA = WrapperMonsters(config.monstersA);
            group.monstersB = WrapperMonsters(config.monstersB);
            group.monstersC = WrapperMonsters(config.monstersC);
            group.monstersD = WrapperMonsters(config.monstersD);
            group.monstersE = WrapperMonsters(config.monstersE);

            if (config.boss != 0)
            {
                group.boss = MonsterData.FromConfig(ConfigMgr.GetInstance().DisassemblygirlMonster.GetConfigById(config.boss));
                group.boss.isBoss = true;
            }

            return group;
        }
    }
}
