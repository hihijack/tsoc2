using UnityEngine;
using System.Collections;

/// <summary>
/// 传送站
/// </summary>
public class ItemTransfer : MonoBehaviour
{
    /// <summary>
    /// 所在地图ID
    /// </summary>
    public int mapID;
    /// <summary>
    /// 是否已经激活
    /// </summary>
    public bool actived = false;

    public Sprite spNormal;
    public Sprite spActed;

    SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {
        actived = GameView.Inst.IsTransferActived(mapID);
        RefreshActived();
    }

    public void RefreshActived()
    {
        if (actived)
        {
            if (sr == null)
            {
                sr = Tools.GetComponentInChildByPath<SpriteRenderer>(gameObject, "model");
            }
            sr.sprite = spActed;
        }
    }
}
