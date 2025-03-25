using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Shortnote,
    Longnote,
    Topnote,
    Projectile
}

public class PoolManager : Singleton<PoolManager>
{
    private GameObject objectPools;
    public ShortNote shortNotePrefab;
    public LongNote longNotePrefab;
    public TopNote topNotePrefab;
    public TopNoteProjectile topNoteProjPrefab;
    public ObjectPool<ShortNote> shortNotePool;
    public ObjectPool<LongNote> longNotePool;
    public ObjectPool<TopNote> topNotePool;
    public ObjectPool<TopNoteProjectile> topNoteProjPool;

    protected override void Awake()
    {
        base.Awake();
        objectPools = new GameObject();
        objectPools.name = "ObjectPools";

        shortNotePool = new ObjectPool<ShortNote>
        (shortNotePrefab, 10, objectPools);

        longNotePool = new ObjectPool<LongNote>
        (longNotePrefab, 10, objectPools);

        topNotePool = new ObjectPool<TopNote>
        (topNotePrefab, 10, objectPools);

        topNoteProjPool = new ObjectPool<TopNoteProjectile>
        (topNoteProjPrefab, 10, objectPools);
    }

    public Note FindNote(Note note)
    {
        switch (note.GetType())
        {
            case Type t when t == typeof(ShortNote):
                return shortNotePool.Pop();
            case Type t when t == typeof(LongNote):
                return longNotePool.Pop();
            case Type t when t == typeof(TopNote):
                return topNotePool.Pop();
            default:
                throw new ArgumentException("Unsupported note type");
        }
    }
}

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    public GameObject poolObj;
    public ObjectPool(T prefab, int initialSize, GameObject parent)
    {
        this.prefab = prefab;
        poolObj = new GameObject();
        poolObj.name = prefab.GetType().ToString();
        poolObj.transform.SetParent(parent.transform);
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private void AddObjectToPool()
    {
        T newObj = GameObject.Instantiate(prefab, poolObj.transform);
        newObj.gameObject.SetActive(false);
        pool.Enqueue(newObj);
    }

    public T Pop()
    {
        T popObj;
        if (this.pool.Count <= 0) AddObjectToPool();
        popObj = pool.Dequeue();
        popObj.transform.parent = null;
        popObj.gameObject.SetActive(true);
        return popObj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolObj.transform);
        pool.Enqueue(obj);
    }
}
