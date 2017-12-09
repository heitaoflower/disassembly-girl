using UnityEngine;
using System.Collections;
using Entity.AI;
using Utils;

namespace Entity.Effector
{
    public class DizzyEffector : AbstractApplierEffector
    {
        private Transform effector = null;

        protected override void Activate()
        {
            base.Activate();

            MonsterEntity2D monster = owner as MonsterEntity2D;

            monster.machine.ChangeState(StateTypes.Dizzy.ToString());

            Transform mountPoint = owner.GetMountPoint(ComponentDefs.Head);

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

            MonsterEntity2D monster = owner as MonsterEntity2D;

            if (monster.data.attributeBox.GetAttribute(Prototype.AttributeKeys.HP) > 0)
            {
                monster.machine.ChangeState(StateTypes.Wander.ToString());
            }          

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