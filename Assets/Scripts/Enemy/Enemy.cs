using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 3;
    public float respawnDelay = 3f;
    int hp;
    Vector3 spawnPos;
    Renderer rend;
    Collider col;
    EnemyAI ai;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        ai = GetComponent<EnemyAI>();
        spawnPos = transform.position;
        hp = maxHp;
    }

    public void TakeDamage(int dmg)
    {
        if (hp <= 0) return;
        hp -= dmg;
        if (hp <= 0) Die();
    }

    void Die()
    {
        GameManager.Instance.AddKill();
        SetAlive(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        transform.position = spawnPos;
        hp = maxHp;
        SetAlive(true);
    }

    void SetAlive(bool alive)
    {
        rend.enabled = alive;
        col.enabled = alive;
        if (ai != null) ai.enabled = alive; // OnEnable resets AI state
    }
}