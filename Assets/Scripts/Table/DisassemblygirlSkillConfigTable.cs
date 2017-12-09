using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlSkillConfig
{
	public	int	id;	//id
	public	string	name;	//名字
	public	string	iconID;	//技能图标
	public	float	ATK;	//拆解力
	public	float	CRT;	//爆破力
	public	float	SPD;	//速度
	public	float	WOE;	//半径
	public	float	CD;	//冷却
	public	int	type;	//技能类型
	public	int	vibrateType;	//震动类型 0:无 | 1:命中 | 2:释放
	public	string	resourceID;	//技能动画ID
	public	string	hitEffectID;	//击中动画ID
	public	int	physicsType;	//物理类型0:无 | 1:穿透 | 2: 单次弹射 3:多重弹射
	public	string	effectors;	//效果器
}

public class DisassemblygirlSkillConfigTable
{
	Dictionary<int, DisassemblygirlSkillConfig> m_configs = new Dictionary<int,DisassemblygirlSkillConfig>();
	public Dictionary<int, DisassemblygirlSkillConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlSkillConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 14)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlSkillConfig rec = new DisassemblygirlSkillConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//技能图标
		rec.ATK = CSVUtility.ToFloat(rh.Read());	//拆解力
		rec.CRT = CSVUtility.ToFloat(rh.Read());	//爆破力
		rec.SPD = CSVUtility.ToFloat(rh.Read());	//速度
		rec.WOE = CSVUtility.ToFloat(rh.Read());	//半径
		rec.CD = CSVUtility.ToFloat(rh.Read());	//冷却
		rec.type = CSVUtility.ToInt(rh.Read());	//技能类型
		rec.vibrateType = CSVUtility.ToInt(rh.Read());	//震动类型 0:无 | 1:命中 | 2:释放
		rec.resourceID = rh.Read();	//技能动画ID
		rec.hitEffectID = rh.Read();	//击中动画ID
		rec.physicsType = CSVUtility.ToInt(rh.Read());	//物理类型0:无 | 1:穿透 | 2: 单次弹射 3:多重弹射
		rec.effectors = rh.Read();	//效果器
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Skill.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlSkillConfig ac = ConfigProcess(row) as DisassemblygirlSkillConfig;
			configs.Add(ac.id, ac);
		}
	}
}
