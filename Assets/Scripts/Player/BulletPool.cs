using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public Bullet prefab;
    public int initialSize = 1;
    readonly Queue<Bullet> pool = new Queue<Bullet>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < initialSize; i++) Create();
    }

    Bullet Create()
    {
        var b = Instantiate(prefab, transform);
        b.gameObject.SetActive(false);
        pool.Enqueue(b);
        return b;
    }

    public Bullet Get(Vector3 pos, Quaternion rot)
    {
        if (pool.Count == 0) Create();
        var b = pool.Dequeue();
        b.transform.SetPositionAndRotation(pos, rot);
        b.gameObject.SetActive(true);
        return b;
    }

    public void Return(Bullet b)
    {
        if (!b.gameObject.activeSelf) return;
        b.gameObject.SetActive(false);
        pool.Enqueue(b);
    }
}