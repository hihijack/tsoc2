using UnityEngine;
using System.Collections;

/// <summary>
/// 恢复神坛
/// </summary>
public class AltarRecover : AltarBase {

    public float val;
    public override void Init(ItemNPC npc)
    {
        base.Init(npc);
        val = npc.npcData.data["val"].AsFloat;
    }

    public override void OnActive()
    {
        base.OnActive();
        GameManager.hero.RecoverHp(Hero.Inst.Prop.HpMax - Hero.Inst.Prop.Hp);
        UIManager.Inst.GeneralTip("你获得了新生",Color.green);
        // 使用后立即销毁
        DestroyObject(gameObject);
    }
}
