using Utils;
using UnityEngine;
using Prototype;
using Newtonsoft.Json;
using System.Collections.Generic;
using Guide;

namespace Manager
{
    public class UserManager : Singleton<UserManager> {

        [HideInInspector]
        public UserData user = null;

        private static readonly int MAX_RP = 999999;

        private static readonly int MAX_GP = 999999;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void CreateUser()
        {
            user = UserData.CreateDefault();
        }

        public bool HasUser()
        {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(typeof(UserData).Name));
        }

        public void LoadUser()
        {
            if (HasUser())
            {
                user = UserData.LoadFromJson(ReadFromDevice());
            }
        }

        public void AddGP(float value)
        {
            if (GetCurrentGP() + value > MAX_GP)
            {
                user.attributeBox.SetAttribute(AttributeKeys.GP, AttributeSetTypes.BaseValue, MAX_GP);
            }
            else
            {
                user.attributeBox.AddAttribute(AttributeKeys.GP, AttributeSetTypes.BaseValue, value);
            }

            EventBox.Send(CustomEvent.USER_APPLY_GP);

            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.UserGP, GetCurrentGP()));
        }

        public void AddRP(float value)
        {
            if (GetCurrentRP() + value > MAX_RP)
            {
                user.attributeBox.SetAttribute(AttributeKeys.RP, AttributeSetTypes.BaseValue, MAX_RP);
            }
            else
            {
                user.attributeBox.AddAttribute(AttributeKeys.RP, AttributeSetTypes.BaseValue, value);
            }
    
            EventBox.Send(CustomEvent.USER_APPLY_RP);
        }

        public float GetCurrentGP()
        {
            float currentGP = user.attributeBox.GetAttribute(AttributeKeys.GP);

            return currentGP;
        }

        public float GetCurrentRP()
        {
            float currentRP = user.attributeBox.GetAttribute(AttributeKeys.RP);

            return currentRP;
        }

        public bool CostRP(float value)
        {
            float currentRP = GetCurrentRP();

            if (user != null && currentRP > value)
            {
                user.attributeBox.SubAttribute(AttributeKeys.RP, AttributeSetTypes.BaseValue, value);

                EventBox.Send(CustomEvent.USER_APPLY_RP);

                return true;
            }

            return false;
        }

        public bool CostGP(float value)
        {
            float currentGP = GetCurrentGP();

            if (user != null && currentGP > value)
            {
                user.attributeBox.SubAttribute(AttributeKeys.GP, AttributeSetTypes.BaseValue, value);

                EventBox.Send(CustomEvent.USER_APPLY_GP);

                return true;
            }

            return false;
        }

        public FunctionData GetFunction(FunctionType type)
        {
            for (int index = 0; index < user.functions.Count; index++)
            {
                if (user.functions[index].id == (int)type)
                {
                    return user.functions[index];
                }
            }

            return null;
        }

        public void OpenFunction(FunctionType type)
        {
            FunctionData functionData = GetFunction(type);

            if (functionData == null)
            {
                functionData = new FunctionData();
                functionData.id = (int)type;
                user.functions.Add(functionData);
            }
        }

        public IList<FunctionData> GetFunctions()
        {
            return user.functions;
        }
      
        public override void Release()
        {
            base.Release();

            SaveToDevice(user);
        }

        private void SaveToDevice(UserData userData)
        {
            if (userData != null && GuideManager.GetInstance().HasFinished(GuideScriptID.G01))
            {
                string userJson = JsonConvert.SerializeObject(userData);

                PlayerPrefs.SetString(typeof(UserData).Name, userJson);
            }

        }

        private string ReadFromDevice()
        {
            return PlayerPrefs.GetString(typeof(UserData).Name);
        }

    }
}