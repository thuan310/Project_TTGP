using UnityEngine;
using TMPro;

public class UI_Boss_HP_bar : UI_StatBar
{
    [SerializeField] AIBossCharacterManager bossCharacter;

    public void EnableBossHpBar(AIBossCharacterManager aiBoss)
    {
        bossCharacter = aiBoss;
        bossCharacter.currentHealth.OnValueChanged += OnBossHPChanged;
        SetMaxStat(bossCharacter.maxHealth.Value);
        SetStat(bossCharacter.currentHealth.Value);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
    }
    private void OnDestroy()
    {
        bossCharacter.currentHealth.OnValueChanged -= OnBossHPChanged;
    }

    public void OnBossHPChanged(int oldValue, int newValue)
    {
        SetStat(newValue);

        if(newValue <= 0)
        {
            RemoveHPBar(2.5f);
        }
    }

    public void RemoveHPBar(float time)
    {
        Destroy(gameObject, time);
    }
}
