using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CirclePuzzle : MonoBehaviour
{
    public GameObject[] puzzleObjects; // Assign the 4 interactables
    private int[] correctSequence = { 0, 1, 2, 3 };
    private List<int> currentSequence = new List<int>();

    public GameObject rewardObject; 
    public GameObject enemyPrefab; 
    public InputActionReference interactAction; 
    private int activateIndex = 0;
    public float rewardSpeed = 5f; 
    public float enemySpawnRadius = 3f; 
    private GameObject movingReward;

    // Call this method when an object is activated 
    public void ActivateObject(int index)
    {
        currentSequence.Add(index);
        Debug.Log($"Object {index} activated. Current sequence: {string.Join(", ", currentSequence)}");

        if (currentSequence.Count == 4)
        {
            if (CheckSequence())
            {
                Win();
            }
            else
            {
                if (enemyPrefab != null)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        float angle = (i * 360f / 4) * Mathf.Deg2Rad; 
                        Vector3 offset = new Vector3(Mathf.Cos(angle) * enemySpawnRadius, 0, Mathf.Sin(angle) * enemySpawnRadius);
                        GameObject enemy = Instantiate(enemyPrefab, transform.position + offset, Quaternion.identity);
                        enemy.SetActive(true); 
                    }
                }
                currentSequence.Clear();
                Debug.Log("Wrong sequence. Resetting.");
                ResetObjectStates();
            }
        }
    }

    private bool CheckSequence()
    {
        for (int i = 0; i < 4; i++)
        {
            if (currentSequence[i] != correctSequence[i])
            {
                return false;
            }
        }
        return true;
    }

    private void Win()
    {
        Debug.Log("You win! Correct sequence entered.");
        if (rewardObject != null && movingReward == null)
        {
            movingReward = rewardObject;
        }
        currentSequence.Clear();
        ResetObjectStates();
    }

    private void ResetObjectStates()
    {
        foreach (GameObject obj in puzzleObjects)
        {
            if (obj != null)
            {
                // Assuming objects have SpriteRenderer or similar
                var renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = Color.white; 
                }
            }
        }
    }

    void Start()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
        }
    }
    
    void Update()
    {
        if (interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Find the closest puzzle object to the player
                int closestIndex = -1;
                float closestDistance = float.MaxValue;
                for (int i = 0; i < puzzleObjects.Length; i++)
                {
                    if (puzzleObjects[i] != null)
                    {
                        float distance = Vector3.Distance(player.transform.position, puzzleObjects[i].transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestIndex = i;
                        }
                    }
                }
                if (closestIndex != -1)
                {
                    ActivateObject(closestIndex);
                }
            }
        }

        // Move reward toward player
        if (movingReward != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 direction = (player.transform.position - movingReward.transform.position).normalized;
                movingReward.transform.position += direction * rewardSpeed * Time.deltaTime;
            }
        }
    }
}
