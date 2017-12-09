using UnityEngine;
using Prototype;
using Manager;
using System;
using Utils;
using System.Collections.Generic;
namespace View.Shop
{
    public class ShopItemRenderer : MonoBehaviour
    {
        public static readonly string PREFAB_PATH = "UI/Shop/ShopItemRenderer";

        public UIButton buyBtn = null;

        public UISprite selloutImg = null;

        public UISprite iconImg = null;

        public UILabel valueLab = null;

        public UILabel gpLab = null;

        public UISprite gpImg = null;

        [HideInInspector]
        public int data = default(int);

        public void Initialize(int data)
        {
            this.data = data;

            Refresh();
        }

        void Awake()
        {
            UIEventListener.Get(buyBtn.gameObject).onClick += delegate (GameObject go)
            {
                Func<float, int> functor = (float value) => { return (int)(100 + value * 10); };

                AttributeBox shopAttributeBox = UserManager.GetInstance().user.shopData.attributeBox;
                AttributeBox girlAttributeBox = UserManager.GetInstance().user.girl.attributeBox;

                float currentValue = 0;
                if (data == (int)ShopItemTypes.Chocolate)
                {
                    currentValue = shopAttributeBox.GetAttribute(AttributeKeys.DEX);
                    shopAttributeBox.AddAttribute(AttributeKeys.DEX, AttributeSetTypes.BaseValue, 1);
                    girlAttributeBox.AddAttribute(AttributeKeys.DEX, AttributeSetTypes.ExtraValue, 1);


                }
                else if (data == (int)ShopItemTypes.Hamburg)
                {
                    currentValue = shopAttributeBox.GetAttribute(AttributeKeys.STR);
                    shopAttributeBox.AddAttribute(AttributeKeys.STR, AttributeSetTypes.BaseValue, 1);
                    girlAttributeBox.AddAttribute(AttributeKeys.STR, AttributeSetTypes.ExtraValue, 1);
                }
                else if (data == (int)ShopItemTypes.Juice)
                {
                    currentValue = shopAttributeBox.GetAttribute(AttributeKeys.SPD);
                    shopAttributeBox.AddAttribute(AttributeKeys.SPD, AttributeSetTypes.BaseValue, 1);
                    girlAttributeBox.AddAttribute(AttributeKeys.SPD, AttributeSetTypes.ExtraValue, 1);
                }
                else if (data == (int)ShopItemTypes.Milk)
                {
                    currentValue = shopAttributeBox.GetAttribute(AttributeKeys.VIT);
                    shopAttributeBox.AddAttribute(AttributeKeys.VIT, AttributeSetTypes.BaseValue, 1);
                    girlAttributeBox.AddAttribute(AttributeKeys.VIT, AttributeSetTypes.ExtraValue, 1);
                }

                UserManager.GetInstance().CostGP(functor(currentValue));

                Refresh();
            };
        }

        void Start()
        {
            transform.localPosition = Vector2.zero;
            transform.localScale = Vector2.one;
        }

        public void Refresh()
        {
            Predicate<float> predicator = (float value) => { return value < 50 ? false : true; };
            Func<float, int> functor = (float value) => { return (int)(100 + value * 10); };

            AttributeBox shopAttributeBox = UserManager.GetInstance().user.shopData.attributeBox;
            float attributeValue = 0;
            if (data == (int)ShopItemTypes.Chocolate)
            {
                attributeValue = shopAttributeBox.GetAttribute(AttributeKeys.DEX);
            }
            else if (data == (int)ShopItemTypes.Hamburg)
            {
                attributeValue = shopAttributeBox.GetAttribute(AttributeKeys.STR);
            }
            else if (data == (int)ShopItemTypes.Juice)
            {
                attributeValue = shopAttributeBox.GetAttribute(AttributeKeys.SPD);
            }
            else if (data == (int)ShopItemTypes.Milk)
            {
                attributeValue = shopAttributeBox.GetAttribute(AttributeKeys.VIT);
            }

            valueLab.text = attributeValue.ToString();
            iconImg.spriteName = data.ToString();

            if (predicator(attributeValue))
            {
                EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.ShopLevelMax, 1));

                gpLab.gameObject.SetActive(false);
                gpImg.gameObject.SetActive(false);
                selloutImg.gameObject.SetActive(true);
                buyBtn.gameObject.SetActive(false);
            }
            else
            {
                int costGP = functor(attributeValue);
                if (UserManager.GetInstance().GetCurrentGP() < costGP)
                {
                    gpLab.color = Color.red;
                    buyBtn.enabled = false;
                }
                else
                {
                    gpLab.color = Color.white;
                    buyBtn.enabled = true;
                }

                gpLab.text = costGP.ToString();
                gpLab.gameObject.SetActive(true);
                gpImg.gameObject.SetActive(true);
                buyBtn.gameObject.SetActive(true);
                selloutImg.gameObject.SetActive(false);
            }
        }

    }
}