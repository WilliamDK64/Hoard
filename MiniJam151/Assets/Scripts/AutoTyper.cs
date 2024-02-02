using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoTyper : MonoBehaviour
{
    [SerializeField] private GameObject encounterEngine;
    private string[] currentText;
    private bool isTyping = false;
    private int pageNumber = 0;

    private void Start()
    {
        currentText = encounterEngine.GetComponent<Encounters>().NPCs[0].GetComponent<NPCConversation>().ConversationText;
        GetComponent<TMP_Text>().text = "";
        StartCoroutine(TypeText(currentText[pageNumber]));
    }

    private void Update()
    {
        if(isTyping == false && Input.GetMouseButtonDown(0))
        {
            if (pageNumber < currentText.Length)
            {
                StartCoroutine(TypeText(currentText[pageNumber]));
            }
        }  
    }

    IEnumerator TypeText(string page)
    {
        char[] textArray = page.ToCharArray();
        GetComponent<TMP_Text>().text = "";
        isTyping = true;

        foreach (char c in textArray)
        {
            GetComponent<TMP_Text>().text += c;
            yield return new WaitForSeconds(0.05f);
        }

        pageNumber++;
        isTyping = false;
    }
}
