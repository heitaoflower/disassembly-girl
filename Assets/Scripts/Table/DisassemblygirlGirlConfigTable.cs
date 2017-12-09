using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlGirlConfig
{
	public	int	id;	//id
	public	int	level;	//级别
	public	string	name;	//名字
	public	string	resourceID;	//角色资源
	public	int	STR;	//力量
	public	int	VIT ;	//体力
	public	int	DEX;	//灵巧
	public	int	SPD;	//速度
	public	int	weaponA;	//武器A
	public	int	weaponB;	//武器B
	public	int	weaponC;	//武器C
	public	int	skillA;	//技能A
	public	int	skillB;	//技能B
	public	int	skillC;	//技能C
}

public class DisassemblygirlGirlConfigTable
{
	Dictionary<int, DisassemblygirlGirlConfig> m_configs = new Dictionary<int,DisassemblygirlGirlConfig>();
	public Dictionary<int, DisassemblygirlGirlConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlGirlConfig GetConfigById(int cid)
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
		DisassemblygirlGirlConfig rec = new DisassemblygirlGirlConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.level = CSVUtility.ToInt(rh.Read());	//级别
		rec.name = rh.Read();	//名字
		rec.resourceID = rh.Read();	//角色资源
		rec.STR = CSVUtility.ToInt(rh.Read());	//力量
		rec.VIT  = CSVUtility.ToInt(rh.Read());	//体力
		rec.DEX = CSVUtility.ToInt(rh.Read());	//灵巧
		rec.SPD = CSVUtility.ToInt(rh.Read());	//速度
		rec.weaponA = CSVUtility.ToInt(rh.Read());	//武器A
		rec.weaponB = CSVUtility.ToInt(rh.Read());	//武器B
		rec.weaponC = CSVUtility.ToInt(rh.Read());	//武器C
		rec.skillA = CSVUtility.ToInt(rh.Read());	//技能A
		rec.skillB = CSVUtility.ToInt(rh.Read());	//技能B
		rec.skillC = CSVUtility.ToInt(rh.Read());	//技能C
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Girl.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlGirlConfig ac = ConfigProcess(row) as DisassemblygirlGirlConfig;
			configs.Add(ac.id, ac);
		}
	}
}
