using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButtons : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject encounterEngine;
    [SerializeField] private GameObject answers;
    private AutoTyper textScript;
    
    private string cost;
    private string reward;

    private void Start()
    {
        textScript = textBox.GetComponent<AutoTyper>();
    }

    public void Gift()
    {
        // Find cost of gift
        cost = encounterEngine.GetComponent<Encounters>().NPCs[textScript.NPCNum].GetComponent<NPCConversation>().Cost[textScript.ConversationNum];

        // Is the cost gold?
        if (cost[0] == '$')
        {
            // Make the cost an integer
            if (Int32.TryParse(cost[1..], out int numCost))
            {
                // If you can afford, take the gold
                if(State.Gold >= numCost)
                {
                    State.Gold -= numCost;
                    textScript.Reply();
                } else
                {
                    textScript.Reply(true);
                }
            }
        } else
        {
            // ITEM GIVING
            Debug.Log("Gave item.");
        }

        answers.SetActive(false);
    }

    public void Trade()
    {
        cost = encounterEngine.GetComponent<Encounters>().NPCs[textScript.NPCNum].GetComponent<NPCConversation>().Cost[textScript.ConversationNum];
        reward = encounterEngine.GetComponent<Encounters>().NPCs[textScript.NPCNum].GetComponent<NPCConversation>().Reward[textScript.ConversationNum];

        // Is the cost gold?
        if (cost[0] == '$')
        {
            // Make the cost an integer
            if (Int32.TryParse(cost[1..], out int numCost))
            {
                // If you can afford
                if (State.Gold >= numCost)
                {
                    // Take gold and give reward
                    State.Gold -= numCost;
                    if (reward[0] == '$')
                    {
                        if (Int32.TryParse(reward[1..], out int numReward))
                        {
                            State.Gold += numReward;
                        }
                        textScript.Reply();
                    } else
                    {
                        State.Items.Add(reward);
                        textScript.Reply();
                    }
                } else
                {
                    textScript.Reply(true);
                }
            }
        }
        else
        {
            // TAKE ITEM
            Debug.Log("Took item.");
            if (reward[0] == '$')
            {
                if (Int32.TryParse(reward[1..], out int numReward))
                {
                    State.Gold += numReward;
                }
                textScript.Reply();
            }
            else
            {
                State.Items.Add(reward);
                textScript.Reply();
            }
        }

        answers.SetActive(false);
    }

    public void Fight()
    {

    }
}
