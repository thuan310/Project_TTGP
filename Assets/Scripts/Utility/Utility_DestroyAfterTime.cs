using UnityEngine;

public class Utility_DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float timeUntilDestroyed;

    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }
}
