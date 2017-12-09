using UnityEngine;
using Manager;
using Prototype;

namespace View.Dungeon
{
    [View(prefabPath ="UI/Dungeon/DamagePopUpUI", layer = ViewLayerTypes.Scene, isSingleton =false)]
    public class DamagePopUpUI : BaseView
    {
        public UILabel valueLabel = null;

        public UILabel value2Label = null;

        public UILabel value3Label = null;

        void Awake()
        {

        }

        void Start()
        {
            StartCoroutine(TweenTransformTo(transform, 0.5f, transform.localPosition + new Vector3(0, 10f), transform.localScale, transform.eulerAngles.z, OnTweenComplete));
        }

        public void Initialize(ValidatePayload payload)
        {
            if (payload.isCRT)
            {
                if (payload.finalCRT == 2)
                {
                    value2Label.gameObject.SetActive(true);

                    value2Label.text = payload.damage.ToString();
                }
                else if (payload.finalCRT >= 3)
                {
                    value3Label.gameObject.SetActive(true);

                    value3Label.text = payload.damage.ToString();
                }
             
            }
            else
            {
                valueLabel.gameObject.SetActive(true);

                valueLabel.text = payload.damage.ToString();
            }
           
        }

        void OnTweenComplete()
        {
            LayerManager.GetInstance().RemovePopUpView(this);
        }
       
    }
}