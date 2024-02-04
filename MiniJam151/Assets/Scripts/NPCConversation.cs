using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCConversation : MonoBehaviour
{
    public string[] Conversations;
    public string[] Answers;
    public string[] Cost;
    public string[] Reward;
    public string[] Reply;
    [SerializeField] private Vector2 spawnPoint;
}
