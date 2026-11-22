using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float fireInterval = 0.2f;
    public float spawnDistance = 1f;
    float nextFire;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFire)
        {
            nextFire = Time.time + fireInterval;
            BulletPool.Instance.Get(
                transform.position + transform.forward * spawnDistance,
                transform.rotation);
        }
    }
}