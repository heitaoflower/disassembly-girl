using System.Collections;
using System.Collections.Generic;
public class DisassemblygirlDungeonConfig
{
	public	int	id;	//id
	public	string	name;	//名字
	public	string	iconID;	//图标ID
	public	int	directorID;	//脚本ID
	public	string	resourceID;	//资源ID
	public	string	audioID;	//音乐资源ID
	public	string	monsterGroups;	//怪物组
}

public class DisassemblygirlDungeonConfigTable
{
	Dictionary<int, DisassemblygirlDungeonConfig> m_configs = new Dictionary<int,DisassemblygirlDungeonConfig>();
	public Dictionary<int, DisassemblygirlDungeonConfig> configs
	{
		get {
			return m_configs;
		}
	}

	public DisassemblygirlDungeonConfig GetConfigById(int cid)
	{
		if(m_configs.ContainsKey(cid)){
			return m_configs[cid];
		}
		return null;
	}

	public object ConfigProcess(string[] row)
	{
		if (row.Length < 7)
		{
			return null;
		}

		RowHelper rh = new RowHelper(row);
		DisassemblygirlDungeonConfig rec = new DisassemblygirlDungeonConfig();

		rec.id = CSVUtility.ToInt(rh.Read());	//id
		rec.name = rh.Read();	//名字
		rec.iconID = rh.Read();	//图标ID
		rec.directorID = CSVUtility.ToInt(rh.Read());	//脚本ID
		rec.resourceID = rh.Read();	//资源ID
		rec.audioID = rh.Read();	//音乐资源ID
		rec.monsterGroups = rh.Read();	//怪物组
		return rec;
	}

	public void Load()
	{
		CSVReader reader = new CSVReader();
		reader.LoadText("Config/Disassemblygirl_Dungeon.txt", 3);
		int rows = reader.GetRowCount();
		for (int r = 0; r < rows; ++r)
		{
			string[] row = reader.GetRow(r);
			DisassemblygirlDungeonConfig ac = ConfigProcess(row) as DisassemblygirlDungeonConfig;
			configs.Add(ac.id, ac);
		}
	}
}
