using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int characterID;
    public Sprite characterSprite;
    public AttackType attackType; // SlotMachine, Dice, etc.

    public enum AttackType { SlotMachine, DiceRolling }
}
