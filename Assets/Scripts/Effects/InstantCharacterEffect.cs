using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int instantEffectD;

    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
