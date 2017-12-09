using Prototype;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Utils;

namespace Entity.Effector
{
    public class AbstractApplierEffector : MonoBehaviour
    {
        public EffectorData data = null;

        public BaseEntity2D owner = null;

        #region Base Method

        public void Run()
        {
            Activate();

            StartCoroutine(RunProcessor());
        }

        public void Cancel()
        {
            StopCoroutine(RunProcessor());

            Deactivate();
        }

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {

        }

        protected virtual IEnumerator RunProcessor()
        {
            yield return null;
        }

        public virtual void ApplyPayload(ValidatePayload payload)
        {

        }

        #endregion  

        private static IDictionary<int, Type> effectorRegistry = new Dictionary<int, Type>()
        {
            { 1, typeof(DecelerationEffector)},
            { 2, typeof(DizzyEffector)},
            { 3, typeof(BackDashEffector)},
            { 4, typeof(RaidenEffector)},
            { 5, typeof(BurningEffector)}
        };

        public static void ApplyPayload(BaseEntity2D owner, ValidatePayload payload)
        {
            AbstractApplierEffector[] effectors = owner.GetComponents<AbstractApplierEffector>();

            for (int index = 0; index < effectors.Length; index++)
            {
                effectors[index].ApplyPayload(payload);
            }
        }

        public static void Apply(EffectorData effectorData, BaseEntity2D owner)
        {
            if (owner.ImmuneEffector(effectorData))
            {
                return;
            }

            Type effectorType = null;

            AbstractApplierEffector[] effectors = owner.GetComponents<AbstractApplierEffector>();

            for (int index = 0; index < effectors.Length; index++)
            {
                if (effectors[index].data.id == effectorData.id)
                {
                    return;
                }
            }

            if (effectorRegistry.TryGetValue(effectorData.id, out effectorType))
            {
                AbstractApplierEffector effector =  owner.gameObject.AddComponent(effectorType) as AbstractApplierEffector;
                effector.data = effectorData;
                effector.owner = owner;
                effector.Run();
            }
            else
            {
                LogUtils.LogWarning("Not fount Weapon effector by ID " + effectorData.id);
            }

        }
 
    }
}