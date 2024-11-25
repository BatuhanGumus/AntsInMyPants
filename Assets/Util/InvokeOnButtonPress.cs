using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonType
{
    Down,
    Hold,
    Up
}


[System.Serializable]
public class ButtonEventPair
{
    public KeyCode button;
    public ButtonType type = ButtonType.Down;
    public UnityEvent onEvent;
}


public class InvokeOnButtonPress : MonoBehaviour
{
    public ButtonEventPair[] actions;

    void Update()
    {
        int len = actions.Length;
        for (int i = 0; i < len; i++)
        {
            bool callAction = false;
            switch (actions[i].type)
            {
                case ButtonType.Down:
                    callAction = Input.GetKeyDown(actions[i].button);
                    break;
                case ButtonType.Up:
                    callAction = Input.GetKeyUp(actions[i].button);
                    break;
                case ButtonType.Hold:
                    callAction = Input.GetKey(actions[i].button);
                    break;
            }
            if(callAction) actions[i].onEvent?.Invoke();

        }
    }
}
