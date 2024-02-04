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
    private bool isIntro;
    private bool isOutro;

    public int NPCNum;
    public int ConversationNum;

    private void Start()
    {
        GetComponent<TMP_Text>().text = "";

        // Select a random NPC and conversation.
        NPCNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs.Length);
        ConversationNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Conversations.Length);

        // Play that NPC's introduction.
        currentText = new string[1];
        currentText[0] = encounterEngine.GetComponent<Encounters>().NPCIntros[NPCNum];
        isIntro = true;
        isOutro = false;
        StartCoroutine(TypeText(currentText[0]));
    }

    private void Update()
    {
        // If there is nothing being typed, you click, and there are sufficient pages: continue.
        if(isTyping == false && Input.GetMouseButtonDown(0))
        {
            if (pageNumber < currentText.Length)
            {
                StartCoroutine(TypeText(currentText[pageNumber]));
            }
        }  
    }

    public void Reply(bool retort = false)
    {
        string reward = encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Reward[ConversationNum];

        if (retort == false)
        {
            currentText = new string[4];
            if (reward[0] == '$')
            {
                currentText[0] = "(" + reward[1..] + " GOLD is added to your hoard)";
            }
            else
            {
                currentText[0] = "(You obtain " + reward + ")";
            }
            currentText[1] = encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Reply[ConversationNum];
            currentText[2] = encounterEngine.GetComponent<Encounters>().NPCOutros[NPCNum];
            currentText[3] = "...";
        } else
        {
            currentText = new string[3];

            currentText[0] = encounterEngine.GetComponent<Encounters>().NPCRetorts[NPCNum];
            currentText[1] = encounterEngine.GetComponent<Encounters>().NPCOutros[NPCNum];
            currentText[2] = "...";
        }
        
        isIntro = false;
        isOutro = true;
        StartCoroutine(TypeText(currentText[0]));
    }

    public IEnumerator TypeText(string page)
    {
        // Set up box.
        arrow.SetActive(false);
        char[] textArray = page.ToCharArray();
        GetComponent<TMP_Text>().text = "";
        isTyping = true;

        // Actual typing animation.
        foreach (char c in textArray)
        {
            GetComponent<TMP_Text>().text += c;
            yield return new WaitForSeconds(0.05f);
        }

        // Increase to the next page and then if it is finished, activate the appropriate answers.
        pageNumber++;
        if(pageNumber >= currentText.Length && isIntro == false && isOutro == false)
        {
            answers.SetActive(true);
            gift.SetActive(false);
            trade.SetActive(false);
            fight.SetActive(false);
            foreach (string s in encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Answers[ConversationNum].Split('$'))
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
            // Then reset the page number for the next conversation.
            pageNumber = 0;
        } else
        {
            // Otherwise pause and wait to continue.
            isTyping = false;
            arrow.SetActive(true);
            // If this is an intro, then move to the appropriate conversation.
            if(isIntro)
            {
                pageNumber = 0;
                isIntro = false;
                isOutro = false;
                currentText = encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Conversations[ConversationNum].Split('$');
            }
            // If this is an outro, then reset the conversation and start again
            if(isOutro && pageNumber >= currentText.Length)
            {
                pageNumber = 0;
                isIntro = true;
                isOutro = false;
                NPCNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs.Length);
                ConversationNum = UnityEngine.Random.Range(0, encounterEngine.GetComponent<Encounters>().NPCs[NPCNum].GetComponent<NPCConversation>().Conversations.Length);
                currentText = new string[1];
                currentText[0] = encounterEngine.GetComponent<Encounters>().NPCIntros[NPCNum];
            }
        }
    }
}
