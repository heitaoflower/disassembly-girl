using Orange.StateKit;

namespace Entity.AI
{
    public class MonsterDizzyState : State<MonsterEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().Pause();
        }

        public override void End()
        {
            base.End();

            GetContext().Resume();
        }

        public override string GetName()
        {
            return StateTypes.Dizzy.ToString();
        }

        public override void Update(float deltaTime)
        {

        }
    }
}
