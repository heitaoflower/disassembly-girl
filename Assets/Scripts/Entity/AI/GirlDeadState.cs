using Orange.StateKit;
using Utils;
using Prototype;
using System.Collections.Generic;
namespace Entity.AI
{
    public class GirlDeadState : State<GirlEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().weaponLauncher.Unload();

            GetContext().skillLauncher.Unload();

            GetContext().componentsHolder.GetComponent(ComponentDefs.Body).animator.AnimationCompleted = null;

            GetContext().Play(AnimationDefs.Dead.ToString().ToLower());

            EventBox.Send(CustomEvent.TROPHY_UPDATE, new KeyValuePair<TrophyType, float>(TrophyType.GirlDead, 1));

            EventBox.Send(CustomEvent.DUNGEON_GIRL_DEAD);
        }

        public override void End()
        {
            base.End();
        }

        public override void Update(float deltaTime)
        {
          
        }

        public override string GetName()
        {
            return StateTypes.Dead.ToString();
        }
    }
}