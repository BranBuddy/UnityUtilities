/*
    Scriptable Object for character creation.
    Currently it only holds info pertaining to the dialouge tree system/
*/

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [Header("Character Info")]
    [SerializeField]
    public string characterName;
    [Header("Character UI Sprites")]
    [SerializeField]
    public Sprite defaultCharacterSprite;
    [SerializeField]
    public Sprite happyCharacterSprite;
    [SerializeField]
    public Sprite angryCharacterSprite;

    [Header("Feeling Towards Player Attributes")]
    [SerializeField]
    public FeelingTowardPlayer defaultFeelingTowardPlayer;
    [SerializeField]
    public FeelingTowardPlayer feelingTowardPlayer;
    [SerializeField]
    public bool loadFeelingScore;
    [SerializeField]
    public int defaultFeelingScore;
    [SerializeField]
    public int feelingScore;

}

public enum FeelingTowardPlayer
{
    Neutral,
    Like,
    Love,
    Dislike,
    Hate    
}