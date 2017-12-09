using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlEffectorConfig
{
	public	int	id;	//id
	public	string	name;	//名字
	public	string	iconID;	//图标ID
	public	string	resourceID;	//资源ID
	public	float	parameter1;	//参数1
	public	float	parameter2;	//参数2
	public	float	parameter3;	//参数3
	public	float	parameter4;	//参数4
	public	float	parameter5;	//参数5
}

public class DisassemblygirlEffectorConfigTable
{
	Dictionary<int, DisassemblygirlEffectorConfig> m_configs = new Dictionary<int,DisassemblygirlEffectorConfig>();
	public Dictionary<int, DisassemblygirlEffectorConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlEffectorConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 9)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlEffectorConfig rec = new DisassemblygirlEffectorConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//图标ID
		rec.resourceID = rh.Read();	//资源ID
		rec.parameter1 = CSVUtility.ToFloat(rh.Read());	//参数1
		rec.parameter2 = CSVUtility.ToFloat(rh.Read());	//参数2
		rec.parameter3 = CSVUtility.ToFloat(rh.Read());	//参数3
		rec.parameter4 = CSVUtility.ToFloat(rh.Read());	//参数4
		rec.parameter5 = CSVUtility.ToFloat(rh.Read());	//参数5
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Effector.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlEffectorConfig ac = ConfigProcess(row) as DisassemblygirlEffectorConfig;
			configs.Add(ac.id, ac);
		}
	}
}
