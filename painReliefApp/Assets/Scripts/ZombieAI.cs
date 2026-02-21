using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float health;

    public float walkSpeed = 1.2f;
    public float runSpeed = 4f;
    public float detectionRadius = 12f;
    public float hearingRadius = 20f;
    public float attackRange = 1.3f;
    public float attackDamagePercent = 3f; // per hit
    public float attackCooldown = 1.2f;

    public bool isRunner = false; // special fast zombie

    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime = -999f;

    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = walkSpeed;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        // small chance to be a runner
        if (Random.value < 0.12f) isRunner = true;
        if (isRunner) runSpeed *= 1.4f;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // adjust detection based on time of day and weather
        float detectionMod = 1f;
        float hearingMod = 1f;
        if (TimeOfDay.Instance != null && TimeOfDay.Instance.IsNight())
        {
            detectionMod *= 0.6f; // harder to see at night
            hearingMod *= 1.15f;  // rely more on sound
        }
        if (WeatherSystem.Instance != null)
        {
            float vis = WeatherSystem.Instance.GetVisibilityMultiplier();
            detectionMod *= vis;
            if (WeatherSystem.Instance.currentWeather == WeatherSystem.WeatherType.Rain)
                hearingMod *= 1.05f; // rain muffles/changes sound slightly
        }

        float effectiveDetection = detectionRadius * detectionMod;
        float effectiveHearing = hearingRadius * hearingMod;

        // Check sight
        bool canSee = false;
        if (dist <= effectiveDetection)
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * 1.2f;
            Vector3 dir = (player.position + Vector3.up * 1.2f) - origin;
            if (Physics.Raycast(origin, dir.normalized, out hit, effectiveDetection))
            {
                if (hit.collider.CompareTag("Player")) canSee = true;
            }
        }

        // Check hearing (last sound)
        Vector3 soundPos;
        bool heard = false;
        if (SoundManager.Instance != null)
        {
            heard = SoundManager.Instance.HearLastSound(transform.position, effectiveHearing, out soundPos);
        }

        if (canSee)
        {
            agent.SetDestination(player.position);
            agent.speed = (isRunner && dist < effectiveDetection * 0.6f) ? runSpeed : walkSpeed;
        }
        else if (heard)
        {
            agent.SetDestination(soundPos);
            agent.speed = walkSpeed;
        }
        else
        {
            // idle wander or stop
            if (agent.remainingDistance < 0.5f) agent.ResetPath();
        }

        // Attack
        if (dist <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                AttemptAttack();
                lastAttackTime = Time.time;
            }
        }
    }

    void AttemptAttack()
    {
        if (player == null) return;
        var stats = player.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyDamage(attackDamagePercent);
        }
    }

    public void ApplyDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
