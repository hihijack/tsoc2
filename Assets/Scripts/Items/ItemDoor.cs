using UnityEngine;
using System.Collections;
using System;

public class ItemDoor : MonoBehaviour
{
    public string guid;
    public int needItemID;

    // Use this for initialization
    void Start()
    {
        if (!string.IsNullOrEmpty(guid) && GameView.Inst.HasRecordDoorOpend(guid))
        {
            Open();
        }
    }

    [ContextMenu("GenGUID")]
    public void GenGUID()
    {
        guid = Tools.GetGUID();
    }

    /// <summary>
    /// 开门
    /// </summary>
    internal void Open()
    {
        //直接移除
       MonoKit.DestroyObject(gameObject);
    }
}
