using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlWeaponConfig
{
	public	int	id;	//id
	public	int	level;	//级别
	public	string	name;	//名字
	public	string	iconID;	//武器图标
	public	float	ATK;	//拆解力
	public	float	CRT;	//爆破力
	public	float	SPD;	//速度
	public	float	WOE;	//半径
	public	float	CD;	//间隔
	public	int	RP;	//价格
	public	int	GP;	//金钱
	public	string	resourceID;	//资源ID
	public	string	hitEffectID;	//击中效果资源ID
	public	string	audioID;	//音效资源ID
	public	float	offsetX;	//武器偏移X
	public	float	offsetY;	//武器偏移Y
	public	int	skillID;	//技能ID
	public	int	physicsType;	//物理类型0:无 | 1:穿透 | 2: 单次弹射|3: 多重弹射
	public	string	effectors;	//效果器
}

public class DisassemblygirlWeaponConfigTable
{
	Dictionary<int, DisassemblygirlWeaponConfig> m_configs = new Dictionary<int,DisassemblygirlWeaponConfig>();
	public Dictionary<int, DisassemblygirlWeaponConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlWeaponConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 19)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlWeaponConfig rec = new DisassemblygirlWeaponConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.level = CSVUtility.ToInt(rh.Read());	//级别
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//武器图标
		rec.ATK = CSVUtility.ToFloat(rh.Read());	//拆解力
		rec.CRT = CSVUtility.ToFloat(rh.Read());	//爆破力
		rec.SPD = CSVUtility.ToFloat(rh.Read());	//速度
		rec.WOE = CSVUtility.ToFloat(rh.Read());	//半径
		rec.CD = CSVUtility.ToFloat(rh.Read());	//间隔
		rec.RP = CSVUtility.ToInt(rh.Read());	//价格
		rec.GP = CSVUtility.ToInt(rh.Read());	//金钱
		rec.resourceID = rh.Read();	//资源ID
		rec.hitEffectID = rh.Read();	//击中效果资源ID
		rec.audioID = rh.Read();	//音效资源ID
		rec.offsetX = CSVUtility.ToFloat(rh.Read());	//武器偏移X
		rec.offsetY = CSVUtility.ToFloat(rh.Read());	//武器偏移Y
		rec.skillID = CSVUtility.ToInt(rh.Read());	//技能ID
		rec.physicsType = CSVUtility.ToInt(rh.Read());	//物理类型0:无 | 1:穿透 | 2: 单次弹射|3: 多重弹射
		rec.effectors = rh.Read();	//效果器
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Weapon.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlWeaponConfig ac = ConfigProcess(row) as DisassemblygirlWeaponConfig;
			configs.Add(ac.id, ac);
		}
	}
}
