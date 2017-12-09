using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Prototype
{
    public enum AttributeKeys : int
    {
        GP,
        RP,
        ANTI,
        HP,
        MaxHP,
        ATK,
        DEF,
        CRT,
        STR,
        DEX,
        VIT,
        SPD,
    }

    public enum ValidateType : int
    {
        None,
        Injured,
        Count
    }

    public class ValidatePayload
    {
        public ValidateType type = default(ValidateType);

        public bool isCRT = false;

        public int finalCRT = default(int);

        public bool weaponEffectorsTriggered = false;

        public bool missileEffectorsTriggered = false;

        public bool skillEffectorsTriggered = false;

        public WeaponData weaponData = null;

        public SkillData skillData = null;

        public MissileData missileData = null;

        public int damage = default(int);

        public Vector2 hitPoint = Vector2.zero;

        private ValidatePayload(ValidateType type)
        {
            this.type = type;
        }

        public static ValidatePayload CreateEmpty(ValidateType type)
        {
            return new ValidatePayload(type);
        }
}

    public enum AttributeSetTypes : int
    {
        BaseValue = 0,
        ExtraValue,
        PercentValue,
        Count
    }

    public class AttributeWrapper
    {
        public AttributeKeys key = default(AttributeKeys);

        public float[] values = new float[((int)AttributeSetTypes.Count)];

        public AttributeWrapper(AttributeKeys key)
        {
            this.key = key;
        }

        public float GetValue(AttributeSetTypes type)
        {
            return values[(int)type];
        }

        public void SetValue(AttributeSetTypes type, float value)
        {
            values[(int)type] = value;
        }

        public void AddValue(AttributeSetTypes type, float value)
        {
            values[(int)type] += value;
        }

        public void SubValue(AttributeSetTypes type, float value)
        {
            values[(int)type] -= value;
        }          

        public float GetFinalValue()
        {
            return GetValue(AttributeSetTypes.BaseValue) * (1.0f - GetValue(AttributeSetTypes.PercentValue)) + GetValue(AttributeSetTypes.ExtraValue);
        }

    }

    public class AttributeBox
    {
        public IList<AttributeWrapper> attributes = new List<AttributeWrapper>();

        public Action<ValidatePayload> OnValidateCompleted = null;

        public AttributeBox()
        {
       
        }

        public static AttributeBox CreateDefault()
        {
            AttributeBox box = new AttributeBox();
            foreach (AttributeKeys key in Enum.GetValues(typeof(AttributeKeys)))
            {
                box.attributes.Add(new AttributeWrapper(key));
            }
            return box;
        }

        public void AddAttribute(AttributeKeys key, AttributeSetTypes type, float value)
        {
            if (!ContainsAttribute(key))
            {
                throw new ArgumentException(string.Format("No atribute with the key {0} in the box.", key.ToString()));
            }
            else
            {
                Get(key).AddValue(type, value);
            }     
        }

        public void SubAttribute(AttributeKeys key, AttributeSetTypes type, float value)
        {
            if (!ContainsAttribute(key))
            {
                throw new ArgumentException(string.Format("No atribute with the key {0} in the box.", key.ToString()));
            }
            else
            {
                Get(key).SubValue(type, value);
            }
        }

        public void SetAttribute(AttributeKeys key, AttributeSetTypes type, float value)
        {
            if (!ContainsAttribute(key))
            {
                throw new ArgumentException(string.Format("No atribute with the key {0} in the box.", key.ToString()));
            }
            else
            {
                Get(key).SetValue(type, value);
            }
         
        }

        public float GetAttribute(AttributeKeys key)
        {
            if (!ContainsAttribute(key))
            {
                throw new ArgumentException(string.Format("No atribute with the key {0} in the box.", key.ToString()));
            }
            else
            {
                return Get(key).GetFinalValue();
            }
           
        }

        public bool ContainsAttribute(AttributeKeys key)
        {
            for (int index = 0; index < attributes.Count; index++)
            {
                if (attributes[index].key == key)
                {
                    return true;
                }
            }

            return false;

        }

        public void Clear()
        {
            OnValidateCompleted = null;

            attributes.Clear();
        }

        private AttributeWrapper Get(AttributeKeys key)
        {
            var query = from AttributeWrapper item in attributes
                        where item.key == key
                        select item;

            foreach (AttributeWrapper item in query)
            {
                return item;
            }

            return null;
        }

        public ValidatePayload ValidateDirect(ValidateType type, AttributeKeys key, AttributeSetTypes setType, float value)
        {
            ValidatePayload payload = ValidatePayload.CreateEmpty(type);

            SubAttribute(key, setType, value);

            if (type == ValidateType.Injured)
            {
                payload.damage = (int)value;
            }

            return payload;
        }

        public void Validate(ValidateType type, AttributeBox sender, MissileData missile, Vector2 point)
        {
            switch (type)
            {
                case ValidateType.Injured:
                    ProcessInjuredByMissile(sender, missile, this, point);
                    break;
            }
        }

        public void Validate(ValidateType type, AttributeBox sender, WeaponData weapon, Vector2 point)
        {
            switch (type)
            {
                case ValidateType.Injured:
                    ProcessInjuredByWeapon(sender, weapon, this, point);
                    break;
            } 
        }

        public void Validate(ValidateType type, AttributeBox sender, SkillData skill, Vector2 point)
        {
            switch (type)
            {
                case ValidateType.Injured:
                    ProcessInjuredBySkill(sender, skill, this, point);
                    break;
            }
        }

        private Action<AttributeBox, SkillData, AttributeBox, Vector2> ProcessInjuredBySkill = delegate (AttributeBox sender, SkillData skillData, AttributeBox receiver, Vector2 hitPoint)
        {
            ValidatePayload payload = ValidatePayload.CreateEmpty(ValidateType.Injured);
            payload.skillData = skillData;
            payload.hitPoint = hitPoint;

            float finalATK = skillData.attributeBox.GetAttribute(AttributeKeys.ATK) + sender.GetAttribute(AttributeKeys.STR) * 0.5f;

            float CRT = (new System.Random().Next(0, 100) / 100f + skillData.attributeBox.GetAttribute(AttributeKeys.CRT) + sender.GetAttribute(AttributeKeys.VIT) * 0.01f - receiver.GetAttribute(AttributeKeys.ANTI));
            if (CRT > 1.75)
            {
                payload.skillEffectorsTriggered = true;
            }

            payload.finalCRT = (int)CRT;

            if (payload.finalCRT <= 1)
            {
                payload.finalCRT = 1;
            }
            else
            {
                payload.isCRT = true;
            }

            int damage = (int)((finalATK * new System.Random().Next(80, 120) / 100f) - receiver.GetAttribute(AttributeKeys.DEF)) * payload.finalCRT;

            if (damage <= 0)
            {
                damage = 1 * payload.finalCRT;
            }

            payload.damage = damage;

            receiver.SubAttribute(AttributeKeys.HP, AttributeSetTypes.BaseValue, damage);

            if (receiver.OnValidateCompleted != null)
            {
                receiver.OnValidateCompleted(payload);
            }
        };

        private Action<AttributeBox, MissileData, AttributeBox, Vector2> ProcessInjuredByMissile = delegate (AttributeBox sender, MissileData missileData, AttributeBox receiver, Vector2 hitPoint)
        {
            ValidatePayload payload = ValidatePayload.CreateEmpty(ValidateType.Injured);
            payload.missileData = missileData;
            payload.hitPoint = hitPoint;

            float finalATK = missileData.attributeBox.GetAttribute(AttributeKeys.ATK);

            float CRT = (new System.Random().Next(0, 100) / 100f + missileData.attributeBox.GetAttribute(AttributeKeys.CRT) - receiver.GetAttribute(AttributeKeys.ANTI));
            if (CRT > 1.75)
            {
                payload.missileEffectorsTriggered = true;
            }

            payload.finalCRT = (int)CRT;

            if (payload.finalCRT <= 1)
            {
                payload.finalCRT = 1;
            }
            else
            {
                payload.isCRT = true;
            }

            int damage = (int)((finalATK * new System.Random().Next(80, 120) / 100f) - receiver.GetAttribute(AttributeKeys.DEF)) * payload.finalCRT;

            if (damage <= 0)
            {
                damage = 1 * payload.finalCRT;
            }

            payload.damage = damage;

            receiver.SubAttribute(AttributeKeys.HP, AttributeSetTypes.BaseValue, damage);

            if (receiver.OnValidateCompleted != null)
            {
                receiver.OnValidateCompleted(payload);
            }
        };

        private Action<AttributeBox, WeaponData, AttributeBox, Vector2> ProcessInjuredByWeapon = delegate (AttributeBox sender, WeaponData weaponData, AttributeBox receiver, Vector2 hitPoint)
        {
            ValidatePayload payload = ValidatePayload.CreateEmpty(ValidateType.Injured);
            payload.weaponData = weaponData;
            payload.hitPoint = hitPoint;

            float finalATK = weaponData.attributeBox.GetAttribute(AttributeKeys.ATK) + sender.GetAttribute(AttributeKeys.STR) * 0.3f;

            float CRT = (new System.Random().Next(0, 100) / 100f + weaponData.attributeBox.GetAttribute(AttributeKeys.CRT) + sender.GetAttribute(AttributeKeys.VIT) * 0.01f - receiver.GetAttribute(AttributeKeys.ANTI));
            if (CRT > 1.75)
            {
                payload.weaponEffectorsTriggered = true;
            }

            payload.finalCRT = (int)CRT;

            if (payload.finalCRT <= 1)
            {
                payload.finalCRT = 1;
            }
            else
            {
                payload.isCRT = true;
            }

            int damage = (int)((finalATK * new System.Random().Next(80, 120) / 100f) - receiver.GetAttribute(AttributeKeys.DEF)) * payload.finalCRT;

            if (damage <= 0)
            {
                damage = 1 * payload.finalCRT;
            }

            payload.damage = damage;

            receiver.SubAttribute(AttributeKeys.HP, AttributeSetTypes.BaseValue, damage);

            if (receiver.OnValidateCompleted != null)
            {
                receiver.OnValidateCompleted(payload);
            }
        };
    }
}