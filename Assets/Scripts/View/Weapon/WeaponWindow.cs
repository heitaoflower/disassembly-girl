using UnityEngine;
using Manager;
using Prototype;
using Utils;
using System.Collections.Generic;

namespace View.Weapon
{
    [View(prefabPath ="UI/Weapon/WeaponWindow", isSingleton =true, layer =ViewLayerTypes.Window)]
    public class WeaponWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIGrid weaponGrid = null;

        private IList<WeaponItemRenderer> weaponItems = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;

            EventBox.Add(CustomEvent.WEAPON_LEVEL_UP, OnWeaponLevelUp);
        }

        void Awake()
        {
            ShowWeapons();
        }

        public override void Dispose()
        {
            weaponItems.Clear();

            EventBox.RemoveAll(this);
        }

        private void ShowWeapons()
        {
            weaponItems = new List<WeaponItemRenderer>();

            GirlData girl = UserManager.GetInstance().user.girl;

            foreach (WeaponData weapon in girl.weapons)
            {
                WeaponItemRenderer itemRenderer = ResourceUtils.GetComponent<WeaponItemRenderer>(WeaponItemRenderer.PREFAB_PATH);
                itemRenderer.Initialize(weapon);
                itemRenderer.transform.SetParent(weaponGrid.transform);
                weaponItems.Add(itemRenderer);
            }

            weaponGrid.Reposition();
        }           

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
        }

        private void OnWeaponLevelUp(object data)
        {
            WeaponData weaponData = data as WeaponData;

            for (int index = 0; index < weaponItems.Count; index++)
            {
                if (weaponItems[index].data.id == weaponData.id)
                {
                    weaponItems[index].Refresh();
                    break;
                }
            }
        }
    }
}