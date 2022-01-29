using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace Misc
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializeMonoBehaviourAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SerializeMonoBehaviourFieldAttribute : Attribute
    {
        public bool ByReference = true;
    }

    public class Snapshot
    {
        public struct GenericSerializedData
        {
            public Vector3 position;
            public Vector3 localScale;
            public Quaternion rotation;
            public bool isActive;
        }

        public class ObjectSnapshot
        {
            private readonly Dictionary<Type, Dictionary<FieldInfo, object>> fieldSnapshots = new Dictionary<Type, Dictionary<FieldInfo, object>>();
            public GenericSerializedData genericSerializedData { get; set; }

            public void AddField(Type type, FieldInfo field, object value)
            {
                if (!fieldSnapshots.ContainsKey(type))
                    fieldSnapshots[type] = new Dictionary<FieldInfo, object>();

                fieldSnapshots[type][field] = value;
            }

            public bool HasField(Type type, FieldInfo field)
                => fieldSnapshots[type].ContainsKey(field);

            public object GetField(Type type, FieldInfo field)
                => fieldSnapshots[type][field];
        }

        private readonly Dictionary<int, ObjectSnapshot> snapshots = new Dictionary<int, ObjectSnapshot>();

        [CanBeNull] public ObjectSnapshot this[GameObject obj]
        {
            get => snapshots.ContainsKey(obj.GetInstanceID()) ? snapshots[obj.GetInstanceID()] : null;
            set => snapshots[obj.GetInstanceID()] = value;
        }
    }

    public class SerializationManager : Singleton<SerializationManager>
    {
        private Dictionary<Type, List<FieldInfo>> toSerialize;
        private Stack<Snapshot> timeline;

        private void Awake()
        {
            toSerialize = new Dictionary<Type, List<FieldInfo>>();
            timeline = new Stack<Snapshot>();

            // get all classes to serialize
            var serializableTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(assembly => assembly.GetTypes())
                .SelectMany(t => t)
                .Where(t => t.IsDefined(typeof(SerializeMonoBehaviourAttribute), false))
                .Where(t => t.IsSubclassOf(typeof(MonoBehaviour)))
                .Select(t => (t, t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(f => f.IsDefined(typeof(SerializeMonoBehaviourFieldAttribute), false))
                    .ToList()));

            foreach (var (key, value) in serializableTypes)
            {
                toSerialize[key] = value;
            }
        }

        private Snapshot TakeSnapshot()
        {
            var snapshot = new Snapshot();

            var objects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in objects)
            {
                var objTransform = obj.transform;
                var objectSnapshot = new Snapshot.ObjectSnapshot
                {
                    genericSerializedData = new Snapshot.GenericSerializedData()
                    {
                        position = objTransform.position,
                        rotation = objTransform.rotation,
                        localScale = objTransform.localScale,
                        isActive = obj.activeSelf
                    }
                };
                foreach (var (type, fields) in toSerialize.Select(kv => (kv.Key, kv.Value)))
                {
                    var componentToSerialize = obj.GetComponent(type) as MonoBehaviour;
                    if (componentToSerialize == null)
                    {
                        continue;
                    }

                    foreach (var field in fields)
                    {
                        objectSnapshot.AddField(type, field, field.GetValue(componentToSerialize));
                    }
                }

                snapshot[obj] = objectSnapshot;
            }

            return snapshot;
        }

        /// <summary>
        /// Does not revive dead objects !
        /// </summary>
        private void RestoreSnapshot(Snapshot snapshot)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in objects)
            {
                var objTransform = obj.transform;
                var objectSnapshot = snapshot[obj];
                if (objectSnapshot == null)
                {
                    continue;
                }
                
                objTransform.position = objectSnapshot.genericSerializedData.position;
                objTransform.rotation = objectSnapshot.genericSerializedData.rotation;
                objTransform.localScale = objectSnapshot.genericSerializedData.localScale;
                obj.SetActive(objectSnapshot.genericSerializedData.isActive);

                foreach (var (type, fields) in toSerialize.Select(kv => (kv.Key, kv.Value)))
                {
                    var componentToRestore = obj.GetComponent(type) as MonoBehaviour;
                    if (componentToRestore == null)
                    {
                        continue;
                    }

                    foreach (var field in fields.Where(field => objectSnapshot.HasField(type, field)))
                    {
                        field.SetValue(componentToRestore, objectSnapshot.GetField(type, field));
                    }
                }
            }
        }

        public void CreateSnapshot()
        {
            timeline.Push(TakeSnapshot());
        }

        public void PopSnapshot()
        {
            if (timeline.Count == 0)
            {
                print("Pop failed");
                return;
            }

            timeline.Pop();
        }

        public void Rollback()
        {
            if (timeline.Count == 0)
            {
                print("Rollback failed");
                return;
            }
            
            RestoreSnapshot(timeline.Peek());
        }
    }
}