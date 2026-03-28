using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : BossState
{   
    protected Lucifer_SM stateMachine;

    float fireRate = .4f;
    float lastFireTime;
    float attackDuration = 1f;
    float dashDuration = 2f;
    bool isFacingPlayer = false;
    Vector2 moveDirection;
  
    void Awake()
    {   
        stateMachine = GetComponent<Lucifer_SM>();
    }

    public override void Enter()
    {
        Debug.Log("Boss enters Attack state");

        attackDuration = Random.Range(10, 16);

        isFacingPlayer = false;

        stateMachine.firePoints = stateMachine.TrianglePoints();
    }


    public override void Action()
    {
        DashMove();

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
            stateMachine.LookAtPlayer(10);
            

            if (shouldBeFacingPlayer)
            {
                // Store the last known position of the player when the arrow starts facing the player
                
                stateMachine.presentPlayerPos = stateMachine.enemy.player.transform.position;
                moveDirection = (stateMachine.presentPlayerPos - transform.position).normalized;

                dashDuration = 2f;
                

                isFacingPlayer = true;
            }
        }
        
        if (isFacingPlayer)
        {
            stateMachine.boss_rb.linearVelocity = moveDirection * (stateMachine.moveSpeed + 5);

            if (Time.time - lastFireTime >= fireRate)
            {   
                stateMachine.DashAttack();
                lastFireTime = Time.time;
            }  
    
            dashDuration  -= Time.deltaTime;
            if (dashDuration  < 0f)
            {   
                stateMachine.boss_rb.linearVelocity = Vector2.zero;
                isFacingPlayer = false;
            }   
            
        }

    }

}
