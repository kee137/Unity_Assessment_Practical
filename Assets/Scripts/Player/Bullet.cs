using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    float timer;

    void Awake()
    {
        // force the setup needed for trigger hits
        GetComponent<Collider>().isTrigger = true;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void OnEnable() { timer = lifeTime; }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0f) BulletPool.Instance.Return(this);
    }

    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Enemy")) return;
        var e = other.GetComponentInParent<Enemy>();
        if (e != null) e.TakeDamage(1);
        BulletPool.Instance.Return(this);
    }
}