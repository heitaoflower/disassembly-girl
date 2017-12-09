using UnityEngine;
using Prototype;
namespace View.Trophy
{
    public class TrophyItemRenderer : MonoBehaviour
    {
        public static readonly string PREFAB_PATH = "UI/Trophy/TrophyItemRenderer";

        public UISprite iconImg = null;

        [HideInInspector]
        public TrophyData data = null;

        public void Initialize(TrophyData data)
        {
            this.data = data;

            Refresh();
        }

        private void Refresh()
        {
            if (data.isCompleted)
            {
                iconImg.gameObject.SetActive(true);
                iconImg.spriteName = data.iconID;
            }

        }

        void Awake()
        {
        }

        void Start()
        {
            transform.localPosition = Vector2.zero;
            transform.localScale = Vector2.one;
        }
    }
}