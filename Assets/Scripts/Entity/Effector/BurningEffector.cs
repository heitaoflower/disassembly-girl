using System.Collections;
using UnityEngine;
using Utils;
using Prototype;

namespace Entity.Effector
{
    public class BurningEffector : AbstractApplierEffector
    {
        tk2dSpriteAnimator effector = null;

        protected override void Activate()
        {
            base.Activate();

            Transform mountPoint = owner.GetMountPoint(ComponentDefs.Foot);

            if (mountPoint != null)
            {
                effector = ResourceUtils.GetComponent<tk2dSpriteAnimator>(GlobalDefinitions.RESOURCE_PATH_BUFF + data.resourceID);
                effector.transform.SetParent(mountPoint);
                effector.transform.localPosition = Vector3.zero;

                effector.AnimationEventTriggered = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame) => 
                {
                    ValidatePayload payload = owner.data.attributeBox.ValidateDirect(ValidateType.Injured, AttributeKeys.HP, AttributeSetTypes.BaseValue, data.parameter2);
                    payload.hitPoint = owner.gameObject.transform.position;

                    EventBox.Send(CustomEvent.DUNGEON_MONSTER_DAMAGE, new object[] { owner, payload });
                };
            }

        }

        protected override void Deactivate()
        {
            base.Deactivate();

            if (effector != null)
            {
                effector.AnimationEventTriggered = null;
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