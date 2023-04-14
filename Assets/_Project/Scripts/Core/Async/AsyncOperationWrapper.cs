using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public class AsyncOperationWrapper<TWrapper> : CustomYieldInstruction where TWrapper : CustomYieldInstruction
    {
        public AsyncOperation Operation { get; }

        public override bool keepWaiting => Operation is null ? false : !Operation.isDone;

        public event Action<TWrapper> completed;

        public AsyncOperationWrapper(AsyncOperation operation)
        {
            Operation = operation;

            if (Operation is not null)
                Operation.completed += OnAysncSceneOperationCompleted;
        }

        protected virtual void OnAysncSceneOperationCompleted(AsyncOperation operation)
        {
            OnCompleted();
        }

        protected void OnCompleted()
        {
            completed?.Invoke(this as TWrapper);
        }
    }

    public class AsyncOperationWrapperCollection<TWrapper, TCollection> : CustomYieldInstructionCollection<TWrapper> where TWrapper : AsyncOperationWrapper<TWrapper> where TCollection : CustomYieldInstructionCollection<TWrapper>
    {
        public event Action<TCollection> completed;

        public AsyncOperationWrapperCollection() : base() { }
        public AsyncOperationWrapperCollection(int capacity) : base(capacity) { }

        public override void AddOperation(TWrapper operation)
        {
            base.AddOperation(operation);

            if (operation is not null)
                operation.completed += OnAysncSceneOperationCompleted;
        }

        protected virtual void OnAysncSceneOperationCompleted(TWrapper operation)
        {
            if (!keepWaiting)
                OnCompleted();
        }

        protected void OnCompleted()
        {
            completed?.Invoke(this as TCollection);
        }
    }
    public class AsyncOperationWrapperCollection<TWrapper> : AsyncOperationWrapperCollection<TWrapper, AsyncOperationWrapperCollection<TWrapper>> where TWrapper : AsyncOperationWrapper<TWrapper>
    {
        public AsyncOperationWrapperCollection() : base() { }
        public AsyncOperationWrapperCollection(int capacity) : base(capacity) { }
    }
}
