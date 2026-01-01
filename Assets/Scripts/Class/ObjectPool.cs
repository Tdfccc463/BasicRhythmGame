//对象池，用于复用音符（和其他大量出现的物件）（也许可以换成Unity内置的）
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> where T : Component
{
    private Queue<T> _Pool = new Queue<T>();

    private readonly T _prefab;
    private readonly Transform _parent;//将生成出来的音符挂靠在同一个父物体下
    private readonly Func<T> _createFunc;

    public ObjectPool(T prefab, Transform transform,int num = 10)
    {
        _prefab = prefab;
        _parent = transform;
        _createFunc = () => 
        {
            T newObject = GameObject.Instantiate(_prefab, _parent);//生成新的物件
            newObject.gameObject.SetActive(false);//初始物件不取用
            return newObject;
        };

        for(int i = 0; i < num; i ++)
        {
            _Pool.Enqueue(_createFunc.Invoke());//生成物件并加入池中
        }
    }
    //从池中获取物件
    public T Get()
    {
        T item;
        if(_Pool.Count > 0)//检查池中是否还有音符
        {
            item = _Pool.Dequeue();//出队
        }
        else
        {
            item = _createFunc.Invoke();//若池中已用尽，则新生成
        }
        item.gameObject.SetActive(true);//激活
        return item;
    }

    //将用完的物件返回池中
    public void Return(T item)
    {
        if (item == null) return;
        
        // 隐藏对象
        item.gameObject.SetActive(false); 

        // 将对象重新入队。
        _Pool.Enqueue(item);
    }
    public void Clear()
    {
        while (_Pool.Count > 0)
        {
            T item = _Pool.Dequeue();
            GameObject.Destroy(item.gameObject);
        }
    }
}
