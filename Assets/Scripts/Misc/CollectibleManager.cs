using UnityEngine;

namespace Misc
{
    public enum CollectibleType : byte
    {
        MECHANICAL,
        NATURE
    }

    [SerializeMonoBehaviour]
    public class CollectibleManager : Singleton<CollectibleManager>
    {
        [SerializeMonoBehaviourField] private ImmutableStack<GameObject> mechanicalPreCheckpointStack;
        [SerializeMonoBehaviourField] private ImmutableStack<GameObject> naturePreCheckpointStack;
        private ImmutableStack<GameObject> mechanicalPostCheckpointStack;
        private ImmutableStack<GameObject> naturePostCheckpointStack;

        public void PushPreCheckpoint(GameObject obj)
        {
            if (obj.GetComponent<Collectible>().type == CollectibleType.NATURE)
            {
                naturePreCheckpointStack.Push(obj);
            }
            else
            {
                mechanicalPostCheckpointStack.Push(obj);
            }
        }

        public void PushPostCheckpoint()
        {
            while (!naturePreCheckpointStack.IsEmpty)
            {
                GameObject tmp;
                naturePreCheckpointStack = naturePreCheckpointStack.Pop(out tmp);
                naturePostCheckpointStack = naturePostCheckpointStack.Push(tmp);
            }
            
            while (!mechanicalPreCheckpointStack.IsEmpty)
            {
                GameObject tmp;
                mechanicalPreCheckpointStack = mechanicalPreCheckpointStack.Pop(out tmp);
                mechanicalPostCheckpointStack = mechanicalPostCheckpointStack.Push(tmp);
            }
            
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}