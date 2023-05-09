using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public class AsyncOperationWrapper<TWrapper> : CustomYieldInstruction where TWrapper : CustomYieldInstruction
    {
        public AsyncOperation Operation { get; }

        public override bool keepWaiting => Operation == null ? false : !Operation.isDone;

        public event Action<TWrapper> completed;

        public AsyncOperationWrapper(AsyncOperation operation)
        {
            Operation = operation;

            if (Operation != null)
                Operation.completed += OnAysncOperationCompleted;
        }

        protected virtual void OnAysncOperationCompleted(AsyncOperation operation)
        {
            OnCompleted();
        }

        protected virtual void OnCompleted()
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

            if (operation != null)
                operation.completed += OnAysncOperationWrapperCompleted;
        }

        protected virtual void OnAysncOperationWrapperCompleted(TWrapper operation)
        {
            if (!keepWaiting)
                OnCompleted();
        }

        protected virtual void OnCompleted()
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
