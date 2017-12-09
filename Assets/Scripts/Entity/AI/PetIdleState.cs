using Orange.StateKit;
using Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Entity.AI
{
    public class PetIdleState : State<PetEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin(data);

            GetContext().idleTime = Random.Range(1, PetEntity2D.DEFAULT_IDLE_TIME);
            GetContext().isRunning = false;
            GetContext().idleCounter = 0;

            GetContext().Play(AnimationDefs.Idle.ToString().ToLower());
        }

        public override void End()
        {
            base.End();
        }

        public override void Update(float deltaTime)
        {
            GetContext().idleCounter += Time.deltaTime;

            if (GetContext().idleCounter >= GetContext().idleTime)
            {
                if (SceneManager.GetActiveScene().buildIndex == GlobalDefinitions.SCENE_DUNGEON_INDEX)
                {
                    if (GetContext().componentsHolder.GetComponent(ComponentDefs.Body).GetClipByName(AnimationDefs.Attack.ToString().ToLower()) != null)
                    {
                        GetStateMachine().ChangeState(StateTypes.Attack.ToString());
                    }
                }             
            }
        }

        public override string GetName()
        {
            return StateTypes.Idle.ToString();
        }

    }
}