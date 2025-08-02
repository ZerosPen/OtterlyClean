using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image character;
    public TextMeshProUGUI nameChar;
    public TextMeshProUGUI dialogueText;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float typingSpeed;
    private Animator animator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        animator = GetComponent<Animator>();
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        if (animator != null)
        {
            animator.Play("show");
        }

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        character.sprite = currentLine.character.icon;
        nameChar.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueText.text = "";
        foreach (char letter in dialogueLine.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void GamePause()
    {
        animator.Play("hide");
    }

    public void GameContinue()
    {
        animator.Play("show");
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");
    }
}
