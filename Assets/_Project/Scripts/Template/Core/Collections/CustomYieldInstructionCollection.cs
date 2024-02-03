using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Template.Core
{
    /// <summary>
    /// Serves as a yieldable collection of <see cref="CustomYieldInstruction"/>.
    /// </summary>
    /// <typeparam name="TCustomYieldInstruction">The specific <see cref="CustomYieldInstruction"/> the collection should hold.</typeparam>
    public abstract class CustomYieldInstructionCollection<TCustomYieldInstruction> : CustomYieldInstruction where TCustomYieldInstruction : CustomYieldInstruction
    {
        protected List<TCustomYieldInstruction> _operations;
        public ReadOnlyCollection<TCustomYieldInstruction> Operations { get; }

        public override bool keepWaiting => !_operations.TrueForAll((o) => o == null || !o.keepWaiting);

        public CustomYieldInstructionCollection()
        {
            _operations = new List<TCustomYieldInstruction>();
            Operations  = new ReadOnlyCollection<TCustomYieldInstruction>(_operations);
        }
        public CustomYieldInstructionCollection(int capacity)
        {
            _operations = new List<TCustomYieldInstruction>(capacity);
            Operations  = new ReadOnlyCollection<TCustomYieldInstruction>(_operations);
        }

        public virtual void AddOperation(TCustomYieldInstruction operation)
        {
            _operations.Add(operation);
        }
    }
}
