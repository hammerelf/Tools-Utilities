//Created by: Ryan King

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace HammerElf.Tools.Utilities
{
    [DefaultExecutionOrder(-50), ShowOdinSerializedPropertiesInInspector]
    public abstract class SerializedSingleton<T> : MonoBehaviour, ISerializationCallbackReceiver, ISupportsPrefabSerialization where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        [SerializeField, HideInInspector]
        private SerializationData serializationData;

        SerializationData ISupportsPrefabSerialization.SerializationData
        {
            get { return this.serializationData; }
            set { this.serializationData = value; }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
        }

        [field: SerializeField, LabelText("Use Don'tDestroyOnLoad")]
        public bool UseDontDestroyOnLoad { get; private set; } = true;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this as T)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
            if (UseDontDestroyOnLoad)
            {
                DontDestroyOnLoad(transform.root);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}