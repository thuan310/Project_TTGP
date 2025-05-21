using UnityEngine;

public class VillagerNPC : MonoBehaviour {
    public GameObject LLMUI;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            LLMUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            LLMUI.SetActive(false);
        }
    }

}