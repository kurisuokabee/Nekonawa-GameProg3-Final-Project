using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DialoguePanel : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime0_5 = new WaitForSecondsRealtime(0.5f);
    [Header("UI References")]
    public TextMeshProUGUI npcNameText;
    public Image npcImage;
    public TextMeshProUGUI dialogueText;
    public Button interactbutton;
    public Button exitbutton;

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private List<string> sentences;
    private int currentIndex = 0;

    private Coroutine typingCoroutine;
    public bool isTyping = false;
    private bool canClick = false;
    public bool DialogueFinished { get; private set; }
    public Transform followTarget = null;

    [SerializeField] private Vector3 offset = Vector3.up * 3;
    
    public void SetDialogue(List<string> newSentences)
    {
        sentences = newSentences;
        currentIndex = 0;
        DialogueFinished = false;

        ShowSentence();
        StartCoroutine(EnableClickDelay());
    }

    //Click delay to avoid skipping dialogue from double clicks
    IEnumerator EnableClickDelay()
    {
        canClick = false;
        yield return _waitForSecondsRealtime0_5;
        canClick = true;
    }

    
    void Update()
    {   
        if (followTarget != null)
        {
            transform.position = followTarget.position + offset;
        }

        if (!canClick) return;

        if (Input.GetMouseButtonDown(0))
        {
            HandleDialogueClick();
        }
    }

    //Left click to continue dialogue
    public void HandleDialogueClick()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = sentences[currentIndex];
            isTyping = false;
        }
        else
        {
            if (currentIndex >= sentences.Count - 1)
            {   
                DialogueFinished = true;
                return;
            } 
            
            currentIndex++;
            ShowSentence();
        }
    }

    //Typing animation
    void ShowSentence()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentences[currentIndex]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (currentIndex >= sentences.Count - 1)
        {
            DialogueFinished = true;
        }

        isTyping = false;

        
    }

    
}