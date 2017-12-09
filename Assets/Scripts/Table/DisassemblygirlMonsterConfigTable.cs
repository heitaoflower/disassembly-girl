using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlMonsterConfig
{
	public	int	id;	//id
	public	int	level;	//级别
	public	string	name;	//名字
	public	string	resourceID;	//资源ID
	public	string	explosionID;	//爆炸资源ID
	public	string	missileResourceID;	//投射物资源ID
	public	int	type;	//类型
	public	float	DEF;	//防御力
	public	float	ANTI;	//抵抗力
	public	float	SPD;	//速度
	public	int	HP;	//生命值
	public	string	components;	//部件吊掉落配置
	public	float	groundDamping;	//地面阻力
	public	float	airDamping;	//空中阻力
	public	string	immunityEffectors;	//效果器免疫
	public	int	RP;	//RP
	public	int	GP;	//GP
}

public class DisassemblygirlMonsterConfigTable
{
	Dictionary<int, DisassemblygirlMonsterConfig> m_configs = new Dictionary<int,DisassemblygirlMonsterConfig>();
	public Dictionary<int, DisassemblygirlMonsterConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlMonsterConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 17)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlMonsterConfig rec = new DisassemblygirlMonsterConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.level = CSVUtility.ToInt(rh.Read());	//级别
		rec.name = rh.Read();	//名字
		rec.resourceID = rh.Read();	//资源ID
		rec.explosionID = rh.Read();	//爆炸资源ID
		rec.missileResourceID = rh.Read();	//投射物资源ID
		rec.type = CSVUtility.ToInt(rh.Read());	//类型
		rec.DEF = CSVUtility.ToFloat(rh.Read());	//防御力
		rec.ANTI = CSVUtility.ToFloat(rh.Read());	//抵抗力
		rec.SPD = CSVUtility.ToFloat(rh.Read());	//速度
		rec.HP = CSVUtility.ToInt(rh.Read());	//生命值
		rec.components = rh.Read();	//部件吊掉落配置
		rec.groundDamping = CSVUtility.ToFloat(rh.Read());	//地面阻力
		rec.airDamping = CSVUtility.ToFloat(rh.Read());	//空中阻力
		rec.immunityEffectors = rh.Read();	//效果器免疫
		rec.RP = CSVUtility.ToInt(rh.Read());	//RP
		rec.GP = CSVUtility.ToInt(rh.Read());	//GP
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Monster.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlMonsterConfig ac = ConfigProcess(row) as DisassemblygirlMonsterConfig;
			configs.Add(ac.id, ac);
		}
	}
}
