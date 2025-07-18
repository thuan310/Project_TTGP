using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("VFX")]

    public GameObject bloodSplatterVFX;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;
    [SerializeField] List<InstantCharacterEffect> instantEffects;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void GenerateEffectIDs()
    {
        for(int i =0; i<instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectD = i;
        }
    }
}
