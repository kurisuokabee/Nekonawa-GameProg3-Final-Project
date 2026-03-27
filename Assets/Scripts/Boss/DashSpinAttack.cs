using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSpinAttack : BossState
{   
    protected Lucifer_SM stateMachine;

    float fireRate = .2f;
    float soundRate = .4f;
    float lastFireTime;
    float lastFireTime1;
    float attackDuration = 1f;
    bool isFacingPlayer = false;

    bool Dash = true;

    float rotationSpeed = 225f; // Adjust the speed as needed
    bool isRotating = false;

    float totalRotation = 0f;

    float rotDirection;
    int count;

    void Awake()
    {   
        stateMachine = GetComponent<Lucifer_SM>();
    }

    public override void Enter()
    {
        Debug.Log("Boss enters Attack state");
        attackDuration = Random.Range(10, 16);

        count = 1;
        rotDirection = -1;
        isFacingPlayer = false; 
        Dash = false;
        isRotating = true;
        rotationSpeed = 180f;
        totalRotation = 0f;


        stateMachine.firePoints = stateMachine.TrianglePoints();
    }

    public override void Action()
    {   
    
        if(Dash)
        {
            DashMove();
        }

        if (isRotating)
        {
            SpinAttack();
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
                
                isFacingPlayer = true;
            
            }
        }
        
        if (isFacingPlayer)
        {
            Vector2 moveDirection = (stateMachine.presentPlayerPos - transform.position).normalized;
            stateMachine.boss_rb.linearVelocity = moveDirection * (stateMachine.moveSpeed + 15);

            if (DistanceToPlayer() <= 3)
            {   
                stateMachine.boss_rb.linearVelocity = Vector2.zero;
                Dash = false;
                isRotating = true;
            }
        }

    }

    void SpinAttack()
    {
        // Rotate the object around the z-axis
        transform.Rotate(Vector3.forward * (rotationSpeed * rotDirection) * Time.deltaTime);

        if (Time.time - lastFireTime >= fireRate)
        {
            stateMachine.DashSpinAttack();
            lastFireTime = Time.time;
        }

        if (Time.time - lastFireTime1 >= soundRate)
        {
            lastFireTime1 = Time.time;
        }

        totalRotation += Mathf.Abs(rotationSpeed) * Time.deltaTime;

        if (totalRotation >= 360f)
        {
            // Stop rotating
            isRotating = false;
            Dash = true;
            isFacingPlayer = false;
            totalRotation = 0;
        
            if (count == 1)
            {   
                
                rotDirection = 1;
                count++;
            }
            else
            {   
                
                rotDirection = -1;
                count--;
            }
        }
    }

    

    float  DistanceToPlayer()
    {
        return Vector3.Distance(stateMachine.presentPlayerPos, transform.position);
    }
    
}
