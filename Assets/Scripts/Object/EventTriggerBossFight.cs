using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int bossID;
    public AIBossCharacterManager boss;

    private void OnTriggerEnter(Collider other)
    { 
         boss =WorldAIManager.instance.GetBossCharacterByID(bossID);

        if(boss != null)
        {
            print("WakeBoss");
            boss.WakeBoss();
        }
    }
}
