using Utils;
using Prototype;
using System.Collections.Generic;
using System;
using View.Trophy;
namespace Manager
{
    public class TrophyManager : Singleton<TrophyManager>
    {
        public Queue<TrophyData> alertTrophyDatas = null;

        public override void Initialize()
        {
            base.Initialize();

            alertTrophyDatas = new Queue<TrophyData>();

            AddEventLisenters();
        }

        private void AddEventLisenters()
        {
            EventBox.Add(CustomEvent.TROPHY_UPDATE, OnTrophyUpdate);
        }

        public override void Release()
        {
            base.Release();

            EventBox.RemoveAll(this);
        }

        private void OnTrophyUpdate(object data)
        {
            Action<KeyValuePair<TrophyType, float>> functor = (input) => 
            {
                IList<TrophyData> trophyDatas = UserManager.GetInstance().user.trophyDatas;
                foreach (TrophyData trophyData in trophyDatas)
                {
                    if (trophyData.type == input.Key && !trophyData.isCompleted)
                    {
                        if (trophyData.conditionType == TrophyConditionType.Value)
                        {
                            trophyData.value = input.Value;
                        }
                        else if (trophyData.conditionType == TrophyConditionType.Frequency)
                        {
                            trophyData.value += input.Value;
                        }                        

                        if (trophyData.value >= trophyData.threshold)
                        {
                            trophyData.isCompleted = true;

                            alertTrophyDatas.Enqueue(trophyData);
                            
                            LayerManager.GetInstance().AddPopUpView<TrophyAlertBar>(alertTrophyDatas.Dequeue());
                        }
                    }
                }
            };

            KeyValuePair<TrophyType, float> source = (KeyValuePair<TrophyType, float>)data;

            functor(source);
        }
        

    }
}
