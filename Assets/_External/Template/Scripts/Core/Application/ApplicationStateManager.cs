using System.Collections.ObjectModel;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    /// <summary>
    /// <see cref="PersistentRuntimeSingleton{TSingleton, TData}"/> that manages and holds states belonging to <see cref="ApplicationStateMachine"/>.
    /// <b>Automatically created at the start of the program.</b>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ApplicationStateMachine))]
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -100)]
    public class ApplicationStateManager : PersistentRuntimeSingleton<ApplicationStateManager, ApplicationStateManagerData>, IStateManager<ApplicationStateMachine, ApplicationState_Base>
    {
        public static bool IsApplicationQuitting { get; private set; } = false;

        public ApplicationStateMachine StateMachine { get; set; }
        public ReadOnlyDictionary<Type, ApplicationState_Base> States { get; private set; }

        public void Initialize(ApplicationStateMachine stateManager)
        {
            StateMachine = stateManager;
            States       = PersistentData.States;

            foreach (var state in States.Values)
                state.Initialize(stateManager);
        }

        public TState GetState<TState>() where TState : ApplicationState_Base
        {
            if (States.TryGetValue(typeof(TState), out var state))
                return (TState)state;

            return null;
        }
        public bool SetState<TState>() where TState : ApplicationState_Base
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
                if (StateMachine.HasState<ApplicationState_Quit>())
                    return true;

                SetState<ApplicationState_Quit>();
                return Application.isEditor;
            };

            Initialize(GetComponent<ApplicationStateMachine>());
            SetState<ApplicationState_Entry>();
        }
    }
}
