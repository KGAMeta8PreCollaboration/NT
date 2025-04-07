using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private GameObject objectPools;
    [SerializeField] private ShortNote shortNotePrefab;
    [SerializeField] private LongNote longNotePrefab;
    [SerializeField] private TopNote topNotePrefab;
    [SerializeField] private TopNoteProjectile topNoteProjPrefab;
    [SerializeField] private HitEffect hitEffectPrefab;
    public ObjectPool<Note> shortNotePool;
    public ObjectPool<Note> longNotePool;
    public ObjectPool<Note> topNotePool;
    public ObjectPool<TopNoteProjectile> topNoteProjPool;
    public ObjectPool<HitEffect> hitEffectPool;

    protected override void Awake()
    {
        base.Awake();
        objectPools = new GameObject();
        objectPools.name = "ObjectPools";

        shortNotePool = new ObjectPool<Note>
        (shortNotePrefab, 25, objectPools);

        longNotePool = new ObjectPool<Note>
        (longNotePrefab, 20, objectPools);

        topNotePool = new ObjectPool<Note>
        (topNotePrefab, 30, objectPools);

        topNoteProjPool = new ObjectPool<TopNoteProjectile>
        (topNoteProjPrefab, 20, objectPools);

        hitEffectPool = new ObjectPool<HitEffect>
        (hitEffectPrefab, 40, objectPools);

    }

    public Note PopNote(Note note)
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

    public void PushNote(Note note)
    {
        switch (note.GetType())
        {
            case Type t when t == typeof(ShortNote):
                shortNotePool.Push(note);
                break;
            case Type t when t == typeof(LongNote):
                longNotePool.Push(note);
                break;
            case Type t when t == typeof(TopNote):
                topNotePool.Push(note);
                break;
        }
    }

    public void HitEffect(Vector3 spawnPosition, bool isEffectHorizontal)
    {
        HitEffect hitEffect = hitEffectPool.Pop();
        hitEffect.gameObject.transform.position = spawnPosition;
        if (isEffectHorizontal)
        {
            hitEffect.EffectHorizontal();
        }
        else
        {
            hitEffect.EffectBillboard();
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
        T newObj = GameObject.Instantiate(prefab, poolObj.transform, true);
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
