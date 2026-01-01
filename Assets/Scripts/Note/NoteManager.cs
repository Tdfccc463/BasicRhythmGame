//管理音符对象池
//最终更新后检查：√
using System;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public NoteController notePrefab; 
    public int initialPoolSize = 50;//初始对象池大小
    private ObjectPool<NoteController> _notePool;//对象池实例
    private void Awake()
    {
        _notePool = new ObjectPool<NoteController>(notePrefab, this.transform, initialPoolSize);
    }
    public NoteController GetNote()
    {
        return _notePool.Get();
    }
    
    public void ReturnNote(NoteController note)
    {
        _notePool.Return(note);
    }
}