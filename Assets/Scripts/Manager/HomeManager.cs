using Utils;
using UnityEngine;
using View.Home;
using Orange.TransitionKit;
using Entity;
using Prototype;
using Map;
using Guide;


namespace Manager
{
    public class HomeManager : SceneManager<HomeManager>
    {
        private GirlEntity2D girlEntity = null;

        private PetEntity2D petEntity = null;

        private MapWrapper mapWrapper = null;

        public override void Initialize()
        {
            base.Initialize();

            AddEventLisenters();
        }

        private void AddEventLisenters()
        {
            EventBox.Add(CustomEvent.USER_APPLY_PET, OnUserApplyPet);
            EventBox.Add(CustomEvent.PET_LEVEL_UP, OnPetLevelUp);
        }

        public override void Release()
        {
            base.Release();

            EventBox.RemoveAll(this);
        }

        public override void EnterScene()
        {
            CameraManager.GetInstance().openCamera(CameraType.Guide);

            UserData user = UserManager.GetInstance().user;

            TransitionEngine.onTransitionComplete += OnTransitionComplete;
            TransitionEngine.onScreenObscured += OnScreenObscured;

            CreateMap();

            girlEntity = CreateGirlEntity(user.girl);
            girlEntity.Flip();

            petEntity = CreatePetEntity(user.GetActivePet());

            LayerManager.GetInstance().AddPopUpView<HomeWindow>();

            SoundManager.GetInstance().PlayBackgroundMusic(AudioRepository.BG_HOME.AsAudioClip(), 1.0f);

            EventBox.Send(CustomEvent.HOME_SHOW_FUNCTIONS);

            GuideManager.GetInstance().Trigger(GuideScriptID.G01);
        }

        public override void EnterSceneComplete()
        {
       
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

        public override void ExitScene()
        {
            TransitionEngine.onTransitionComplete -= OnTransitionComplete;
            TransitionEngine.onScreenObscured -= OnScreenObscured;

            if (mapWrapper != null)
            {
                Destroy(mapWrapper.map.gameObject);
            }

            if (girlEntity != null)
            {
                girlEntity.Dispose();
            }

            if (petEntity != null)
            {
                petEntity.Dispose();
            }

            CameraManager.GetInstance().closeCamera(CameraType.Guide);
            SoundManager.GetInstance().backgroundSound.FadeOutAndStop(1.0f);
        }

        private void CreateMap()
        {
            mapWrapper = new MapWrapper(ResourceUtils.GetComponent<tk2dTileMap>(GlobalDefinitions.RESOURCE_PATH_MAP + "Home/01TileMap"), MapType.Home, 100);

            LayerManager.GetInstance().AddObject(mapWrapper.map.transform);
            LayerManager.GetInstance().AddObject(mapWrapper.map.renderData.transform);
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
            if (Input.GetButton("Fire1") && SystemManager.GetInstance().sceneTouchEnabled)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (girlEntity != null && !UICamera.isOverUI)
                {
                    if (position.x > girlEntity.transform.position.x)
                    {
                        if (girlEntity.componentsHolder.transform.localScale.x > 0f)
                        {
                            girlEntity.Flip();
                        }
                    }
                    else
                    {
                        if (girlEntity.componentsHolder.transform.localScale.x < 0f)
                        {
                            girlEntity.Flip();
                        }
                    }
                }
            }
          
        }

        private void OnUserApplyPet(object data)
        {
            PetData petData = data as PetData;

            if (petEntity != null)
            {
                petEntity.Dispose();
                petEntity = null;
            }

            petEntity = CreatePetEntity(petData);
        }

        private void OnPetLevelUp(object data)
        {
            PetData petData = data as PetData;
            
            if (petEntity != null)
            {
                if (UserManager.GetInstance().user.activePetID == petData.id)
                {
                    petEntity.Dispose();
                    petEntity = null;

                    petEntity = CreatePetEntity(petData);
                }                
            }

        }
    }
}