using System.Collections;
using UnityEngine;
using Utils;

namespace Entity.Effector
{
    public class DecelerationEffector : AbstractApplierEffector
    {
        Transform effector = null;

        protected override void Activate()
        {
            base.Activate();            

            owner.data.attributeBox.SubAttribute(Prototype.AttributeKeys.SPD, Prototype.AttributeSetTypes.PercentValue, -data.parameter2);

            Transform mountPoint = owner.GetMountPoint(ComponentDefs.Foot);

            if (mountPoint != null)
            {
                effector = ResourceUtils.GetComponent<Transform>(GlobalDefinitions.RESOURCE_PATH_BUFF + data.resourceID);
                effector.SetParent(mountPoint);
                effector.localPosition = Vector3.zero;
            }

        }

        protected override void Deactivate()
        {
            base.Deactivate();

            owner.data.attributeBox.AddAttribute(Prototype.AttributeKeys.SPD, Prototype.AttributeSetTypes.PercentValue, -data.parameter2);

            if (effector != null)
            {
                Destroy(effector.gameObject);
            }

            Destroy(this);
        }

        protected override IEnumerator RunProcessor()
        {
            yield return new WaitForSeconds(data.parameter1);

            Cancel();
        }
    }
}