using Orange.StateKit;

namespace Entity.AI
{

    public class MonsterBackDashState : State<MonsterEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin();  

            GetContext().Play(AnimationDefs.BackDash.ToString().ToLower());
        }

        public override void End()
        {
            base.End();
        }

        public override string GetName()
        {
            return StateTypes.BackDash.ToString();
        }

        public override void Update(float deltaTime)
        {
      
        }
    }
}
