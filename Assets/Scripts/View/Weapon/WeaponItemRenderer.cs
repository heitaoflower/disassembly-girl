using UnityEngine;
using Utils;
using Manager;

namespace View.Weapon
{
    public class WeaponItemRenderer : MonoBehaviour
    {
        public static readonly string PREFAB_PATH = "UI/Weapon/WeaponItemRenderer";

        public UIButton itemBtn = null;

        public UISprite iconImg = null;

        public UISprite levelImg = null;

        [HideInInspector]
        public Prototype.WeaponData data = null;

        public void Initialize(Prototype.WeaponData data)
        {
            this.data = data;

            this.Refresh();
        }

        void Awake()
        {
            UIEventListener.Get(itemBtn.gameObject).onClick += delegate(GameObject go)
            {
                LayerManager.GetInstance().AddPopUpView<WeaponSubWindow>(data);
            };
        }

        void Start()
        {
            transform.localPosition = Vector2.zero;
            transform.localScale = Vector2.one;
        }

        public void Refresh()
        {
            this.levelImg.spriteName = string.Format("IMAGE_WEAPON_LEVEL_{0}", data.level);

            this.iconImg.spriteName = data.iconID;
        }
      
    }
}