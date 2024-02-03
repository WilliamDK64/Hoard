using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoTyper : MonoBehaviour
{
    [SerializeField] private GameObject encounterEngine;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject answers;
    [SerializeField] private GameObject gift;
    [SerializeField] private GameObject trade;
    [SerializeField] private GameObject fight;
    private string[] currentText;
    private bool isTyping = false;
    private int pageNumber = 0;

    private int NPCNum;
    private int ConversationNum;

    private void Start()
    {
        GetComponent<TMP_Text>().text = "";

        NPCNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs.Length);
        ConversationNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Conversations.Length);

        currentText = new string[1];
        currentText[0] = encounterEngine.GetComponent<Encounters>().NPCIntros[NPCNum];
        StartCoroutine(TypeText(currentText[0], true));
    }

    private void Update()
    {
        if(isTyping == false && Input.GetMouseButtonDown(0))
        {
            if (pageNumber < currentText.Length)
            {
                StartCoroutine(TypeText(currentText[pageNumber], false));
            }
        }  
    }

    IEnumerator TypeText(string page, bool isIntro)
    {
        arrow.SetActive(false);
        char[] textArray = page.ToCharArray();
        GetComponent<TMP_Text>().text = "";
        isTyping = true;

        foreach (char c in textArray)
        {
            GetComponent<TMP_Text>().text += c;
            yield return new WaitForSeconds(0.05f);
        }

        pageNumber++;
        if(pageNumber >= currentText.Length && isIntro == false)
        {
            answers.SetActive(true);
            foreach(string s in encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Answers[ConversationNum].Split('$'))
            {
                if(s == "Gift")
                {
                    gift.SetActive(true);
                }
                if(s == "Trade")
                {
                    trade.SetActive(true);
                }
                if(s == "Fight")
                {
                    fight.SetActive(true);
                }
            }
            pageNumber = 0;
        } else
        {
            isTyping = false;
            arrow.SetActive(true);
            if(isIntro)
            {
                pageNumber = 0;
                currentText = encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Conversations[ConversationNum].Split('$');
            }
        }
    }
}
