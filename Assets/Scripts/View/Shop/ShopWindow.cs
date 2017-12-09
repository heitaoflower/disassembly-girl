using UnityEngine;
using Manager;
using Utils;
using System.Collections.Generic;
using Prototype;
using System;

namespace View.Shop
{
    [View(prefabPath = "UI/Shop/ShopWindow", isSingleton = true, layer = ViewLayerTypes.Window)]
    public class ShopWindow : BaseView
    {
        public UIButton closeBtn = null;

        public UIGrid itemGrid = null;

        private IList<ShopItemRenderer> shopItems = null;

        public override void Show(object data = null)
        {
            base.Show(data);

            UIEventListener.Get(closeBtn.gameObject).onClick += OnClick;
        }

        public override void Hide()
        {
            base.Hide();

            shopItems.Clear();

            EventBox.RemoveAll(this);
        }

        void Awake()
        {
            ShowItems();
        }

        private void ShowItems()
        {
            shopItems = new List<ShopItemRenderer>();

            foreach (int type in Enum.GetValues(typeof(ShopItemTypes)))
            {
                ShopItemRenderer itemRenderer = ResourceUtils.GetComponent<ShopItemRenderer>(ShopItemRenderer.PREFAB_PATH);
                itemRenderer.Initialize(type);
                itemRenderer.transform.SetParent(itemGrid.transform);
                shopItems.Add(itemRenderer);
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