using UnityEngine;
using System.Collections.Generic;

public class CirclePuzzle : MonoBehaviour
{
    public GameObject[] puzzleObjects; // Assign the 4 interactables
    private int[] correctSequence = { 0, 1, 2, 3 };
    private List<int> currentSequence = new List<int>();

    // Call this method when an object is activated 
    public void ActivateObject(int index)
    {
        if (currentSequence.Count > 0 && currentSequence[currentSequence.Count - 1] == index)
        {
            // Pressed the same object twice, reset the sequence
            currentSequence.Clear();
            Debug.Log("Sequence reset: Same object pressed twice.");

            ResetObjectStates();
        }
        else
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
                    currentSequence.Clear();
                    Debug.Log("Wrong sequence. Resetting.");
                    ResetObjectStates();
                }
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

    
    void Update()
    {
        // Example: Press keys 1-4 to simulate activation
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateObject(i);
            }
        }
    }
}
