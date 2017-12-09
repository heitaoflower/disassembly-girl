using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlTrophyConfig
{
	public	int	id;	//id
	public	string	name;	//名字
	public	string	type;	//类型
	public	string	conditionType;	//判断类型
	public	int	threshold;	//数值
	public	string	iconID;	//图标ID
}

public class DisassemblygirlTrophyConfigTable
{
	Dictionary<int, DisassemblygirlTrophyConfig> m_configs = new Dictionary<int,DisassemblygirlTrophyConfig>();
	public Dictionary<int, DisassemblygirlTrophyConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlTrophyConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 6)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlTrophyConfig rec = new DisassemblygirlTrophyConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.name = rh.Read();	//名字
		rec.type = rh.Read();	//类型
		rec.conditionType = rh.Read();	//判断类型
		rec.threshold = CSVUtility.ToInt(rh.Read());	//数值
		rec.iconID = rh.Read();	//图标ID
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Trophy.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlTrophyConfig ac = ConfigProcess(row) as DisassemblygirlTrophyConfig;
			configs.Add(ac.id, ac);
		}
	}
}
