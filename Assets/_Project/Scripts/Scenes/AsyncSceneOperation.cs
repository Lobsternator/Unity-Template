using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template.Core;

namespace Template.Scenes
{
    public class AsyncSceneOperation : AsyncOperationWrapper<AsyncSceneOperation>
    {
        public int BuildIndex { get; }

        public AsyncSceneOperation(AsyncOperation operation, int buildIndex) : base(operation)
        {
            BuildIndex = buildIndex;
        }
    }

    public class AsyncSceneOperationCollection : AsyncOperationWrapperCollection<AsyncSceneOperation, AsyncSceneOperationCollection>
    {
        public AsyncSceneOperationCollection() : base() { }
        public AsyncSceneOperationCollection(int capacity) : base(capacity) { }

        public override void AddOperation(AsyncSceneOperation operation)
        {
            base.AddOperation(operation);

            if (operation is not null)
                operation.completed += OnAysncSceneOperationCompleted;
        }
    }
}
