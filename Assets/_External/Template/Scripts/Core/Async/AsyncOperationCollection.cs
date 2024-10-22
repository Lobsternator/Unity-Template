using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Serves as a yieldable collection of <see cref="AsyncOperation"/>.
    /// </summary>
    public class AsyncOperationCollection : CustomYieldInstruction
    {
        protected List<AsyncOperation> _operations;
        public ReadOnlyCollection<AsyncOperation> Operations { get; }

        public override bool keepWaiting => !_operations.TrueForAll((o) => o == null || o.isDone);

        public event Action<AsyncOperationCollection> completed;

        public AsyncOperationCollection()
        {
            _operations = new List<AsyncOperation>();
            Operations  = new ReadOnlyCollection<AsyncOperation>(_operations);
        }
        public AsyncOperationCollection(int capacity)
        {
            _operations = new List<AsyncOperation>(capacity);
            Operations  = new ReadOnlyCollection<AsyncOperation>(_operations);
        }

        public void AddOperation(AsyncOperation operation)
        {
            _operations.Add(operation);

            if (operation != null)
                operation.completed += OnAysncOperationCompleted;
        }

        private void OnAysncOperationCompleted(AsyncOperation operation)
        {
            if (!keepWaiting)
                completed?.Invoke(this);
        }
    }
}
