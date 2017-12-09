
using System.Collections;
using UnityEngine;

namespace Effect.Component
{
    public class ComponentDefaultEffect : AbstractComponentEffect
    {        
        public override void Initaizlie(object data = null)
        {
            
        }

        protected override IEnumerator StartTaskProcessor()
        {
            Vector3 detalMovement = Vector3.zero;
            detalMovement.y = Mathf.Sqrt(0.5f * Random.Range(minForce, maxForce) * -gravity);
            detalMovement.x = Mathf.Cos(angle * Mathf.Deg2Rad) * Random.Range(minSpeed, maxSpeed);

            while (true)
            {
                detalMovement.y += gravity * Time.deltaTime;

                transform.Translate(detalMovement * Time.deltaTime, Space.World);

                if (spinEnabled) { transform.Rotate(0, 0, -spin); }                

                yield return null;
            }
        }

    }
}