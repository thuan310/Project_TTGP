using NUnit.Framework.Constraints;
using UnityEngine;


    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingManager; // when calculating damage this is used to check for attackedrs damage for attackers damage modifiers, effect ect

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;
        public float light_Attack_02_Modifier;
        public float light_Attack_03_Modifier;
        public float heavy_Attack_01_Modifier;
        public float heavy_Attack_02_Modifier;
        public float charge_Attack_01_Modifier;
        public float charge_Attack_02_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false; // melee weapon colliders should be disabled at start, only enabled when animations allow

        }
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            //print(damageTarget.name);

            if (damageTarget != null)
            {
                // we do not want to damage ourselves
                if (damageTarget == characterCausingManager)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // check if we can damage this target based on frindly fire

                // check if target is blocking

                //check if target is vulnerable

                // damage
                //print(damageTarget.name);
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damgageTarget)
        {
            // we don't want to damage the same target more than once in a single attack
            // so we add them to a list that checks before applying damage
            if (charactersDamaged.Contains(damgageTarget))
            {
                return;
            }
            charactersDamaged.Add(damgageTarget);
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingManager.transform.forward, damgageTarget.transform.forward, Vector3.up);

            switch (characterCausingManager.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApllyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApllyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack03:
                    ApllyAttackDamageModifiers(light_Attack_03_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApllyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApllyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApllyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApllyAttackDamageModifiers(charge_Attack_02_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            damgageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
        }

        private void ApllyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;

            // if attack is a fully charged heavy, multiply by full charge modifier after normal modifier have been calculated

        }
    }

