using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUIManager : MonoBehaviour
{   
    public static  DialogueUIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public DialoguePanel CreateNPCDialoguePanel(Transform npcTransform, NPCData npc, Action endTalk)
    {
        GameObject panelObj = Utilities.Factory.SpawnObject(
            ObjectType.Dialogue,
            npcTransform.position + Vector3.up * 3,
            Quaternion.identity
        );

        DialoguePanel panel = panelObj.GetComponent<DialoguePanel>();
        panel.npcNameText.text = npc.NPCName;
        panel.npcImage.sprite = npc.NPCImage;

        panel.followTarget = npcTransform;

        panel.interactbutton.onClick.RemoveAllListeners();
        panel.interactbutton.interactable = false;
        panel.interactbutton.gameObject.SetActive(false);

        panel.exitbutton.gameObject.SetActive(true);
        panel.exitbutton.onClick.RemoveAllListeners();
        panel.exitbutton.onClick.AddListener(() => endTalk?.Invoke());

        return panel;
    }

    public DialoguePanel CreateStoryDialoguePanel(Transform playerTransform)
    {   
        GameObject panelObj = Utilities.Factory.SpawnObject(
            ObjectType.Dialogue,
            playerTransform.position + Vector3.up * 3,
            Quaternion.identity
        );

        DialoguePanel panel = panelObj.GetComponent<DialoguePanel>();
        panel.followTarget = Utilities.Player.transform;

        panel.interactbutton.onClick.RemoveAllListeners();
        panel.interactbutton.interactable = false;
        panel.interactbutton.gameObject.SetActive(false);

        panel.exitbutton.gameObject.SetActive(false);
    
        return panel;
    }

    public void InitInteractButton(MonoBehaviour owner, DialoguePanel panel, Action functionToCall, string buttonText)
    {
        owner.StartCoroutine(WaitForDialogueThenEnableButton(panel, functionToCall, buttonText));
    }

    private IEnumerator WaitForDialogueThenEnableButton(DialoguePanel panel, Action functionToCall, string buttonText)
    {
        yield return new WaitUntil(() => panel.DialogueFinished);

        panel.interactbutton.gameObject.SetActive(true);
        panel.interactbutton.onClick.RemoveAllListeners();
        panel.interactbutton.onClick.AddListener(() => functionToCall?.Invoke());
        panel.interactbutton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        panel.interactbutton.interactable = true;
    }
}
