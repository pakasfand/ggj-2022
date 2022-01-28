using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "DialogueSystem/Character")]
public class Character : ScriptableObject
{
    public string CharacterName;
    public Font font;
    public AudioClip[] voices;
    public Sprite portrait;
}
