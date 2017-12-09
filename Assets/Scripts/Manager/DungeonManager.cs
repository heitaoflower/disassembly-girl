using Utils;
using Entity;
using Entity.Missile;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using View.Dungeon;
using Orange.TransitionKit;
using Prototype;
using Map;
using Entity.AI;

namespace Manager
{
    public enum BattleResult : int
    {
        Win = 0,
        Lose,
        Count
    }

    public enum BattleStatus : int
    {
        None = 0,
        Start,
        Pause,
        Finish,
        Count
    }

    public class DungeonManager : SceneManager<DungeonManager>
    {

        private Task battleTask = null;           

        private DungeonData dungeon = null;

        public BattleStatus battleStatus = BattleStatus.None;

        public int currentDungeonIndex = default(int);

        public GirlEntity2D girlEntity = null;

        private PetEntity2D petEntity = null;

        public MapWrapper mapWrapper = null;

        public IList<MonsterEntity2D> monsters = null;

        public IList<AbstractMissileUI> missiles = null;
        
        public override void Initialize()
        { 
            base.Initialize();

            AddEventLisenters();
        }

        private void AddEventLisenters()
        {
            EventBox.Add(CustomEvent.DUNGEON_MONSTER_DAMAGE, OnDungeonMonsterDamage);
            EventBox.Add(CustomEvent.DUNGEON_GIRL_DEAD, OnDungeonGirlDead);
            EventBox.Add(CustomEvent.DUNGEON_MONSTER_TRANSFER, OnDungeonMonsterTransfer);
            EventBox.Add(CustomEvent.DUNGEON_MISSILE_DESTROY, OnDungeonMissileDestroy);
            EventBox.Add(CustomEvent.DUNGEON_SKILL_PAUSE, OnDungeonSkillPause);
            EventBox.Add(CustomEvent.DUNGEON_SKILL_RESUME, OnDungeonSkillResume);
        }

        public override void Release()
        {
            base.Release();

            EventBox.RemoveAll(this);
        }

        public override void EnterScene()
        {
            TransitionEngine.onTransitionComplete += OnTransitionComplete;
            TransitionEngine.onScreenObscured += OnScreenObscured;

            UserData user = UserManager.GetInstance().user;

            DisassemblygirlDungeonConfig config = ConfigMgr.GetInstance().DisassemblygirlDungeon.GetConfigById(currentDungeonIndex);
            dungeon = DungeonData.FromConfig(config);

            CreateMap(dungeon.resourceID);

            girlEntity = CreateGirlEntity(user.girl);
            girlEntity.Play(AnimationDefs.Idle.ToString().ToLower());

            petEntity = CreatePetEntity(user.GetActivePet());

            if (petEntity != null)
            {
                petEntity.Flip();
            }          

            LayerManager.GetInstance().AddPopUpView<DungeonWindow>();
            LayerManager.GetInstance().AddPopUpView<DungeonReadyBar>();         
        }

        public override void EnterSceneComplete()
        {
            EventBox.Send(CustomEvent.ENTER_SCENE_COMPLETE);

            SoundManager.GetInstance().PlayBackgroundMusic(AudioRepository.LoadBackgroundAudio(dungeon.audioID), 1.0f);
        }

        public override void ExitScene()
        {
            TransitionEngine.onTransitionComplete -= OnTransitionComplete;
            TransitionEngine.onScreenObscured -= OnScreenObscured;

            if (mapWrapper != null)
            {
                Destroy(mapWrapper.map.gameObject);
            }

            while (monsters.Count > 0)
            {
                monsters[0].Dispose();
            }

            monsters.Clear();

            while (missiles.Count > 0)
            {
                missiles[0].Dispose();
            }

            missiles.Clear();

            if (girlEntity != null)
            {
                girlEntity.Dispose();
            }

            if (petEntity != null)
            {
                petEntity.Dispose();
            }

            if (battleTask != null)
            {
                battleTask.Stop();
                battleTask = null;
            }      

            dungeon = null;

            Resources.UnloadUnusedAssets();

            CameraManager.GetInstance().closeCamera(CameraType.Skill);
        }

        public void StartBattle()
        {
            battleStatus = BattleStatus.Start;

            monsters = new List<MonsterEntity2D>();
            missiles = new List<AbstractMissileUI>();
     
            if (battleTask != null)
            {
                battleTask.Stop();
                battleTask = null;
            }

            if (dungeon.ActiveMonsterGroup())
            {
                battleTask = new Task(BattleTaskCoroutine());
            }
        }

        private IEnumerator BattleTaskCoroutine()
        {
            while (battleTask != null && battleTask.Running)
            {
                if (dungeon.currentGroup.HasMonsters())
                {
                    CreateAllMonsterGroup();

                    yield return new WaitForSeconds(dungeon.currentGroup.jointInterval);
                }
                else
                {
                    battleTask.Stop();
                    battleTask = null;
                }
            }
        }

        public void EndBattle(BattleResult result)
        {
            battleStatus = BattleStatus.Finish;

            if (battleTask != null)
            {
                battleTask.Stop();
                battleTask = null;
            }

            if (result == BattleResult.Win)
            {
                while (monsters.Count > 0)
                {
                    monsters[0].Dispose();
                }

                monsters.Clear();

                while (missiles.Count > 0)
                {
                    missiles[0].Dispose();
                }

                missiles.Clear();

                EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.DungeonClear, dungeon.id));

                dungeon = null;
            }

            LayerManager.GetInstance().AddPopUpView<DungeonResultWindow>(result);
        }
        
        private GirlEntity2D CreateGirlEntity(GirlData girlData)
        {
            GirlEntity2D entity = ResourceUtils.GetComponent<GirlEntity2D>(GlobalDefinitions.RESOURCE_PATH_GIRL + girlData.resourceID);
            entity.BindData(girlData);

            LayerManager.GetInstance().AddObject(entity.transform);
            entity.transform.position = mapWrapper.girlSpawnPoint.Value;
  
            return entity;
        }

        private PetEntity2D CreatePetEntity(PetData petData)
        {
            if (petData == null)
            {
                return null;
            }

            PetEntity2D entity = ResourceUtils.GetComponent<PetEntity2D>(GlobalDefinitions.RESOURCE_PATH_PET + petData.resourceID);
            entity.BindData(petData);

            LayerManager.GetInstance().AddObject(entity.transform);
            entity.transform.position = mapWrapper.petSpwanPoint.Value;

            return entity;
        }
        private void CreateMap(string resourceID)
        {
            mapWrapper = new MapWrapper(ResourceUtils.GetComponent<tk2dTileMap>(GlobalDefinitions.RESOURCE_PATH_MAP + resourceID), MapType.Dungeon, dungeon.directorID);

            LayerManager.GetInstance().AddObject(mapWrapper.map.transform);
            LayerManager.GetInstance().AddObject(mapWrapper.map.renderData.transform);
        }

        private void CreateAllMonsterGroup()
        {
            IDictionary<SpwanPointIds, IList<MonsterData>> allJoins = dungeon.currentGroup.GetAllJoinMonsters();

            if (dungeon.currentGroup.isBoss)
            {
                if (dungeon.currentGroup.boss != null)
                {
                    CreateMonsterEntity(SpwanPointIds.Boss, dungeon.currentGroup.boss);

                    dungeon.currentGroup.boss = null;
                }
            }

            foreach (KeyValuePair<SpwanPointIds, IList<MonsterData>> joins in allJoins)
            {
                if (joins.Value.Count != 0)
                {
                    CreateMonsterGroup(joins.Key, joins.Value);
                }
            }
        }

        private void CreateMonsterGroup(SpwanPointIds spwanID, IList<MonsterData> group)
        {
            if (group == null || group.Count == 0)
            {
                return;
            }

            foreach (MonsterData monster in group)
            {
                CreateMonsterEntity(spwanID, monster);             
            }
        }

        public MonsterEntity2D SearchMonsterAtRandom()
        {
            if (monsters != null && monsters.Count != 0)
            {
                return monsters[UnityEngine.Random.Range(0, monsters.Count)];
            }
            else
            {
                return null;
            }   
        }

        private MonsterEntity2D CreateMonsterEntity(SpwanPointIds pointID, MonsterData monsterData)
        {
            SpwanPoint2D point = null;
            if (monsterData.isBoss)
            {
                point  = mapWrapper.bossSpawnPoints[UnityEngine.Random.Range(0, mapWrapper.bossSpawnPoints.Count)];
            }
            else
            {
                IList<SpwanPoint2D> points = null;

                mapWrapper.monsterSpawnPoints.TryGetValue(pointID, out points);

                if (points.Count != 0)
                {
                    point = points[UnityEngine.Random.Range(0, points.Count)];
                }            
            }

            if (point == null) { return null;}
      
            MonsterEntity2D entity = ResourceUtils.GetComponent<MonsterEntity2D>(GlobalDefinitions.RESOURCE_PATH_MONSTER + monsterData.resourceID);
            entity.BindData(monsterData);
            entity.OnDispose = () => { monsters.Remove(entity); };

            LayerManager.GetInstance().AddObject(entity.transform);
            monsters.Add(entity);

            entity.transform.position += point.transform.position;
            entity.direction = point.direction;

            if (point.direction == Vector2.right)
            {
                entity.Flip();
            }

            return entity;
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {
         
        }

        void OnScreenObscured()
        {
            ExitScene();
        }

        void OnTransitionComplete()
        {
            EnterSceneComplete();          
        }

        void Update()
        {
            if (Input.GetButton("Fire1") && battleStatus == BattleStatus.Start && SystemManager.GetInstance().sceneTouchEnabled)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (girlEntity != null && !UICamera.isOverUI)
                {
                    girlEntity.ApplyWeapon(position);
                }
            }
          
        }

        private void CheckBattleResult()
        {
            if (monsters.Count == 0 && !dungeon.currentGroup.HasMonsters() && missiles.Count == 0)
            {
                if (dungeon.ActiveMonsterGroup())
                {
                    if (dungeon.currentGroup.isBoss)
                    {
                        if (battleTask != null)
                        {
                            battleTask.Stop();
                            battleTask = null;
                        }

                        Action BossGroup = () =>
                        {
                            battleTask = new Task(BattleTaskCoroutine());
                        };

                        CameraManager.GetInstance().closeCamera(CameraType.Skill);

                        LayerManager.GetInstance().AddPopUpView<DungeonWarningBar>(BossGroup);
                    }
                    else
                    {
                        if (battleTask != null)
                        {
                            battleTask.Stop();
                            battleTask = null;
                        }

                        Action NextMonsterGroup = () =>
                        {
                            battleTask = new Task(BattleTaskCoroutine());
                        };

                        CameraManager.GetInstance().closeCamera(CameraType.Skill);
                        LayerManager.GetInstance().AddPopUpView<DungeonNextBar>(NextMonsterGroup);
                    }
                }
                else
                {
                    EndBattle(BattleResult.Win);
                }
            }
        }

        private void OnDungeonMonsterDamage(object data)
        {
            object[] datas = data as object[];

            MonsterEntity2D entity = datas[0] as MonsterEntity2D;
            ValidatePayload payload = datas[1] as ValidatePayload;

            DamagePopUpUI view = LayerManager.GetInstance().AddPopUpView<DamagePopUpUI>();
            view.Initialize(payload);
            
            view.transform.position = entity.GetMountPoint(ComponentDefs.HUD).transform.position;

            float currentHP = entity.data.attributeBox.GetAttribute(AttributeKeys.HP);
            float maxHP = entity.data.attributeBox.GetAttribute(AttributeKeys.MaxHP);
            float percent = currentHP / maxHP;

            entity.UpdateHUD();

            entity.componentsHolder.ValidateAndUpdate(percent);
            
            foreach (EntityComponent component in entity.componentsHolder.components)
            {
                component.Blink();       
            }

            if (entity.data.attributeBox.GetAttribute(AttributeKeys.HP) <= 0)
            {
                EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.DungeonKillMonster, 1));

                entity.machine.ChangeState(StateTypes.Dead.ToString(), payload);

                float value = entity.data.attributeBox.GetAttribute(AttributeKeys.RP);
                
                if (value != 0)
                {
                    UserManager.GetInstance().AddRP(value);
                }

                value = entity.data.attributeBox.GetAttribute(AttributeKeys.GP);

                if (value != 0)
                {
                    UserManager.GetInstance().AddGP(value);
                }

                if (monsters.Contains(entity))
                {
                    monsters.Remove(entity);
                }

                if (entity is BossEntity2D)
                {
                    EndBattle(BattleResult.Win);
                }
                else
                {
                    if (monsters.Count == 0)
                    {
                        if (dungeon.currentGroup != null && dungeon.currentGroup.HasMonsters())
                        {
                            CreateAllMonsterGroup();
                        }
                        else
                        {
                            CheckBattleResult();
                        }
                    }              
                }
     
            }
        }

        private void OnDungeonGirlDead(object data)
        {
            EndBattle(BattleResult.Lose);
        }

        private void OnDungeonSkillPause(object data)
        {
            CameraManager.GetInstance().openCamera(CameraType.Skill);
            LayerManager.GetInstance().AddPopUpView<DungeonMaskWindow>();
            LayerMaskDefines.SKILL.ToLayer(girlEntity.gameObject);
            foreach (MonsterEntity2D entity in monsters)
            {
                entity.enabled = false;
                entity.Pause();
            }

            foreach (AbstractMissileUI missileUI in missiles)
            {
                missileUI.Pause();
            }

            if (petEntity != null)
            {
                petEntity.enabled = false;
                petEntity.Pause();
            }

            if (battleTask != null)
            {
                battleTask.Pause();
            }                      
        }

        private void OnDungeonSkillResume(object data)
        {
            CameraManager.GetInstance().closeCamera(CameraType.Skill);
            LayerManager.GetInstance().RemovePopUpView<DungeonMaskWindow>();
            LayerMaskDefines.GIRL.ToLayer(girlEntity.gameObject);
            foreach (MonsterEntity2D entity in monsters)
            {
                entity.enabled = true;
                entity.Resume();
            }

            foreach (AbstractMissileUI missileUI in missiles)
            {            
                missileUI.Resume();              
            }

            if (petEntity != null)
            {
                petEntity.enabled = true;
                petEntity.Resume();
            }

            if (battleTask != null)
            {
                battleTask.Unpause();
            }
        }

        private void OnDungeonMissileDestroy(object data)
        {
            if (dungeon != null)
            {
                CheckBattleResult();
            }      
        }

        private void OnDungeonMonsterTransfer(object data)
        {
            MonsterEntity2D entity = data as MonsterEntity2D;

            if (monsters.Contains(entity))
            {
                Vector3 thePosition = entity.transform.position;
                thePosition.x = mapWrapper.monsterOutPoint.Value.x;
                thePosition.y = mapWrapper.monsterOutPoint.Value.y;

                entity.transform.position = thePosition;

                entity.machine.ChangeState(StateTypes.Idle.ToString());

                entity.FadeIn();
            }
        }
    }
}