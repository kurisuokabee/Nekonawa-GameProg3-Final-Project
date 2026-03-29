using UnityEngine;

public class Lucifer_DecideState : BossState
{   
    protected Lucifer_SM stateMachine;
    float delay = 2f;
    private bool isFacingPlayer;

    void Awake()
    {
        stateMachine = GetComponent<Lucifer_SM>();
    }
    public override void Action()
    {
        delay -= Time.deltaTime;
        if (delay < 0f)
        {   
            DashMove();
        }

        if (stateMachine.canChangeAttack)
        {   
            ChangeAttackState();
            stateMachine.canChangeAttack = false;
        }
    }

    public override void Enter()
    {
        Debug.Log("Boss Idle");
        delay = 2f;
        isFacingPlayer = false;
     
        stateMachine.boss_rb.linearVelocity = Vector2.zero;
        stateMachine.firePoints = stateMachine.TrianglePoints();
    }

    public override void Exit()
    {
        stateMachine.firePoints.Clear();
        stateMachine.DestroyAllChildren();
    }

    float  DistanceToPlayer()
    {
        return Vector3.Distance(stateMachine.presentPlayerPos, transform.position);
    }

    void DashMove()
    {   
        stateMachine.lookPlayerPos = stateMachine.enemy.player.transform.position;
        Vector2 lookDirection = (stateMachine.lookPlayerPos - transform.position).normalized;
        float facingThreshold = 0.99f;

        // Calculate the dot product between the arrow's forward vector and the direction to the player
        float dotProduct = Vector3.Dot(transform.up, lookDirection);
        // Check if the arrow is facing the player based on the threshold
        bool shouldBeFacingPlayer = dotProduct >= facingThreshold;

        if (!isFacingPlayer)
        {
            stateMachine.LookAtPlayer(5);

            if (shouldBeFacingPlayer)
            {
                
                stateMachine.presentPlayerPos = stateMachine.enemy.player.transform.position;
                isFacingPlayer = true;
                stateMachine.StationaryAttack();
            }
        }
        
        if (isFacingPlayer)
        {   
            Vector2 moveDirection = (stateMachine.presentPlayerPos - transform.position).normalized;
            stateMachine.boss_rb.linearVelocity = moveDirection * (stateMachine.moveSpeed + 15);

            if (DistanceToPlayer() <= 2)
            {   
                stateMachine.boss_rb.linearVelocity = Vector2.zero;
                stateMachine.canChangeAttack = true;

                stateMachine.StationaryAttack();
            }
        }
        
    }

    void ChangeAttackState()
    {
        switch(stateMachine.AttackName)
        {
            case "ArrowAttack":
                stateMachine.ChangeState<ArrowAttack>();
            break;

            case "ArrowPointAttack":
                stateMachine.ChangeState<ArrowPointAttack>();
            break;

            case "DashSpinAttack":
                stateMachine.ChangeState<DashSpinAttack>();
            break;

            case "DashAttack":
                stateMachine.ChangeState<DashAttack>();
            break;
        }
    }
}
