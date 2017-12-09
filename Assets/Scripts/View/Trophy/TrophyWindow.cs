using UnityEngine;
using Manager;
using Prototype;
using Utils;
using System.Collections.Generic;
namespace View.Trophy
{
    [View(prefabPath = "UI/Trophy/TrophyWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class TrophyWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIGrid itemGrid = null;

        private IList<TrophyItemRenderer> trophyItems = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
        }

        public override void Hide()
        {
            base.Hide();

            if (trophyItems != null)
            {
                trophyItems.Clear();
            }          

            EventBox.RemoveAll(this);
        }

        void Awake()
        {
            ShowItems();
        }

        private void ShowItems()
        {
            trophyItems = new List<TrophyItemRenderer>();

            foreach (TrophyData data in UserManager.GetInstance().user.trophyDatas)
            {
                TrophyItemRenderer itemRenderer = ResourceUtils.GetComponent<TrophyItemRenderer>(TrophyItemRenderer.PREFAB_PATH);
                itemRenderer.Initialize(data);
                itemRenderer.transform.SetParent(itemGrid.transform);
                trophyItems.Add(itemRenderer);
            }

            itemGrid.Reposition();
        }

        private void OnClick(GameObject go)
        {
            if (go == closeBtn.gameObject)
            {
                SoundManager.GetInstance().PlayOneShot(AudioRepository.COMMON_BUTTON_CLOSE.AsAudioClip());

                LayerManager.GetInstance().RemovePopUpView(this);
            }
        }

    }
}