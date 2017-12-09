using System;
using System.Collections.Generic;

namespace Orange.StateKit
{
    public class StateMachine<T>
    {
        protected T _context;
        #pragma warning disable
        public event Action OnStateChanged = null;
        #pragma warning restore

        public State<T> currentState { get { return _currentState; } }
        public State<T> previousState;
        public float elapsedTimeInState = 0f;

        private Dictionary<string, State<T>> _states = new Dictionary<string, State<T>>();
        private State<T> _currentState;

        public StateMachine(T context, State<T> initialState)
        {
            this._context = context;

            AddState(initialState);
            _currentState = initialState;
            _currentState.Begin();
        }

        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, _context);
            _states[state.GetName()] = state;
        }

        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;
            _currentState.Update(deltaTime);
        }

        public void Dispose()
        {
            foreach (State<T> state in _states.Values)
            {
                state.End();
            }

            _states.Clear();
        }

        public State<T> ChangeState(string name, object data = null)
        {
            if (_currentState.GetName().Equals(name))
            {
                return _currentState;
            }

            if (_currentState != null)
            {
                _currentState.End();
            }

            #if UNITY_EDITOR
            if (!_states.ContainsKey(name))
            {
                var error = GetType() + ": state " + name + " does not exist. Did you forget to add it by calling addState?";
                throw new Exception(error);
            }
            #endif

            previousState = _currentState;
            _currentState = _states[name];
            _currentState.Begin(data);
            elapsedTimeInState = 0f;

            if (OnStateChanged != null)
            {
                OnStateChanged();
            }

            return _currentState;
        }
    }
}