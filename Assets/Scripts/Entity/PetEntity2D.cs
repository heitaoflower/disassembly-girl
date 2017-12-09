using UnityEngine;
using System;
using Prototype;
using Orange.StateKit;
using Entity.AI;

namespace Entity
{
    public class PetEntity2D : BaseEntity2D
    {
        [HideInInspector]
        public StateMachine<PetEntity2D> machine = null;
        [HideInInspector]
        public AbstractLauncher weaponLauncher = null;

        public static readonly float DEFAULT_IDLE_TIME = 3;

        public static readonly float DEFAULT_RUN_TIME = 5;

        [HideInInspector]
        public float runTime = default(float);
        [HideInInspector]
        public float idleTime = default(float);
        [HideInInspector]
        public float runCounter = default(float);
        [HideInInspector]
        public float idleCounter = default(float);

        public override void BindData(object data)
        {
            PetData petData = data as PetData;

            this.data = petData;
        }

        public override bool ImmuneEffector(EffectorData effectorData)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            machine = new StateMachine<PetEntity2D>(this, new PetIdleState());
            machine.AddState(new PetAttackState());
        }

        protected override void Update()
        {
            if (machine != null)
            {
                machine.Update(Time.deltaTime);
            }
        }
    }
}