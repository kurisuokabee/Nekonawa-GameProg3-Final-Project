
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : BossState
{   
    protected Lucifer_SM stateMachine;

    float fireRate = 1f;
    float lastFireTime;
    float attackDuration = 1f;

    float startTime;
    float initialRotationZ;

    float rotationSpeed = 3f; // Adjust the speed of oscillation as needed
    float amplitude = 180f;

    bool isAtPeak = false;
    bool isAtMiddle = false;

    void Awake()
    {   
        stateMachine = GetComponent<Lucifer_SM>();
    }
   
    public override void Enter()
    {
        Debug.Log("Boss enters Attack state");

        startTime = Time.time;
        attackDuration = Random.Range(10, 16);

        initialRotationZ = transform.rotation.eulerAngles.z;
       


        stateMachine.firePoints = stateMachine.TrianglePoints();

        
    }

    public override void Action()
    {
        Oscillate();

        if (Time.time - lastFireTime >= fireRate)
        {
            StartCoroutine(stateMachine.ArrowAttack());
            lastFireTime = Time.time;
        }

        attackDuration -= Time.deltaTime;
        if (attackDuration < 0f)
        {
            stateMachine.ChangeState<Lucifer_DecideState>();
        }
    }

    private void Oscillate()
    {
        // Calculate the time-dependent oscillation
        float oscillation = amplitude * Mathf.Sin((Time.time - startTime) * rotationSpeed);


        // Apply the rotation to the object based on the initial rotation and oscillation
        transform.rotation = Quaternion.Euler(0f, 0f, initialRotationZ + oscillation);

        // Check if the oscillation is at its peak (1 or -1)
        if (Mathf.Abs(Mathf.Sin((Time.time - startTime) * rotationSpeed)) >= 0.99f && !isAtPeak)
        {
            
            isAtPeak = true;
        }
        else if (Mathf.Abs(Mathf.Sin((Time.time - startTime) * rotationSpeed)) < 0.99f)
        {
            isAtPeak = false; // Reset the peak flag
        }

         // Check if the oscillation is at its middle (0)
        if (Mathf.Abs(Mathf.Sin((Time.time - startTime) * rotationSpeed)) < 0.01f && !isAtMiddle)
        {
            
            isAtMiddle = true;
        }
        else if (Mathf.Abs(Mathf.Sin((Time.time - startTime) * rotationSpeed)) >= 0.01f)
        {
            isAtMiddle = false; // Reset the middle flag
        }

    }

    public override void Exit()
    {
        stateMachine.firePoints.Clear();

        stateMachine.PickAnAttack();
    }

    
    
    
}


    

