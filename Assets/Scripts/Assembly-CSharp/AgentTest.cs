using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    public bool db;
    public Transform player; // Inspector'dan atama yapılmalı veya otomatik bulunmalı
    public Transform wanderTarget;
    public AILocationSelectorScript wanderer;
    public float coolDown;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // EĞER PLAYER BOŞSA OTOMATİK BUL:
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        Wander();
    }

    private void Update()
    {
        if (coolDown > 0f)
        {
            coolDown -= 1f * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // Player hala atanmamışsa hata vermemesi için kontrol
        if (player == null) return;

        Vector3 direction = player.position - transform.position;

        // Raycast kontrolü
        if (Physics.Raycast(transform.position, direction, out var hitInfo, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore) && hitInfo.transform.CompareTag("Player"))
        {
            db = true;
            TargetPlayer();
            return;
        }

        db = false;
        if (agent.velocity.magnitude <= 1f && coolDown <= 0f)
        {
            Wander();
        }
    }

    private void Wander()
    {
        if (wanderer != null && wanderTarget != null)
        {
            wanderer.GetNewTarget();
            agent.SetDestination(wanderTarget.position);
            coolDown = 1f;
        }
    }

    private void TargetPlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            coolDown = 1f;
        }
    }
}
