using UnityEngine;
using Manager;
using Utils;
using Prototype;
using Entity;

namespace View.Dungeon
{
    [View(prefabPath = "UI/Dungeon/DungeonWindow", isSingleton = true, layer = ViewLayerTypes.Main)]
    public class DungeonWindow : BaseView
    {
        public UIButton pauseBtn = null;

        public DungeonSkillSlotUI skillSlot1UI = null;

        public DungeonSkillSlotUI skillSlot2UI = null;

        public DungeonSkillSlotUI skillSlot3UI = null;

        public DungeonWeaponSlotUI weaponSlot1UI = null;

        public DungeonWeaponSlotUI weaponSlot2UI = null;

        public DungeonWeaponSlotUI weaponSlot3UI = null;

        public UILabel rpLab = null;

        public UILabel gpLab = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(pauseBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot1UI.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot2UI.gameObject).onClick += OnClick;
            UIEventListener.Get(weaponSlot3UI.gameObject).onClick += OnClick;

            EventBox.Add(CustomEvent.USER_APPLY_GP, OnUserApplyGP);
            EventBox.Add(CustomEvent.USER_APPLY_RP, OnUserApplyRP);

            Refresh();
        }

        public override void Dispose()
        {
            base.Dispose();

            EventBox.RemoveAll(this);
        }

        private void Refresh()
        {
            gpLab.text = UserManager.GetInstance().GetCurrentGP().ToString();
            rpLab.text = UserManager.GetInstance().GetCurrentRP().ToString();

            ConfigWeapons();
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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GirlEntity2D girlEntity = DungeonManager.GetInstance().girlEntity;

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (girlEntity != null && !UICamera.isOverUI)
                {
                    WeaponData weapon = UserManager.GetInstance().user.weaponSlotI;

                    if (weapon != null && weapon.skillData != null)
                    {
                        girlEntity.ApplySkill(position, weapon.skillData);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GirlEntity2D girlEntity = DungeonManager.GetInstance().girlEntity;

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (girlEntity != null && !UICamera.isOverUI)
                {
                    WeaponData weapon = UserManager.GetInstance().user.weaponSlotII;

                    if (weapon != null && weapon.skillData != null)
                    {
                        girlEntity.ApplySkill(position, weapon.skillData);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                GirlEntity2D girlEntity = DungeonManager.GetInstance().girlEntity;

                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (girlEntity != null && !UICamera.isOverUI)
                {
                    WeaponData weapon = UserManager.GetInstance().user.weaponSlotIII;

                    if (weapon != null && weapon.skillData != null)
                    {
                        girlEntity.ApplySkill(position, weapon.skillData);
                    }
                }
            }
        }

        private void ConfigWeapons()
        {
            WeaponData weaponI = UserManager.GetInstance().user.weaponSlotI;

            if (weaponI != null)
            {
                if (UserManager.GetInstance().user.activeSlotIndex == SlotIndex.I)
                {
                    weaponSlot1UI.Active();
                }

                weaponSlot1UI.Show(weaponI);

                if (weaponI.skillData != null)
                {
                    skillSlot1UI.Show(weaponI.skillData);
                }
                else
                {
                    skillSlot1UI.Hide();
                }
            }

            WeaponData weaponII = UserManager.GetInstance().user.weaponSlotII;

            if (weaponII != null)
            {
                if (UserManager.GetInstance().user.activeSlotIndex == SlotIndex.II)
                {
                    weaponSlot2UI.Active();
                }

                weaponSlot2UI.Show(weaponII);

                if (weaponII.skillData != null)
                {
                    skillSlot2UI.Show(weaponII.skillData);
                }
                else
                {
                    skillSlot2UI.Hide();
                }
            }

            WeaponData weaponIII = UserManager.GetInstance().user.weaponSlotIII;

            if (UserManager.GetInstance().user.weaponSlotIII != null)
            {
                if (UserManager.GetInstance().user.activeSlotIndex == SlotIndex.III)
                {
                    weaponSlot3UI.Active();
                }

                weaponSlot3UI.Show(weaponIII);

                if (weaponIII.skillData != null)
                {
                    skillSlot3UI.Show(weaponIII.skillData);
                }
                else
                {
                    skillSlot3UI.Hide();
                }
            }
        }

        private void ConfigWeaponSlotI()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotI != null && user.activeSlotIndex != SlotIndex.I)
            {
                weaponSlot1UI.Active();
                weaponSlot2UI.Deactive();
                weaponSlot3UI.Deactive();

                user.activeSlotIndex = SlotIndex.I;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotI);
            }
        }

        private void ConfigWeaponSlotII()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotII != null && user.activeSlotIndex != SlotIndex.II)
            {
                weaponSlot1UI.Deactive();
                weaponSlot2UI.Active();
                weaponSlot3UI.Deactive();

                user.activeSlotIndex = SlotIndex.II;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotII);
            }
        }

        private void ConfigWeaponSlotIII()
        {
            UserData user = UserManager.GetInstance().user;

            if (user.weaponSlotIII != null && user.activeSlotIndex != SlotIndex.III)
            {
                weaponSlot1UI.Deactive();
                weaponSlot2UI.Deactive();
                weaponSlot3UI.Active();

                user.activeSlotIndex = SlotIndex.III;

                EventBox.Send(CustomEvent.USER_APPLY_WEAPON, user.weaponSlotIII);
            }
        }

        private void OnClick(GameObject go)
        {
            if (go == pauseBtn.gameObject)
            {
                LayerManager.GetInstance().AddPopUpView<DungeonPauseAlertWindow>();
            }
            else if (go == weaponSlot1UI.gameObject)
            {
                ConfigWeaponSlotI();
            }
            else if (go == weaponSlot2UI.gameObject)
            {
                ConfigWeaponSlotII();
            }
            else if (go == weaponSlot3UI.gameObject)
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

    }
}
