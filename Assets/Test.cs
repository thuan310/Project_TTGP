using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
