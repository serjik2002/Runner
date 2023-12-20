using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> objectPool = new Queue<T>();
    private T _prefab;
    private bool _isExpandable;

    public ObjectPool(T prefab, int initialSize, bool isExpandable = true)
    {
        this._prefab = prefab;
        this._isExpandable = isExpandable;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    public T GetObjectFromPool()
    {
        if (_isExpandable && objectPool.Count == 0 )
        {
            CreateNewObject();
        }
        T obj = objectPool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }

    private void CreateNewObject()
    {
        T newObj = Object.Instantiate(_prefab);
        newObj.gameObject.SetActive(false);
        objectPool.Enqueue(newObj);
    }

    public bool TryGetObject()
    {
        if(objectPool.Count == 0)
        {
            return false;
        }
        return true;
    }
}