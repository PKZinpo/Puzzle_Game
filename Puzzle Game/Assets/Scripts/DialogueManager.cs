using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text dialogueText;
    public bool isTyping;

    private Queue<string> sentences;

    void Awake() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        sentences.Clear();

        foreach (var sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayStartingSentence();
    }

    public void DisplayStartingSentence() {
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        GameObject.FindGameObjectWithTag("Tutorial").transform.GetChild(8).gameObject.SetActive(true);
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
        GameObject.FindGameObjectWithTag("Tutorial").transform.GetChild(8).gameObject.SetActive(false);
    }

    private IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.015f);
        }
        isTyping = false;
        if (!GameObject.FindGameObjectWithTag("Tutorial").transform.GetChild(3).gameObject.activeSelf) {
            GameObject.FindGameObjectWithTag("Tutorial").transform.GetChild(8).gameObject.SetActive(true);
        }
    }

    public void EndDialogue() {
        GameObject.FindGameObjectWithTag("Tutorial").SetActive(false);
    }
}
