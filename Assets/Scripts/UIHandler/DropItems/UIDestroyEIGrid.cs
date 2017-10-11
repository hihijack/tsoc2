using UnityEngine;
using System.Collections;
using System;

public class UIDestroyEIGrid : MonoBehaviour
{
    public UIEventTrigger et;

    // Use this for initialization
    void Start()
    {
        et.onHoverOver.Add(new EventDelegate(OnHoverOVer));
        et.onHoverOut.Add(new EventDelegate(OnHoverOut));
        et.onPress.Add(new EventDelegate(OnPress));
    }

    private void OnPress()
    {
        UIManager.Inst.OnDropEquipItemTo(gameObject, true);
    }

    private void OnHoverOut()
    {
        UIManager.Inst.ShowCursorTip("", Color.red);
    }

    private void OnHoverOVer()
    {
        if (UIEquipItemOperControll.curDropItem != null)
        {
            UIManager.Inst.ShowCursorTip("丢弃！", Color.red);
        }
        else
        {
            UIManager.Inst.ShowCursorTip("", Color.red);
        }
    }
}
