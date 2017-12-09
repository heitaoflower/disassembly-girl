using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlMonstergroupConfig
{
	public	int	id;	//id
	public	string	monstersA;	//怪物组A
	public	string	monstersB;	//怪物组B
	public	string	monstersC;	//怪物组C
	public	string	monstersD;	//怪物组D
	public	string	monstersE;	//怪物组E
	public	int	boss;	//bossID
	public	int	joinCount;	//每次加入战场单位数量
	public	float	joinInterval;	//加入间隔(秒)
	public	int	isBoss;	//是否是BOSS组
}

public class DisassemblygirlMonstergroupConfigTable
{
	Dictionary<int, DisassemblygirlMonstergroupConfig> m_configs = new Dictionary<int,DisassemblygirlMonstergroupConfig>();
	public Dictionary<int, DisassemblygirlMonstergroupConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlMonstergroupConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 10)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlMonstergroupConfig rec = new DisassemblygirlMonstergroupConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.monstersA = rh.Read();	//怪物组A
		rec.monstersB = rh.Read();	//怪物组B
		rec.monstersC = rh.Read();	//怪物组C
		rec.monstersD = rh.Read();	//怪物组D
		rec.monstersE = rh.Read();	//怪物组E
		rec.boss = CSVUtility.ToInt(rh.Read());	//bossID
		rec.joinCount = CSVUtility.ToInt(rh.Read());	//每次加入战场单位数量
		rec.joinInterval = CSVUtility.ToFloat(rh.Read());	//加入间隔(秒)
		rec.isBoss = CSVUtility.ToInt(rh.Read());	//是否是BOSS组
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Monstergroup.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlMonstergroupConfig ac = ConfigProcess(row) as DisassemblygirlMonstergroupConfig;
			configs.Add(ac.id, ac);
		}
	}
}
