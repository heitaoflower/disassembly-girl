using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlPetConfig
{
	public	int	id;	//id
	public	int	level;	//级别
	public	string	name;	//名字
	public	string	iconID;	//图标ID
	public	string	resourceID;	//资源ID
	public	string	missileResourceID;	//投射物资源ID
	public	string	missileHitEffectID;	//投射物击中资源ID
	public	float	ATK;	//拆解力
	public	float	CRT;	//爆破力
	public	float	SPD;	//速度
	public	float	WOE;	//半径
	public	int	RP;	//价格
	public	int	GP;	//金钱
	public	string	missileAudioID;	//投射物音效资源ID
	public	int	physicsType;	//物理类型0:无 | 1:穿透 | 2: 单次弹射|3: 多重弹射
	public	string	effectors;	//效果器
}

public class DisassemblygirlPetConfigTable
{
	Dictionary<int, DisassemblygirlPetConfig> m_configs = new Dictionary<int,DisassemblygirlPetConfig>();
	public Dictionary<int, DisassemblygirlPetConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlPetConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 16)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlPetConfig rec = new DisassemblygirlPetConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.level = CSVUtility.ToInt(rh.Read());	//级别
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//图标ID
		rec.resourceID = rh.Read();	//资源ID
		rec.missileResourceID = rh.Read();	//投射物资源ID
		rec.missileHitEffectID = rh.Read();	//投射物击中资源ID
		rec.ATK = CSVUtility.ToFloat(rh.Read());	//拆解力
		rec.CRT = CSVUtility.ToFloat(rh.Read());	//爆破力
		rec.SPD = CSVUtility.ToFloat(rh.Read());	//速度
		rec.WOE = CSVUtility.ToFloat(rh.Read());	//半径
		rec.RP = CSVUtility.ToInt(rh.Read());	//价格
		rec.GP = CSVUtility.ToInt(rh.Read());	//金钱
		rec.missileAudioID = rh.Read();	//投射物音效资源ID
		rec.physicsType = CSVUtility.ToInt(rh.Read());	//物理类型0:无 | 1:穿透 | 2: 单次弹射|3: 多重弹射
		rec.effectors = rh.Read();	//效果器
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Pet.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlPetConfig ac = ConfigProcess(row) as DisassemblygirlPetConfig;
			configs.Add(ac.id, ac);
		}
	}
}
