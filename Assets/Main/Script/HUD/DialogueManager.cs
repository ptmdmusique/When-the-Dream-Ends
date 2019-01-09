using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public float textWaitTime = 0.1f;
    public Queue<string> sentences;
    public Text nameText;
    public Text dialogueText;

    public Transform dialogueBox;
    public Animator boxAnimator;

    private bool boxState = false;
    private bool finishTyping = true;
    private string curSentence;
    private float prevTimeScale = 1;
    private bool pauseDialogue = false;

    private void Start() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, bool pause) {
        if (pause == true) {        //Does dialogue require pause?
            pauseDialogue = pause;
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        boxState = true;            //BRING IN THE BOX!
        dialogueBox.GetComponent<Animator>().SetBool("Active", boxState);

        nameText.text = dialogue.name;  //Load the name
        sentences.Clear();
        
        foreach(string sentence in dialogue.sentences) {    //Load each sentences respectively
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() { //Display the freaking next sentence
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        curSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypingText(curSentence));
    }

    //Typing effect
    IEnumerator TypingText(string sentence) {
        dialogueText.text = "";
        finishTyping = false;

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textWaitTime);
        }

        finishTyping = true;
    }

    void EndDialogue() {
        boxState = false;
        dialogueBox.GetComponent<Animator>().SetBool("Active", boxState);

        if (pauseDialogue == true) {    //Unpause
            Time.timeScale = prevTimeScale;
        }
    }

    private void Update() {
        if (boxState == true) { //Press Enter to continue
            if (Input.GetKeyDown(KeyCode.Return) == true) {
                if (finishTyping == false) {
                    StopAllCoroutines();
                    dialogueText.text = curSentence;
                    finishTyping = true;
                } else {
                    DisplayNextSentence();
                }
            }
        }
    }
}
