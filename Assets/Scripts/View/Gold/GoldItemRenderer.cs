using UnityEngine;
using System.Collections;
using Prototype;

namespace View.Gold
{
    public class GoldItemRenderer : MonoBehaviour
    {
        public static readonly string PREFAB_PATH = "UI/Gold/GoldItemRenderer";

        public UISprite iconImg = null;

        public UISprite winImg = null;

        [HideInInspector]
        public GoldData data = null;

        public void Initialize(GoldData data)
        {
            this.data = data;

            Refresh();
        }

        void Awake()
        {
            

        }

        void Start()
        {
            transform.localPosition = Vector2.zero;
            transform.localScale = Vector2.one;
        }

        public void Refresh()
        {
            iconImg.spriteName = 0.ToString();
        }

        public IEnumerator Shuffle()
        {
            DisassemblygirlGoldConfigTable goldTable = ConfigMgr.GetInstance().DisassemblygirlGold;
         
            int lastValue = default(int);
            while (true)
            {
                int value = Random.Range(1, goldTable.configs.Count);
                if (value == lastValue)
                {
                    value = (value == goldTable.configs.Count ? value - 1 : value + 1);                   
                }

                lastValue = value;

                DisassemblygirlGoldConfig config = goldTable.GetConfigById(lastValue);
                iconImg.spriteName = config.iconID;
                data.name = goldTable.GetConfigById(lastValue).name;
                data.value = config.value;

                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}