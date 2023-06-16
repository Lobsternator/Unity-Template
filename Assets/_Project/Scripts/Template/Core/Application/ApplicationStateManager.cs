using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    /// <summary>
    /// Manages and holds states belonging to <see cref="ApplicationStateMachine"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ApplicationStateMachine))]
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -100)]
    public class ApplicationStateManager : PersistentRuntimeSingleton<ApplicationStateManager, ApplicationStateManagerData>, IStateManager<ApplicationStateMachine, ApplicationStateBase>
    {
        public static bool IsApplicationQuitting { get; private set; } = false;

        public ApplicationStateMachine StateMachine { get; set; }
        public ReadOnlyDictionary<Type, ApplicationStateBase> States { get; private set; }

        public void Initialize(ApplicationStateMachine stateManager)
        {
            StateMachine = stateManager;
            States       = PersistentData.States;

            foreach (var state in States.Values)
                state.Initialize(stateManager);
        }

        public TState GetState<TState>() where TState : ApplicationStateBase
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return (TState)state;

            return null;
        }
        public bool SetState<TState>() where TState : ApplicationStateBase
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return StateMachine.SetState(state);

            return false;
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
                if (StateMachine.HasState<ApplicationStateQuit>())
                    return true;

                SetState<ApplicationStateQuit>();
                return Application.isEditor;
            };

            Initialize(GetComponent<ApplicationStateMachine>());
            SetState<ApplicationStateEntry>();
        }
    }
}
