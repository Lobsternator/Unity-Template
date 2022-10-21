using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Template.Core
{
    public class AsyncOperationCollection : CustomYieldInstruction
    {
        public List<AsyncOperation> Operations { get; }

        public override bool keepWaiting => !Operations.TrueForAll((o) => o.isDone);

        public AsyncOperationCollection()
        {
            Operations = new List<AsyncOperation>();
        }
        public AsyncOperationCollection(int capacity)
        {
            Operations = new List<AsyncOperation>(capacity);
        }

        public void AddOperation(AsyncOperation operation)
        {
            Operations.Add(operation);
        }
    }
}
