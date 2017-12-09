using UnityEngine;
using Manager;
using View.Weapon;
using View.Pet;
using View.Gold;
using View.Shop;
using Utils;
using Prototype;
using View.Dungeon;
using View.Common;
using View.Trophy;
using System.Collections.Generic;

namespace View.Home
{
    [View(prefabPath ="UI/Home/HomeWindow", isSingleton =true, layer =ViewLayerTypes.Main)]
    public class HomeWindow : BaseView
    {
        public UIButton pauseBtn = null;

        public UIButton weaponSlot1Btn = null;

        public UIButton weaponSlot2Btn = null;

        public UIButton weaponSlot3Btn = null;

        public UIButton skillSlot1Btn = null;

        public UIButton skillSlot2Btn = null;

        public UIButton skillSlot3Btn = null;

        public UISprite active1Img = null;

        public UISprite active2Img = null;

        public UISprite active3Img = null;

        public UISprite activeSkill1Img = null;

        public UISprite activeSkill2Img = null;

        public UISprite activeSkill3Img = null;

        public UISprite weaponIconImgI = null;

        public UISprite weaponIconImgII = null;

        public UISprite weaponIconImgIII = null;

        public UISprite skillIconImgI = null;

        public UISprite skillIconImgII = null;

        public UISprite skillIconImgIII = null;

        public UIButton homeBtn = null;

        public UIButton shopBtn = null;

        public UIButton petBtn = null;

        public UIButton weaponBtn = null;

        public UIButton dungeonBtn = null;

        public UIButton goldBtn = null;

        public UIButton trophyBtn = null;

        public UILabel gpLab = null;

        public UILabel rpLab = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(pauseBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot1Btn.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot2Btn.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot3Btn.gameObject).onClick += OnClick;
            UIEventListener.Get(skillSlot1Btn.gameObject).onClick += OnClick;
            UIEventListener.Get(skillSlot2Btn.gameObject).onClick += OnClick;
            UIEventListener.Get(skillSlot3Btn.gameObject).onClick += OnClick;

            UIEventListener.Get(homeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(petBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(dungeonBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(shopBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(goldBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(trophyBtn.gameObject).onClick += OnClick;

            EventBox.Add(CustomEvent.HOME_UPDATE_WEAPON_SLOT, OnHomeUpdateWeaponSlot);
            EventBox.Add(CustomEvent.HOME_SHOW_FUNCTION, OnHomeShowFunction);
            EventBox.Add(CustomEvent.HOME_SHOW_FUNCTIONS, OnHOME_SHOW_FUNCTIONS);
            EventBox.Add(CustomEvent.WEAPON_LEVEL_UP, OnWeaponLevelUp);
            EventBox.Add(CustomEvent.USER_APPLY_GP, OnUserApplyGP);
            EventBox.Add(CustomEvent.USER_APPLY_RP, OnUserApplyRP);

            Refresh();
        }

        private void ConfigFrontSight()
        {
            FrontSightUI ui = ResourceUtils.GetComponent<FrontSightUI>(FrontSightUI.PREFAB_PATH);
            ui.transform.SetParent(transform);
            ui.transform.localScale = Vector3.one;
        }

        private void Refresh()
        {
            gpLab.text = UserManager.GetInstance().GetCurrentGP().ToString();
            rpLab.text = UserManager.GetInstance().GetCurrentRP().ToString();

            ConfigWeapons();
        }

        private void ConfigWeapons()
        {
            UserData user = UserManager.GetInstance().user;

            WeaponData weaponI = user.weaponSlotI;

            if (weaponI != null)
            {
                if (user.activeSlotIndex == SlotIndex.I)
                {
                    active1Img.gameObject.SetActive(true);
                }

                weaponIconImgI.spriteName = weaponI.iconID;
                weaponIconImgI.gameObject.SetActive(true);

                if (weaponI.skillData != null)
                {
                    skillIconImgI.spriteName = weaponI.skillData.iconID;
                    skillIconImgI.gameObject.SetActive(true);
                    activeSkill1Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgI.gameObject.SetActive(false);
                    activeSkill1Img.gameObject.SetActive(false);
                }
            }

            WeaponData weaponII = user.weaponSlotII;

            if (weaponII != null)
            {
                if (user.activeSlotIndex == SlotIndex.II)
                {
                    active2Img.gameObject.SetActive(true);
                }

                weaponIconImgII.spriteName = weaponII.iconID;
                weaponIconImgII.gameObject.SetActive(true);

                if (weaponII.skillData != null)
                {
                    skillIconImgII.spriteName = weaponII.skillData.iconID;
                    skillIconImgII.gameObject.SetActive(true);
                    activeSkill2Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgII.gameObject.SetActive(false);
                    activeSkill2Img.gameObject.SetActive(false);
                }
            }

            WeaponData weaponIII = user.weaponSlotIII;

            if (weaponIII != null)
            {
                if (user.activeSlotIndex == SlotIndex.III)
                {
                    active3Img.gameObject.SetActive(true);
                }

                weaponIconImgIII.spriteName = weaponIII.iconID;
                weaponIconImgIII.gameObject.SetActive(true);

                if (weaponIII.skillData != null)
                {
                    skillIconImgIII.spriteName = weaponIII.skillData.iconID;
                    skillIconImgIII.gameObject.SetActive(true);
                    activeSkill3Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgIII.gameObject.SetActive(false);
                    activeSkill3Img.gameObject.SetActive(false);
                }
            }
        }

        public override void Dispose()
        {
            EventBox.RemoveAll(this);
        }

        private void ConfigWeaponSlotI()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotI != null && user.activeSlotIndex != SlotIndex.I)
            {
                active1Img.gameObject.SetActive(true);
                active2Img.gameObject.SetActive(false);
                active3Img.gameObject.SetActive(false);

                user.activeSlotIndex = SlotIndex.I;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotI);
            }
        }

        private void ConfigWeaponSlotII()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotII != null && user.activeSlotIndex != SlotIndex.II)
            {
                active1Img.gameObject.SetActive(false);
                active2Img.gameObject.SetActive(true);
                active3Img.gameObject.SetActive(false);

                user.activeSlotIndex = SlotIndex.II;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotII);
            }
        }

        private void ConfigWeaponSlotIII()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotIII != null && user.activeSlotIndex != SlotIndex.III)
            {
                active1Img.gameObject.SetActive(false);
                active2Img.gameObject.SetActive(false);
                active3Img.gameObject.SetActive(true);

                user.activeSlotIndex = SlotIndex.III;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotIII);
            }
        }

        private void OnClick(GameObject go)
        {
            if (go == pauseBtn.gameObject)
            {
                SystemManager.GetInstance().Resume();

                LayerManager.GetInstance().AddPopUpView<HomePauseAlertWindow>();
            }
            else if (weaponSlot1Btn.gameObject == go)
            {
                ConfigWeaponSlotI();
            }
            else if (weaponSlot2Btn.gameObject == go)
            {
                ConfigWeaponSlotII();
            }
            else if (weaponSlot3Btn.gameObject == go)
            {
                ConfigWeaponSlotIII();
            }
            else if (go == weaponBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<WeaponWindow>();
            }
            else if (go == dungeonBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<DungeonSelectWindow>();
            }
            else if (go == petBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<PetWindow>();
            }
            else if (go == shopBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<ShopWindow>();
            }
            else if (go == goldBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<GoldWindow>();
            }
            else if (go == trophyBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<TrophyWindow>();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ConfigWeaponSlotI();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ConfigWeaponSlotII();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ConfigWeaponSlotIII();
            }
        }

        private void OnUserApplyGP(object data)
        {
            gpLab.text = UserManager.GetInstance().GetCurrentGP().ToString();
        }

        private void OnUserApplyRP(object data)
        {
            rpLab.text = UserManager.GetInstance().GetCurrentRP().ToString();
        }

        private void OnWeaponLevelUp(object data)
        {
            UserData user = UserManager.GetInstance().user;

            WeaponData weaponData = data as WeaponData;

            if (user.weaponSlotI != null && (user.weaponSlotI.id == weaponData.id))
            {
                weaponIconImgI.spriteName = weaponData.iconID;
                weaponIconImgI.gameObject.SetActive(true);

                if (weaponData.skillData != null)
                {
                    skillIconImgI.spriteName = weaponData.skillData.iconID;
                    skillIconImgI.gameObject.SetActive(true);
                    activeSkill1Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgI.gameObject.SetActive(false);
                    activeSkill1Img.gameObject.SetActive(false);
                }
            }
            else if (user.weaponSlotII != null && (user.weaponSlotII.id == weaponData.id))
            {
                weaponIconImgII.spriteName = weaponData.iconID;
                weaponIconImgII.gameObject.SetActive(true);

                if (weaponData.skillData != null)
                {
                    skillIconImgII.spriteName = weaponData.skillData.iconID;
                    skillIconImgII.gameObject.SetActive(true);
                    activeSkill2Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgII.gameObject.SetActive(false);
                    activeSkill2Img.gameObject.SetActive(false);
                }
            }
            else if (user.weaponSlotIII != null && (user.weaponSlotIII.id == weaponData.id))
            {
                weaponIconImgIII.spriteName = weaponData.iconID;
                weaponIconImgIII.gameObject.SetActive(true);

                if (weaponData.skillData != null)
                {
                    skillIconImgIII.spriteName = weaponData.skillData.iconID;
                    skillIconImgIII.gameObject.SetActive(true);
                    activeSkill3Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgIII.gameObject.SetActive(false);
                    activeSkill3Img.gameObject.SetActive(false);
                }
            }
        }

        private void OnHOME_SHOW_FUNCTIONS(object data)
        {
            IList<FunctionData> functionDatas = UserManager.GetInstance().GetFunctions();

            for (int index = 0; index < functionDatas.Count; index++)
            {
                OnHomeShowFunction(functionDatas[index]);
            }
        }

        private void OnHomeShowFunction(object data)
        {
            FunctionData functionData = data as FunctionData;

            UIButton functionBtn = null;
            if (functionData.id == (int)FunctionType.Weapon)
            {
                functionBtn = weaponBtn;
            }
            else if (functionData.id == (int)FunctionType.Dungeon)
            {
                functionBtn = dungeonBtn;
            }

            if (functionBtn != null)
            {
                functionBtn.gameObject.SetActive(true);
                TweenScale tween = TweenScale.Begin(functionBtn.gameObject, 0.5f, Vector3.one);
                tween.from = Vector3.zero;
            }
        }

        private void OnHomeUpdateWeaponSlot(object data)
        {
            UserData user = UserManager.GetInstance().user;
            WeaponSlot slot = data as WeaponSlot;

            if (slot.index == SlotIndex.I)
            {
                weaponIconImgI.spriteName = slot.weapon.iconID;
                weaponIconImgI.gameObject.SetActive(true);

                if (slot.weapon.skillData != null)
                {
                    skillIconImgI.spriteName = slot.weapon.skillData.iconID;
                    skillIconImgI.gameObject.SetActive(true);
                    activeSkill1Img.gameObject.SetActive(true);
                }
                else
                {                
                    skillIconImgI.gameObject.SetActive(false);
                    activeSkill1Img.gameObject.SetActive(false);
                }

                if (user.activeSlotIndex == SlotIndex.None)
                {
                    active1Img.gameObject.SetActive(true);
                    user.activeSlotIndex = SlotIndex.I;
                }

                if (user.activeSlotIndex == SlotIndex.I)
                {
                    EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotI);
                }
            }
            else if (slot.index == SlotIndex.II)
            {
                weaponIconImgII.spriteName = slot.weapon.iconID;
                weaponIconImgII.gameObject.SetActive(true);

                if (slot.weapon.skillData != null)
                {
                    skillIconImgII.spriteName = slot.weapon.skillData.iconID;
                    skillIconImgII.gameObject.SetActive(true);
                    activeSkill2Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgII.gameObject.SetActive(false);
                    activeSkill2Img.gameObject.SetActive(false);
                }

                if (user.activeSlotIndex == SlotIndex.None)
                {
                    active2Img.gameObject.SetActive(true);
                    user.activeSlotIndex = SlotIndex.II;
                }

                if (user.activeSlotIndex == SlotIndex.II)
                {
                    EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotII);
                }
            }
            else
            {
                weaponIconImgIII.spriteName = slot.weapon.iconID;
                weaponIconImgIII.gameObject.SetActive(true);

                if (slot.weapon.skillData != null)
                {
                    skillIconImgIII.spriteName = slot.weapon.skillData.iconID;
                    skillIconImgIII.gameObject.SetActive(true);
                    activeSkill3Img.gameObject.SetActive(true);
                }
                else
                {
                    skillIconImgIII.gameObject.SetActive(false);
                    activeSkill3Img.gameObject.SetActive(false);
                }

                if (user.activeSlotIndex == SlotIndex.None)
                {
                    active3Img.gameObject.SetActive(true);
                    user.activeSlotIndex = SlotIndex.III;
                }

                if (user.activeSlotIndex == SlotIndex.III)
                {
                    EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotIII);
                }
            }           
        }
    }
}