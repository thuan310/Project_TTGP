using System;
using UnityEngine;

public enum InputEventContext {
    DEFAULT,
    DIALOGUE
}

public class InputEvents 
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    public void ChangeInputEventContext(InputEventContext context)
    {
        this.inputEventContext = context;
    }


    public event Action<Vector2> onMovePressed;
    public void MovePressed(Vector2 moveDir)
    {
        onMovePressed?.Invoke(moveDir);
    }

    public event Action<InputEventContext> onInteractPressed;
    public void InteractPressed()
    {
        onInteractPressed?.Invoke(this.inputEventContext);
    }

    public event Action onToggleQuestPressed;
    public void ToggleQuestPressed()
    {
        onToggleQuestPressed?.Invoke();
    }

    public event Action<bool> onDodgePressed;
    public void DodgePressed(bool ctx)
    {
        onDodgePressed?.Invoke(ctx);
    }

    public event Action<bool> onSprintPressed;
    public void SprintPressed(bool ctx)
    {
        onSprintPressed?.Invoke(ctx);
    }
}