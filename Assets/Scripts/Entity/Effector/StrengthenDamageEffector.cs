using System.Collections;
using Prototype;

namespace Entity.Effector
{
    public class StrengthenDamageEffector : AbstractApplierEffector
    {
        protected override void Activate()
        {
            base.Activate();
        }

        public override void ApplyPayload(ValidatePayload payload)
        {
            payload.damage = (int)(payload.damage * (1 + data.parameter1));
        }

        protected override void Deactivate()
        {
            base.Deactivate();

            Destroy(this);
        }

        protected override IEnumerator RunProcessor()
        {
            yield return null;

            Cancel();
        }
    }
}