using System.Collections;

public class ConfigMgr
{
	public static ConfigMgr  GetInstance() 
	{ 
		if(null == mInstance)
		{
			mInstance = new ConfigMgr();
			mInstance.Init(); 
		}
		return mInstance;
	}
	private static ConfigMgr mInstance = null;

	public DisassemblygirlDungeonConfigTable	DisassemblygirlDungeon;
	public DisassemblygirlEffectorConfigTable	DisassemblygirlEffector;
	public DisassemblygirlGirlConfigTable	DisassemblygirlGirl;
	public DisassemblygirlGoldConfigTable	DisassemblygirlGold;
	public DisassemblygirlMonsterConfigTable	DisassemblygirlMonster;
	public DisassemblygirlMonstergroupConfigTable	DisassemblygirlMonstergroup;
	public DisassemblygirlPetConfigTable	DisassemblygirlPet;
	public DisassemblygirlSkillConfigTable	DisassemblygirlSkill;
	public DisassemblygirlTrophyConfigTable	DisassemblygirlTrophy;
	public DisassemblygirlWeaponConfigTable	DisassemblygirlWeapon;

	public void Init () 
	{
		DisassemblygirlDungeon = new DisassemblygirlDungeonConfigTable();
		DisassemblygirlDungeon.Load();
		DisassemblygirlEffector = new DisassemblygirlEffectorConfigTable();
		DisassemblygirlEffector.Load();
		DisassemblygirlGirl = new DisassemblygirlGirlConfigTable();
		DisassemblygirlGirl.Load();
		DisassemblygirlGold = new DisassemblygirlGoldConfigTable();
		DisassemblygirlGold.Load();
		DisassemblygirlMonster = new DisassemblygirlMonsterConfigTable();
		DisassemblygirlMonster.Load();
		DisassemblygirlMonstergroup = new DisassemblygirlMonstergroupConfigTable();
		DisassemblygirlMonstergroup.Load();
		DisassemblygirlPet = new DisassemblygirlPetConfigTable();
		DisassemblygirlPet.Load();
		DisassemblygirlSkill = new DisassemblygirlSkillConfigTable();
		DisassemblygirlSkill.Load();
		DisassemblygirlTrophy = new DisassemblygirlTrophyConfigTable();
		DisassemblygirlTrophy.Load();
		DisassemblygirlWeapon = new DisassemblygirlWeaponConfigTable();
		DisassemblygirlWeapon.Load();

	}
}