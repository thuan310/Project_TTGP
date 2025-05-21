using UnityEngine;

public class FogWallInteractable : MonoBehaviour
{
    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("I.D")]
    public int fogWallID;

    [Header("Active")]
    public Observable<bool> isActive;

    private void Start()
    {
        OnIsActiveChanged(false, isActive.Value);
        isActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.instance.AddFogWallToList(this);
    }

    private void OnDestroy()
    {
        isActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.instance.RemoveFogWallToList(this);
    }

    private void OnIsActiveChanged(bool oldStatus, bool nerStatus)
    {
        if(isActive.Value)
        {
            foreach(var fogObject in fogGameObjects)
            {
                fogObject.SetActive(true);
            }
        }
        else
        {
            foreach(var fogObject in fogGameObjects)
            {
                fogObject.SetActive(false) ;
            }
        }
    }
    

}
