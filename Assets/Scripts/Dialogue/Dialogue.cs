using UnityEngine;

[System.Serializable]
public struct Sentence
{
    public Character character;
    public string text;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Sentence[] sentences;
}
