using UnityEngine;
using Manager;
using Prototype;

namespace View.Pet
{
    public class PetItemRenderer : MonoBehaviour
    {
        public static readonly string PREFAB_PATH = "UI/Pet/PetItemRenderer";

        public UIButton itemBtn = null;

        public UISprite activeImg = null;

        public UISprite iconImg = null;

        [HideInInspector]
        public PetData data = null;

        public void Initialize(PetData data)
        {
            this.data = data;

            Refresh();
        }

        void Awake()
        {
            UIEventListener.Get(itemBtn.gameObject).onClick += delegate (GameObject go)
            {
                LayerManager.GetInstance().AddPopUpView<PetSubWindow>(data);
            };
        }

        void Start()
        {
            transform.localPosition = Vector2.zero;
            transform.localScale = Vector2.one;
        }

        public void Refresh()
        {
            this.iconImg.spriteName = string.Format("{0}_{1}_01", data.iconID, data.level);
            this.iconImg.GetComponent<UISpriteAnimation>().namePrefix = string.Format("{0}_{1}", data.iconID, data.level);

            PetData activePetData = UserManager.GetInstance().user.GetActivePet();
            if (activePetData != null && activePetData.id == data.id)
            {
                Active();
            }
            else
            {
                Deactive();
            }
        }

        public void Active()
        {
            this.activeImg.gameObject.SetActive(true);
        }

        public void Deactive()
        {
            this.activeImg.gameObject.SetActive(false);
        }
    }
}