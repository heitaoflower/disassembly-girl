using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlGoldConfig
{
	public	int	id;	//id
	public	string	name;	//名字
	public	string	iconID;	//图标ID
	public	int	value;	//数值
}

public class DisassemblygirlGoldConfigTable
{
	Dictionary<int, DisassemblygirlGoldConfig> m_configs = new Dictionary<int,DisassemblygirlGoldConfig>();
	public Dictionary<int, DisassemblygirlGoldConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlGoldConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 4)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlGoldConfig rec = new DisassemblygirlGoldConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//图标ID
		rec.value = CSVUtility.ToInt(rh.Read());	//数值
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Gold.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlGoldConfig ac = ConfigProcess(row) as DisassemblygirlGoldConfig;
			configs.Add(ac.id, ac);
		}
	}
}
