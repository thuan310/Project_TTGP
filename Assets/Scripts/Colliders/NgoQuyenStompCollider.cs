using UnityEngine;

public class NgoQuyenStompCollider : DamageCollider
{
    [SerializeField] AINgoQuyenCharacterManager ngoQuyenCharacterManager;

    protected override void Awake()
    {
        base.Awake();

        ngoQuyenCharacterManager = GetComponentInParent<AINgoQuyenCharacterManager>();

    }
    public void StompAttack()
    {
        charactersDamaged.Clear();
        GameObject stompVFX = Instantiate(ngoQuyenCharacterManager.ngoQuyenCombatManager.durkImpactVFX, transform);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ngoQuyenCharacterManager.ngoQuyenCombatManager.stompAttackAOERadius, WorldUtilityManager.instance.GetCharacterLayers());
        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();
            //print(collider.name);
            //print(character.name);

            if (character != null)
            {
                if(charactersDamaged.Contains(character))
                    continue;

                // we dont want durk to hurt himself when stomps
                if(character == ngoQuyenCharacterManager)
                    continue;

                charactersDamaged.Add(character);
            }

            // we only process damage if the character "Isowner" so that they only get damage if the collider connects on the clients
            // meaning if you are hit on the host screen but not on your own, you will not be hit

            // check ofr block
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = ngoQuyenCharacterManager.ngoQuyenCombatManager.stompDamage;
            damageEffect.poiseDamage = ngoQuyenCharacterManager.ngoQuyenCombatManager.stompDamage;

            character.characterEffectManager.ProcessInstantEffect(damageEffect);
        }
}
}
