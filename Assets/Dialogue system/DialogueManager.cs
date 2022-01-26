using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private Queue <string> sentences;

    [SerializeField] Text NameText;
    [SerializeField] Text DialogueText;
    [SerializeField] Font NameFont;
    [SerializeField] Font DialogueFont;
    [SerializeField] float TypingSpeed;
    [SerializeField] float TimeUntilTextAppears;

    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        
    }

    public void StartDialogue(Dialogue dialogue)

    {
        Time.timeScale = 0;

        anim.SetBool("isOpen", true);

        Debug.Log("Starting dialogue of: " + dialogue.name);

        StartCoroutine(CheckAnimationCompeted(dialogue));
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentence);
    }

    private IEnumerator CheckAnimationCompeted(Dialogue dialogue)
    {
        yield return new WaitForSecondsRealtime(TimeUntilTextAppears);
        NameText.text = dialogue.name;
        NameText.font = NameFont;
        DialogueText.font = DialogueFont;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    private IEnumerator TypeSentence (string sentence)
    {
        DialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSecondsRealtime(TypingSpeed);
        }
    }

    void EndDialogue()
    {
        anim.SetBool("isOpen", false);
        DialogueText.text = "";
        NameText.text = "";
        Time.timeScale = 1;
        Debug.Log("End of Conversation");
    }
}
