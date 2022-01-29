using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public struct Transform
        {
            public Vector3 position;
            public Vector3 localScale;
            public Quaternion rotation;
        }

        public class ObjectSnapshot
        {
            private readonly Dictionary<Type, Dictionary<FieldInfo, object>> fieldSnapshots = new Dictionary<Type, Dictionary<FieldInfo, object>>();
            public Transform transform { get; set; }

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

        public ObjectSnapshot this[GameObject obj]
        {
            get => snapshots[obj.GetInstanceID()];
            set => snapshots[obj.GetInstanceID()] = value;
        }
    }

    public class SerializationManager : Singleton<SerializationManager>
    {
        private Dictionary<Type, List<FieldInfo>> toSerialize;

        private void Start()
        {
            toSerialize = new Dictionary<Type, List<FieldInfo>>();

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

        public Snapshot TakeSnapshot()
        {
            var snapshot = new Snapshot();

            var objects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var obj in objects)
            {
                var objTransform = obj.transform;
                var objectSnapshot = new Snapshot.ObjectSnapshot
                {
                    transform = new Snapshot.Transform()
                    {
                        position = objTransform.position,
                        rotation = objTransform.rotation,
                        localScale = objTransform.localScale
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
        public void RestoreSnapshot(Snapshot snapshot)
        {
            var objects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var obj in objects)
            {
                var objTransform = obj.transform;
                var objectSnapshot = snapshot[obj];
                objTransform.position = objectSnapshot.transform.position;
                objTransform.rotation = objectSnapshot.transform.rotation;
                objTransform.localScale = objectSnapshot.transform.localScale;

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
    }
}