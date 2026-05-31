/*
    Scriptable Object for the dialouge tree system.
    The designer can set up the flow of the dialouge with its own response text, response connotation, the character's reaction to the response
    and whether the conversation ends there.
*/

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Individual Piece of dialouge
[System.Serializable]
public class DialougeNode
{
    [TextArea(3, 10), SerializeField]
    public string text;
    [SerializeField]
    public DialougePlayerResponse[] responses;

    [HideInInspector]
    public int responseIndex;

}

// Players reaction to its master dialouge node
[System.Serializable]
public class DialougePlayerResponse
{
    [Multiline, SerializeField]
    public string text;
    [SerializeField]
    public Connotation connotation;

    [Multiline, SerializeField]
    public string optionialResponseText;

    [SerializeField]
    public int responseIndex;

    [SerializeField]
    public bool endConversation = false;

    [SerializeField]
    public ButtonEvents[] buttonEvents;

}

[System.Serializable]
public class ButtonEvents
{
    public UnityEvent buttonEvent;
    internal string eventName = "fuck";
}

public class DialougeTreeSO : MonoBehaviour
{
   [SerializeField, Tooltip("This is the dialouge tree and create as many as needed")] public DialougeNode[] dialougeTree;

    [Header("One Time Conversation Attributes")]
   [SerializeField, Tooltip("If the full conversation should only happen once check this")] public bool oneTimeConversation;
   [SerializeField, HideInInspector] public bool conservationSpent;

   [SerializeField, HideInInspector] public int finalNode;
}

public enum Connotation
{
    Positive,
    Negative,
    Neutral
}