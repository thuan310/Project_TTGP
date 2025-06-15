using Ink.Parsed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerUIDynamicHUDManager : MonoBehaviour
{

    public List<GameObject> UIDisplayeds;

    private void Start()
    {
    }

    public void AddCanvasToListAndTurnOn(GameObject canvas)
    {
        UIDisplayeds.Add(canvas);
        canvas.SetActive(true);
    }

    public void RemoveCanvasFromListAndTurnOff(GameObject canvas)
    {
        UIDisplayeds.Remove(canvas);
        canvas.SetActive(false);
    }

    public void ClearList()
    {
        foreach (var UIDisplayed in UIDisplayeds)
        {
            UIDisplayed.gameObject.SetActive(false);
        }
        UIDisplayeds.Clear();
    }
    public void SetInteractableUIWithAction(bool flag, string action)
    {
        if(flag)
        {
            AddCanvasToListAndTurnOn(PlayerUIManager.instance.interacted_UI);
        }
        else
        {
            RemoveCanvasFromListAndTurnOff(PlayerUIManager.instance.interacted_UI);
        }
        PlayerUIManager.instance.actionToDo.text = action;
    }

}
