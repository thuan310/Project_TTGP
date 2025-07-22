using System;
using UnityEngine;

public class ChopWoodMinigameEvents : MonoBehaviour
{
    public event Action onChopWood;
    public void ChopWood()
    {
        onChopWood?.Invoke();
    }


    public event Action onActivateIcon;
    public void ActivateIcon()
    {
        onActivateIcon?.Invoke();
    }

    public event Action<string> onDisableIcon;
    public void DisableIcon(string id)
    {
        onDisableIcon?.Invoke(id);
    }

    public event Action onSharpenWood;
    public void SharpenWood()
    {
        onSharpenWood?.Invoke();
    }

    public event Action onDropWood;
    public void DropWood()
    {
        onDropWood?.Invoke();
    }

}
