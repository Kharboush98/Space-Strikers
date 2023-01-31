using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefap;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;


    public static ObjectPooler Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefap);
                obj.transform.parent = transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool tag" + tag + "not found");
            return null;
        }
        GameObject ObjToSpawn = PoolDictionary[tag].Dequeue();

        ObjToSpawn.SetActive(true);
        ObjToSpawn.transform.position = position;
        ObjToSpawn.transform.rotation = rotation;


        IPooledObjects pooled = ObjToSpawn.GetComponent<IPooledObjects>();
        if (pooled != null)
        {
            pooled.OnObjectSpawn();
        }

        PoolDictionary[tag].Enqueue(ObjToSpawn);
        return ObjToSpawn;
    }
}
