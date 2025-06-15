using UnityEngine;

public class Enums : MonoBehaviour
{

}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum CharacterGroup
{
    Team_01,
    Team_02, 

}

public enum WeaponModeSLot
{
    RightHand,
    LeftHand,
    // Right hips
    // Left hips
    // Back
}

// this is used to calculate damage based on attackType
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    LightAttack03,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
}

public enum TreeType
{
    Tree,
    Log,
    Coc
}

public enum PLayerAction
{
    Normal,
    ChopTree,
    CarrySomething,
    LogSharpening,
    PlayingDialogue

}
