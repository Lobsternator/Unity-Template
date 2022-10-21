using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Template.Core;

namespace Template.Scenes
{
    public class AsyncSceneOperation : CustomYieldInstruction
    {
        public AsyncOperation Operation { get; }
        public int BuildIndex { get; }

        public override bool keepWaiting => Operation is null ? false : !Operation.isDone;

        public event Action<AsyncSceneOperation> completed;

        public AsyncSceneOperation(AsyncOperation operation, int buildIndex)
        {
            Operation  = operation;
            BuildIndex = buildIndex;

            if (Operation is null)
                return;

            Operation.completed += OnCompleted;
        }

        private void OnCompleted(AsyncOperation operation)
        {
            completed?.Invoke(this);
        }
    }

    public class AsyncSceneOperationCollection : CustomYieldInstructionCollection<AsyncSceneOperation>
    {
        public AsyncSceneOperationCollection()
        {
            Operations = new List<AsyncSceneOperation>();
        }
        public AsyncSceneOperationCollection(int capacity)
        {
            Operations = new List<AsyncSceneOperation>(capacity);
        }
    }
}
