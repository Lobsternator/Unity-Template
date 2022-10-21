using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public abstract class CustomYieldInstructionCollection<TCustomAsyncOperation> : CustomYieldInstruction where TCustomAsyncOperation : CustomYieldInstruction
    {
        public List<TCustomAsyncOperation> Operations { get; protected set; }

        public override bool keepWaiting => !Operations.TrueForAll((o) => !o.keepWaiting);

        public virtual void AddOperation(TCustomAsyncOperation operation)
        {
            Operations.Add(operation);
        }
    }
}
