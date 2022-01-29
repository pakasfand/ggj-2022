using TMPro;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Character", menuName = "DialogueSystem/Character")]
    public class Character : ScriptableObject
    {
        public string CharacterName;
        public TMP_FontAsset font;
        public AudioClip[] voices;
        public Sprite portrait;
    }
}
