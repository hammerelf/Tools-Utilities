using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace HammerElf.Tools.Utilities
{
    /// <summary>
    /// Placed on the root GameObject of a prefab to give it automatic object pooling.
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        private static Dictionary<int, Pool> pools = new Dictionary<int, Pool>();
        //private static Dictionary<int, ObjectPool<GameObject>> pools = new Dictionary<int, ObjectPool<GameObject>>();

        private class Pool
        {
            GameObject prefab;
            Queue<GameObject> pool = new Queue<GameObject>();
            int maxSize;
            int currentSize = 0;
            int avaliable = 0;
            public bool ObjectAvaliable
            {
                get
                {
                    return pool.Count > 0 || currentSize < maxSize;
                }
            }

            public Pool(int maxSize, GameObject prefab)
            {
                this.maxSize = maxSize;
                this.prefab = prefab;
            }

            public GameObject Get()
            {
                GameObject obj = null;
                if (pool.Count > 0)
                {
                    obj = pool.Dequeue();
                    if (obj == null) obj = Get();
                    if (obj == null) return null;
                    avaliable--;
                    obj.SetActive(true);
                }
                else if (currentSize < maxSize)
                {
                    currentSize++;
                    obj = Instantiate(prefab);
                }
                //obj = Instantiate(prefab);
                return obj;
            }

            public void Release(GameObject obj)
            {
                pool.Enqueue(obj);
                //Debug.Log($"Pool size: {pool.Count} - {currentSize}/{maxSize}");
                //Destroy(obj);
                //currentSize--;
                avaliable++;
            }
        }

        [SerializeField]
        private Renderer[] renderers;
        //[SerializeField]
        //private int initialPoolSize = 10;
        [SerializeField]
        private int maximumPoolSize = 20;
        [SerializeField, HideInInspector]
        private int poolID;

        public bool ObjectAvaliable => pools.ContainsKey(poolID) ? pools[poolID].ObjectAvaliable : true;
        public UnityEvent<GameObject> OnGet;
        public System.Action<GameObject> OnGetReset;
        public UnityEvent<GameObject> OnRelease;
        public System.Action<GameObject> OnReleaseReset;

        private void Start()
        {
            renderers = gameObject.GetComponentsInChildren<Renderer>();
        }

        public GameObject RequestObject()
        {
            CheckForPool();
            GameObject obj = pools[poolID].Get();
            OnGet?.Invoke(obj);
            OnGetReset?.Invoke(obj);
            OnReleaseReset = null;
            OnGetReset = null;
            return obj;
        }

        public void ReleaseObject()
        {
            CheckForPool();
            if (!pools.ContainsKey(poolID)) return;

            OnRelease?.Invoke(gameObject);
            OnReleaseReset?.Invoke(gameObject);
            pools[poolID].Release(gameObject);
            gameObject.SetActive(false);
        }

        public void ReleaseObjectImmediately()
        {
            CheckForPool();
            if(!pools.ContainsKey(poolID)) return;
            OnRelease?.Invoke(gameObject);
            OnReleaseReset?.Invoke(gameObject);
            pools[poolID].Release(gameObject);
            gameObject.SetActive(false);           
        }

        private bool IsVisible()
        {
            bool vis = false;
            foreach (Renderer r in renderers)
                if (r.isVisible)
                {
                    vis = true;
                    break;
                }
            return vis;
        }

        private void CheckForPool()
        {
            //short-circuit if object is not a prefab
            //TODO: Make a Utility method out of this
            if (gameObject.GetInstanceID() < 0) return; 
            poolID = gameObject.GetInstanceID();
            if (!pools.ContainsKey(poolID))
            {
                //ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                //    () => Instantiate(prefab),
                //    obj => { obj.SetActive(true); OnGet?.Invoke(obj); },
                //    obj => { OnRelease?.Invoke(obj); obj.SetActive(false); },
                //    obj => Destroy(obj),
                //    true, initialPoolSize, maximumPoolSize);
                //pools.Add(poolID, pool);
                pools.Add(poolID, new Pool(maximumPoolSize, gameObject));
            }
        }
    }
}