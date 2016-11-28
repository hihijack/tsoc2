using UnityEngine;
using System.Collections;

/// <summary>
/// 狂怒神坛
/// </summary>

public class AltarFury : AltarBase
{
    float val;
    int dur;
    public override void Init(ItemNPC npc)
    {
        base.Init(npc);
        this.val = npc.npcData.data["val"].AsFloat;
        this.dur = npc.npcData.data["dur"].AsInt;
    }

    public override void OnActive()
    {
        base.OnActive();
        // 不允许叠加buff
        Buff_AltarFury buffFury = GameManager.hero.gameObject.GetComponent<Buff_AltarFury>();
        if (buffFury == null)
        {
            buffFury = GameManager.hero.gameObject.AddComponent<Buff_AltarFury>();
            buffFury.Init(GameManager.hero, val, dur);
            buffFury.StartEffect();
			UIManager._Instance.GeneralTip("你觉得武器变得毫无重量",Color.green);
			// 使用后立即销毁
			DestroyObject(gameObject);
        }
    }
}
