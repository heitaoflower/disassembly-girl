
namespace Orange.StateKit
{
    public abstract class  State<T>
    {
        private StateMachine<T> _machine;

        private T _context;

        public State()
        {

        }

        internal void SetMachineAndContext(StateMachine<T> machine, T context)
        {
            this._machine = machine;
            _context = context;
        }

        public virtual void Begin(object data = null)
        {

        }

        public abstract void Update(float deltaTime);

        public abstract string GetName();

        public virtual void End()
        {
      
        }

        public T GetContext()
        {
            return _context;
        }

        public StateMachine<T> GetStateMachine()
        {
            return _machine;
        }
    }
}