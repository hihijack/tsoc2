using UnityEngine;
using System.Collections;

public class ItemGrilTip : ItemBase
{
    public string[] tips;
    
    // Use this for initialization
    void Start()
    {
        if (!string.IsNullOrEmpty(guid) && GameView.Inst.HasRecordGirlTip(guid))
        {
            MonoKit.DestroyObject(gameObject);
        }
    }

    public override void OnTiggered()
    {
        base.OnTiggered();
        UINPCMutual unm = UIManager.Inst.ShowUINPCMutual();
        unm.Init(this);
    }

    /// <summary>
    /// 当少女提示结束
    /// </summary>
    /// <param name="girlTip"></param>
    internal void OnGirlTipEnd()
    {
        //保存
        GameView.Inst.SaveRecordGirlTip(guid);
        //移除
        MonoKit.DestroyObject(gameObject);
    }
}
