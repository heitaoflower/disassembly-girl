using Orange.StateKit;
using Utils;
using Manager;

namespace Entity.AI
{
    public class MonsterTransferState : State<MonsterEntity2D>
    {
        public override void Begin(object data = null)
        {
            base.Begin();

            GetContext().motor2D.boxCollider.isTrigger = true;

            GetContext().FadeOut(() => { EventBox.Send(CustomEvent.DUNGEON_MONSTER_TRANSFER, GetContext()); });

        }

        public override void End()
        {
            base.End();

            if (DungeonManager.GetInstance().girlEntity != null)
            {
                GetContext().LookAt(DungeonManager.GetInstance().girlEntity.transform);
            }

            if (GetContext().motor2D != null)
            {
                GetContext().motor2D.boxCollider.isTrigger = false;
            }

        }
        public override string GetName()
        {
            return StateTypes.Transfer.ToString();
        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}
