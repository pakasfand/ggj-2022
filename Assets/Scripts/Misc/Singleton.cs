using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Misc
{
    public abstract class Singleton<T> : AbstractSingleton where T : MonoBehaviour
    {
        [CanBeNull] private static T _instance;

        [NotNull]
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        [SerializeField] private bool _persistent = true;

        [NotNull]
        public static T instance
        {
            get
            {
                if (quitting)
                {
                    Debug.LogWarning(
                        $"[{typeof(Singleton<>)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return null;
                }

                lock (globalLock)
                {
                    if (!initialized)
                    {
                        initialized = true;
                        var subclasses =
                            from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where type.IsSubclassOf(typeof(AbstractSingleton))
                            where !type.IsAbstract
                            select type;

                        var currentType = typeof(Singleton<>);

                        foreach (var subclass in subclasses)
                        {
                            var concreteType = currentType.MakeGenericType(new Type[] {subclass});
                            var instanceProp = concreteType.GetProperty(nameof(instance));
                            if (instanceProp != null) 
                                instanceProp.GetMethod.Invoke(null, new object[0]);
                        }
                    }

                    lock (Lock)
                    {
                        if (_instance != null)
                            return _instance;
                        var instances = FindObjectsOfType<T>();
                        var count = instances.Length;
                        if (count > 0)
                        {
                            if (count == 1)
                                return _instance = instances[0];
                            Debug.LogWarning(
                                $"[{typeof(Singleton<>)}<{typeof(T)}>] There should never be more than one {typeof(Singleton<>)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                            for (var i = 1; i < instances.Length; i++)
                                Destroy(instances[i]);
                            return _instance = instances[0];
                        }

                        Debug.Log(
                            $"[{typeof(Singleton<>)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                        return _instance = new GameObject($"({typeof(Singleton<>)}){typeof(T)}")
                            .AddComponent<T>();
                    }
                }
            }
        }

        private void Awake()
        {
            if (_persistent)
                DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }

    public abstract class AbstractSingleton : MonoBehaviour
    {
        protected static bool quitting { get; private set; }
        protected static bool initialized = false;
        protected static object globalLock = new object();

        private void OnApplicationQuit()
        {
            quitting = true;
        }
    }
}