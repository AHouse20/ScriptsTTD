using System;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    [SerializeField] private Pool[] pools = null;
    private Transform objectPoolParent;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    private void Start()
    {
        objectPoolParent = this.gameObject.transform;

        foreach (Pool pool in pools)
        {
            CreatePool(pool.prefab, pool.poolSize, pool.componentType);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name;
        GameObject poolParent = new GameObject(prefabName + "Anchor");
        poolParent.transform.SetParent(objectPoolParent);
        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, poolParent.transform) as GameObject;
                newObject.SetActive(false);
                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }

    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if (poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFromPool(poolKey);
            ResetObject(position, rotation, componentToReuse, prefab);
            return componentToReuse;
        }
        else
        {
            Debug.Log("No object pool for " + prefab);
            return null;
        }
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }
}
