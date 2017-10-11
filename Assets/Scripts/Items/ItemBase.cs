using UnityEngine;
using System.Collections;

/// <summary>
/// 地图上，对点击做出反应的基类
/// </summary>
public class ItemBase : MonoBehaviour
{
    public string guid;

    public virtual void OnTiggered() { }

    [ContextMenu("GenGUID")]
    public void GenGUID()
    {
        guid = Tools.GetGUID();
    }
}
