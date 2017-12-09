using Orange.StateKit;

namespace Entity.AI
{
    public class GirlIdleState : State<GirlEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().Play(AnimationDefs.Idle.ToString().ToLower());
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
            return StateTypes.Idle.ToString();
        }
    }
}