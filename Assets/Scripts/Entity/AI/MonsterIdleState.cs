using Orange.StateKit;

using UnityEngine;

namespace Entity.AI
{
    public class MonsterIdleState : State<MonsterEntity2D>
    {
        public override string GetName()
        {
            return StateTypes.Idle.ToString();
        }

        public override void Update(float deltaTime)
        {
            GetContext().idleCounter += Time.deltaTime;

            if (GetContext().idleCounter >= GetContext().idleTime)
            {
                if (GetContext().componentsHolder.HasComponent(ComponentDefs.Weapon))
                {
                    GetStateMachine().ChangeState(StateTypes.Attack.ToString());
                }
                else
                {
                    GetStateMachine().ChangeState(StateTypes.Wander.ToString());
                }       
            }
        }

        public override void End()
        {
            base.End();
        }

        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().idleTime = Random.Range(1, MonsterEntity2D.DEFAULT_IDLE_TIME);
            GetContext().isRunning = false;
            GetContext().idleCounter = 0;

            GetContext().Play(AnimationDefs.Idle.ToString().ToLower());
        }
    }
}