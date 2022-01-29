using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // [SerializeMonoBehaviourField] private ImmutableStack<GameObject> mechanicalPreCheckpointStack;
        // [SerializeMonoBehaviourField] private ImmutableStack<GameObject> naturePreCheckpointStack;
        // private ImmutableStack<GameObject> mechanicalPostCheckpointStack;
        // private ImmutableStack<GameObject> naturePostCheckpointStack;
        [SerializeMonoBehaviourField] private int mechanicalStartAmount;
        [SerializeMonoBehaviourField] private int natureStartAmount;
        // [SerializeMonoBehaviourField] private int mechanicalEndAmount;
        // [SerializeMonoBehaviourField] private int natureEndAmount;

        public void PushPreCheckpoint(GameObject obj)
        {
            // if (obj.GetComponent<Collectible>().type == CollectibleType.NATURE)
            // {
            //     naturePreCheckpointStack.Push(obj);
            //     
            // }
            // else
            // {
            //     mechanicalPreCheckpointStack.Push(obj);
            // }
            //obj.SetActive(false);
        }

        public void IncrementNature() => natureStartAmount++;
        public void IncrementMechanical() => mechanicalStartAmount++;
        

        // public void PushPostCheckpoint()
        // {
        //     while (!naturePreCheckpointStack.IsEmpty)
        //     {
        //         GameObject tmp;
        //         naturePreCheckpointStack = naturePreCheckpointStack.Pop(out tmp);
        //         naturePostCheckpointStack = naturePostCheckpointStack.Push(tmp);
        //     }
        //
        //     while (!mechanicalPreCheckpointStack.IsEmpty)
        //     {
        //         GameObject tmp;
        //         mechanicalPreCheckpointStack = mechanicalPreCheckpointStack.Pop(out tmp);
        //         mechanicalPostCheckpointStack = mechanicalPostCheckpointStack.Push(tmp);
        //     }
        // }


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            mechanicalStartAmount = 0;
            natureStartAmount = 0;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public int GetMechanicalAmount() => mechanicalStartAmount;

        public int GetNatureAmount() => natureStartAmount;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            // mechanicalPreCheckpointStack = new ImmutableStack<GameObject>();
            // naturePreCheckpointStack = new ImmutableStack<GameObject>();
            //
            // mechanicalPostCheckpointStack = new ImmutableStack<GameObject>();
            // naturePostCheckpointStack = new ImmutableStack<GameObject>();
        }

        private void Update()
        {
            print($"Mechanical: {mechanicalStartAmount}, Nature: {natureStartAmount}");
        }
    }
}