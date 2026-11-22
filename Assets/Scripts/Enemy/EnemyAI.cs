using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State { Patrol, Chase, Attack }

    public Transform player;
    public Vector3 patrolOffset = new Vector3(6, 0, 0);
    public float patrolSpeed = 1f;
    public float chaseSpeed = 3f;      // slower than player (5)
    public float chaseRange = 8f;
    public float loseRange = 12f;
    public float attackRange = 1.5f;
    public float attackExitRange = 2f; // avoids jitter at 1.5m

    public float attackDamage = 10f;
    public float attackInterval = 1f;
    float attackTimer;

    public float separation = 1.1f; // min distance between enemies
    static readonly List<EnemyAI> all = new List<EnemyAI>();

    State state = State.Patrol;
    Vector3 pointA, pointB, target;
    Vector3 baseScale;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        pointA = transform.position;
        pointB = pointA + patrolOffset;
        target = pointB;
        baseScale = transform.localScale;
    }

    void OnEnable()
    {
        state = State.Patrol;
        attackTimer = 0f;
        if (!all.Contains(this)) all.Add(this);
    }

    void OnDisable()
    {
        transform.localScale = baseScale;
        all.Remove(this);
    }

    void Update()
    {
        float dist = FlatDistance(player.position);
        switch (state)
        {
            case State.Patrol: UpdatePatrol(dist); break;
            case State.Chase: UpdateChase(dist); break;
            case State.Attack: UpdateAttack(dist); break;
        }
        Separate();
    }

    void UpdatePatrol(float dist)
    {
        if (dist < chaseRange) { state = State.Chase; return; }
        MoveTo(target, patrolSpeed);
        if (FlatDistance(target) < 0.2f)
            target = (target == pointB) ? pointA : pointB;
    }

    void UpdateChase(float dist)
    {
        if (dist > loseRange) { state = State.Patrol; return; }
        if (dist <= attackRange) { state = State.Attack; return; }
        MoveTo(player.position, chaseSpeed);
    }

    void UpdateAttack(float dist)
    {
        if (dist > attackExitRange)
        {
            transform.localScale = baseScale;
            state = State.Chase;
            return;
        }
        // simple "attack" effect: scale pulse
        float s = 1f + 0.2f * Mathf.Sin(Time.time * 15f);
        transform.localScale = baseScale * s;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
            var ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(attackDamage);
        }
    }

    // push apart from other enemies so they don't stack
    void Separate()
    {
        foreach (var o in all)
        {
            if (o == this) continue;
            Vector3 d = transform.position - o.transform.position;
            d.y = 0;
            float dist = d.magnitude;
            if (dist < 0.001f)
            {
                Vector2 r = Random.insideUnitCircle;
                transform.position += new Vector3(r.x, 0, r.y) * 0.1f;
            }
            else if (dist < separation)
            {
                transform.position += d.normalized * (separation - dist) * 0.5f;
            }
        }
    }

    void MoveTo(Vector3 pos, float speed)
    {
        Vector3 p = new Vector3(pos.x, transform.position.y, pos.z);
        transform.position = Vector3.MoveTowards(transform.position, p, speed * Time.deltaTime);
        Vector3 dir = p - transform.position;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    float FlatDistance(Vector3 pos)
    {
        Vector3 d = pos - transform.position;
        d.y = 0;
        return d.magnitude;
    }
}