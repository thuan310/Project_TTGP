using System;
using UnityEngine;

public class TalkToNPCEvents 
{
    public event Action<int> onTalkToNPC;
    public void TalkToNPC(int npcID)
    {
        onTalkToNPC?.Invoke(npcID);
    }

    public event Action<int> onActivateNPC;
    public void ActivateNPC(int npcID)
    {
        onActivateNPC?.Invoke(npcID);
    }

    public event Action onActivateIcon;
    public void ActivateIcon()
    {
        onActivateIcon?.Invoke();
    }
}
