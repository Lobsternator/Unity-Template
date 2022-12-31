using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ApplicationStateMachine))]
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -100)]
    public class ApplicationStateManager : PersistentRuntimeSingleton<ApplicationStateManager, ApplicationStateManagerData>, IStateManager<ApplicationStateMachine>
    {
        public static bool IsApplicationQuitting { get; private set; } = false;

        public ApplicationStateMachine StateMachine { get; private set; }
        public Dictionary<Type, State<ApplicationStateMachine>> States { get; private set; }

        public TState GetState<TState>() where TState : State<ApplicationStateMachine>
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return (TState)state;

            return null;
        }

        public bool SetState<TState>() where TState : State<ApplicationStateMachine>
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return StateMachine.SetState(state);

            return false;
        }

        private IEnumerator ResetApplicationQuitting()
        {
            yield return CoroutineUtility.WaitForFrames(1);

            IsApplicationQuitting = false;
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            IsApplicationQuitting     = false;
            Application.wantsToQuit  += () =>
            {
                IsApplicationQuitting = true;
                if (this) StartCoroutine(ResetApplicationQuitting());

                return true;
            };

            StateMachine = GetComponent<ApplicationStateMachine>();
            States       = StateMachineUtility.GetStates(PersistentData);
            StateMachineUtility.InitializeStates(States.Values, StateMachine);

            SetState<ApplicationStateTest>();
        }
    }
}
