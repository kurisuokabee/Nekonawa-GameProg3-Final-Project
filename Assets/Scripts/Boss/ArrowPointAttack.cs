using UnityEngine;

public class ArrowPointAttack : BossState
{
    protected Lucifer_SM stateMachine;

    float fireRate = .3f;
    float lastFireTime;
    float attackDuration = 1f;

    Vector2[] chosenPath;
    int currentPointIndex = 0;
    
    void Awake()
    {   
        stateMachine = GetComponent<Lucifer_SM>();
    }

    public override void Enter()
    {
        Debug.Log("Boss enters Attack state");
        attackDuration = Random.Range(10, 16);

        stateMachine.firePoints = stateMachine.TrianglePoints();

        
        // Define three different triangle paths
        Vector2[] path1 = new Vector2[]
        {
            transform.position,
            transform.position + new Vector3(7, 0),
            transform.position + new Vector3(3.5f, 6.06f),
            transform.position
        };

        Vector2[] path2 = new Vector2[]
        {
            transform.position,
            transform.position + new Vector3(0, 7),
            transform.position + new Vector3(-6.06f, 3.5f),
            transform.position
        };

        Vector2[] path3 = new Vector2[]
        {
            transform.position,
            transform.position + new Vector3(-7, 0),
            transform.position + new Vector3(-3.5f, -6.06f),
            transform.position
        };

        // Choose a random path at the start
        int randomPathIndex = Random.Range(0, 3);
        switch (randomPathIndex)
        {
            case 0:
                chosenPath = path1;
                break;
            case 1:
                chosenPath = path2;
                break;
            case 2:
                chosenPath = path3;
                break;
        }


        stateMachine._Point = stateMachine.Point();
    }

    public override void Action()
    {   
        MoveInTriangle();
        stateMachine.LookAtPlayer(10);

        if (Time.time - lastFireTime >= fireRate)
        {   
            StartCoroutine(stateMachine.ArrowPointAttack());
            lastFireTime = Time.time;
        }

        attackDuration -= Time.deltaTime;
        if (attackDuration < 0f)
        {   
            stateMachine.ChangeState<Lucifer_DecideState>();
        }
    }

    

    public override void Exit()
    {
       stateMachine.firePoints.Clear();

       stateMachine.PickAnAttack();
    }

    void MoveInTriangle()
    {
        // Move towards the current point in the chosen triangle path
        Vector2 targetPoint = chosenPath[currentPointIndex];
        Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
        stateMachine.boss_rb.linearVelocity = direction * stateMachine.moveSpeed;

        // Check if the GameObject is close enough to the current point
        float distance = Vector2.Distance(transform.position, targetPoint);
        if (distance < 0.1f)
        {   
            // Move to the next point in the chosen triangle path
            currentPointIndex = (currentPointIndex + 1) % chosenPath.Length;
        }
    }
}
