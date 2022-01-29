using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public struct Sentence
    {
        public Character character;
        public string text;
    }

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
    public class DialogueInstance : ScriptableObject
    {
        public Sentence[] sentences;
    }
}