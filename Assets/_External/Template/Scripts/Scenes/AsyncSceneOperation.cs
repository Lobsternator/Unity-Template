using Template.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Template.Scenes
{
    /// <summary>
    /// Wrapper around <see cref="AsyncOperation"/>, describes an asynchronous action applied to a <see cref="Scene"/>.
    /// </summary>
    public class AsyncSceneOperation : AsyncOperationWrapper<AsyncSceneOperation>
    {
        public int BuildIndex { get; }

        public AsyncSceneOperation(AsyncOperation operation, int buildIndex) : base(operation)
        {
            BuildIndex = buildIndex;
        }
    }

    /// <summary>
    /// A yielable collection of <see cref="AsyncSceneOperation"/>.
    /// </summary>
    public class AsyncSceneOperationCollection : AsyncOperationWrapperCollection<AsyncSceneOperation, AsyncSceneOperationCollection>
    {
        public AsyncSceneOperationCollection() : base() { }
        public AsyncSceneOperationCollection(int capacity) : base(capacity) { }
    }
}
