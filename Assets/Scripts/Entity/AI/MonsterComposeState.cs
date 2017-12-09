using Orange.StateKit;

namespace Entity.AI
{
    public class MonsterComposeState : State<MonsterEntity2D>
    {
        private bool hasComposeAnimations = false;

        public override string GetName()
        {
            return StateTypes.Compose.ToString();
        }

        public override void Update(float deltaTime)
        {
            if (!hasComposeAnimations)
            {
                GetStateMachine().ChangeState(StateTypes.Wander.ToString());
            }
        }

        public override void End()
        {
            base.End();
        }

        public override void Begin(object data = null)
        {
            base.Begin();

            for (int index = 0; index < GetContext().componentsHolder.components.Count; index++)
            {
                hasComposeAnimations |= GetContext().componentsHolder.components[index].Play(AnimationDefs.Compose.ToString().ToLower());
            }

            if (hasComposeAnimations)
            {
                EntityComponent component = GetContext().componentsHolder.GetComponent(ComponentDefs.Body);

                component.animator.AnimationCompleted = (tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) =>
                {
                    animator.AnimationCompleted = null;

                    GetStateMachine().ChangeState(StateTypes.Idle.ToString());
                };
            }
        }
    }
}