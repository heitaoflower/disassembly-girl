using UnityEngine;

using Orange.StateKit;
using Manager;
using Prototype;

namespace Entity.AI
{
    public class MonsterWanderState : State<MonsterEntity2D>
    {
        public override string GetName()
        {
            return StateTypes.Wander.ToString();
        }

        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().runCounter = 0;

            GetContext().runTime = Random.Range(1, MonsterEntity2D.DEFAULT_RUN_TIME);

            MonsterData monster = GetContext().data as MonsterData;

            if (monster.type == (int)MonsterType.Flying)
            {
                Vector3 distance = (DungeonManager.GetInstance().girlEntity.transform.position - GetContext().transform.position).normalized;

                float angle = Mathf.Atan2(distance.y, distance.x);
                GetContext().direction.x = Mathf.Cos(angle);
                GetContext().direction.y = Mathf.Sin(angle);
            }

            GetContext().Play(AnimationDefs.Run.ToString().ToLower());

            GetContext().isRunning = true;
        }

        public override void End()
        {
            base.End();

            GetContext().isRunning = false;
        }

        public override void Update(float deltaTime)
        {
            GetContext().runCounter += Time.deltaTime;

            if (GetContext().runCounter < GetContext().runTime)
            {              

                if (GetContext().direction == Vector2.left && GetContext().motor2D.collisionState.left)
                {
                    GetContext().direction = Vector2.right;
                    GetContext().Flip();
                }
                else if (GetContext().direction == Vector2.right && GetContext().motor2D.collisionState.right)
                {
                    GetContext().direction = Vector2.left;
                    GetContext().Flip();
                }
            }
            else
            {
                GetStateMachine().ChangeState(StateTypes.Idle.ToString());
            }
        }
    }
}