using System;
using UnityEngine;

public class InputEvents 

{
    public event Action<Vector2> onMovePressed;
    public void MovePressed(Vector2 moveDir)
    {
        if (onMovePressed != null)
        {
            onMovePressed(moveDir);
        }
    }

    public event Action onInteractPressed;
    public void InteractPressed()
    {
        if (onInteractPressed != null)
        {
            onInteractPressed();
        }
    }

    public event Action onToggleQuestPressed;
    public void ToggleQuestPressed()
    {
        if (onToggleQuestPressed != null)
        {
            onToggleQuestPressed();
        }
    }


}