using UnityEngine;
using Manager;
using Prototype;
using Utils;
using System.Collections.Generic;
namespace View.Weapon
{
    [View(prefabPath = "UI/Weapon/WeaponSubWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class WeaponSubWindow : BaseView
    {

        public UIButton closeBtn = null;

        public UISprite iconImg = null;

        public UISprite levelImg = null;

        public UIButton levelUpBtn = null;

        public UISprite levelMaxImg = null;

        public UILabel rpLab = null;

        public UILabel gpLab = null;

        public UIButton slotIBtn = null;

        public UIButton slotIIBtn = null;

        public UIButton slotIIIBtn = null;

        private WeaponData data = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            this.data = data as WeaponData;

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(slotIBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(slotIIBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(slotIIIBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(levelUpBtn.gameObject).onClick += OnClick;

            Refresh();
        }

        private void OnClick(GameObject go)
        {
            UserData user = UserManager.GetInstance().user;

            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == slotIBtn.gameObject)
            {
                user.weaponSlotI = data;
                EventBox.Send(CustomEvent.HOME_UPDATE_WEAPON_SLOT, new WeaponSlot(SlotIndex.I, data));
                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == slotIIBtn.gameObject)
            {
                user.weaponSlotII = data;
                EventBox.Send(CustomEvent.HOME_UPDATE_WEAPON_SLOT, new WeaponSlot(SlotIndex.II, data));
                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == slotIIIBtn.gameObject)
            {
                user.weaponSlotIII = data;
                EventBox.Send(CustomEvent.HOME_UPDATE_WEAPON_SLOT, new WeaponSlot(SlotIndex.III, data));
                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == levelUpBtn.gameObject)
            {
                int costRP = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).RP;
                int costGP = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).GP;

                if (UserManager.GetInstance().GetCurrentRP() >= costRP && UserManager.GetInstance().GetCurrentGP() >= costGP)
                {
                    if (data.LevelUP())
                    {
                        UserManager.GetInstance().CostRP(costRP);
                        UserManager.GetInstance().CostGP(costGP);

                        Refresh();

                        EventBox.Send(CustomEvent.WEAPON_LEVEL_UP, data);

                        if (data.LevelMax())
                        {
                            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.WeaponLevelMax, 1));
                        }                      
                    }

                }
            }
        }

        private void Refresh()
        {
            this.levelImg.spriteName = string.Format("IMAGE_WEAPON_LEVEL_{0}", data.level);

            this.iconImg.spriteName = data.iconID;

            if (data.LevelMax())
            {
                levelUpBtn.gameObject.SetActive(false);
                levelMaxImg.gameObject.SetActive(true);
                gpLab.gameObject.SetActive(false);
                rpLab.gameObject.SetActive(false);
            }
            else
            {
                levelMaxImg.gameObject.SetActive(false);
                levelUpBtn.gameObject.SetActive(true);

                rpLab.text = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).RP.ToString();
                gpLab.text = ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).GP.ToString();

                gpLab.gameObject.SetActive(true);
                rpLab.gameObject.SetActive(true);

                if (UserManager.GetInstance().GetCurrentRP() < ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).RP)
                {
                    this.rpLab.color = Color.red;
                }
                else
                {
                    this.rpLab.color = Color.white;
                }

                if (UserManager.GetInstance().GetCurrentGP() < ConfigMgr.GetInstance().DisassemblygirlWeapon.GetConfigById(data.id + 1).GP)
                {
                    this.gpLab.color = Color.red;
                }
                else
                {
                    this.gpLab.color = Color.white;
                }

                if (this.gpLab.color == Color.red || this.rpLab.color == Color.red)
                {
                    levelUpBtn.isEnabled = false;
                }
                else
                {
                    levelUpBtn.isEnabled = true;
                }
            }
        }
    }
}