using UnityEngine;
using Prototype;
using Utils;
using Manager;
using System.Collections.Generic;
namespace View.Pet
{
    [View(prefabPath = "UI/Pet/PetSubWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class PetSubWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIButton selectBtn = null;

        public UIButton levelUpBtn = null;

        public UISprite levelMaxImg = null;

        public UISprite activeImg = null;

        public UILabel rpLab = null;

        public UILabel gpLab = null;

        public UISprite iconImg = null;

        private PetData data = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            this.data = data as PetData;

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(selectBtn.gameObject).onClick += OnClick;
            UIEventListener.Get(levelUpBtn.gameObject).onClick += OnClick;

            Refresh();
        }

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == selectBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_PRESS.AsAudioClip());

                EventBox.Send(CustomEvent.USER_APPLY_PET, data);
                LayerManager.GetInstance().RemovePopUpView(this);
            }
            else if (go == levelUpBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_PRESS.AsAudioClip());

                int costRP = ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).RP;
                int costGP = ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).GP;

                if (UserManager.GetInstance().GetCurrentRP() >= costRP && UserManager.GetInstance().GetCurrentGP() >= costGP)
                {
                    bool isActivePetData = data.id == UserManager.GetInstance().user.activePetID;

                    if (data.LevelUP())
                    {
                        UserManager.GetInstance().CostRP(costRP);
                        UserManager.GetInstance().CostGP(costGP);

                        if (isActivePetData)
                        {
                            UserManager.GetInstance().user.activePetID = data.id;
                        }

                        Refresh();

                        EventBox.Send(CustomEvent.PET_LEVEL_UP, data);

                        if (data.LevelMax())
                        {
                            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.PetLevelMax, 1));
                        }

                    }
                }
            }
        }

        private void Refresh()
        {
            this.iconImg.spriteName = string.Format("{0}_{1}_01", data.iconID, data.level);
            this.iconImg.GetComponent<UISpriteAnimation>().namePrefix = string.Format("{0}_{1}", data.iconID, data.level);

            activeImg.gameObject.SetActive(data.id == UserManager.GetInstance().user.activePetID);

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

                rpLab.text = ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).RP.ToString();
                gpLab.text = ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).GP.ToString();

                gpLab.gameObject.SetActive(true);
                rpLab.gameObject.SetActive(true);

                if (UserManager.GetInstance().GetCurrentRP() < ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).RP)
                {
                    this.rpLab.color = Color.red;
                }
                else
                {
                    this.rpLab.color = Color.white;
                }

                if (UserManager.GetInstance().GetCurrentGP() < ConfigMgr.GetInstance().DisassemblygirlPet.GetConfigById(data.id + 1).GP)
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