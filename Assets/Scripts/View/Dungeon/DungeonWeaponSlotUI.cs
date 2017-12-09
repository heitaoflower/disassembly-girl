using Prototype;

namespace View.Dungeon
{
    public class DungeonWeaponSlotUI : BaseView {

        public UISprite iconImg = null;

        public UISprite activeImg = null;

        public void Active()
        {
            activeImg.gameObject.SetActive(true);
        }

        public void Deactive()
        {
            activeImg.gameObject.SetActive(false);
        }

        public override void Show(object data = null)
        {
            WeaponData weaponData = data as WeaponData;

            iconImg.spriteName = weaponData.iconID;
            iconImg.gameObject.SetActive(true);
        }
    }
}