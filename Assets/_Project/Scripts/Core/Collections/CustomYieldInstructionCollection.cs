using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    public abstract class CustomYieldInstructionCollection<TCustomAsyncOperation> : CustomYieldInstruction where TCustomAsyncOperation : CustomYieldInstruction
    {
        protected List<TCustomAsyncOperation> _operations;
        public ReadOnlyCollection<TCustomAsyncOperation> Operations { get; }

        public override bool keepWaiting => !_operations.TrueForAll((o) => !o.keepWaiting);

        public CustomYieldInstructionCollection()
        {
            _operations = new List<TCustomAsyncOperation>();
            Operations  = new ReadOnlyCollection<TCustomAsyncOperation>(_operations);
        }
        public CustomYieldInstructionCollection(int capacity)
        {
            _operations = new List<TCustomAsyncOperation>(capacity);
            Operations  = new ReadOnlyCollection<TCustomAsyncOperation>(_operations);
        }

        public virtual void AddOperation(TCustomAsyncOperation operation)
        {
            _operations.Add(operation);
        }
    }
}
