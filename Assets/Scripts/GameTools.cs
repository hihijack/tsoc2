using System.Collections;

public static class GameTools{

	// 属性描述
    public static string Prop2Desc(EEquipItemProperty property)
    {
        string desc = "";
        switch (property)
        {
            case EEquipItemProperty.Str:
                desc = "力量";
                break;
            case EEquipItemProperty.Agi:
                desc = "敏捷";
                break;
            case EEquipItemProperty.Ten:
                desc = "精神力";
                break;
            case EEquipItemProperty.Sta:
                desc = "体能";
                break;
            case EEquipItemProperty.Arm:
                desc = "护甲";
                break;
            case EEquipItemProperty.IAS:
                desc = "攻击速度";
                break;
            case EEquipItemProperty.ResFire:
                desc = "火焰抗性";
                break;
            case EEquipItemProperty.ResThunder:
                desc = "闪电抗性";
                break;
            case EEquipItemProperty.ResPoison:
                desc = "毒素抗性";
                break;
            case EEquipItemProperty.ResFrozen:
                desc = "冰冻抗性";
                break;
            case EEquipItemProperty.CriticalStrike:
                desc = "致命一击";
                break;
            case EEquipItemProperty.ParryDamage:
                desc = "格挡强化";
                break;
            case EEquipItemProperty.FireDamage:
                desc = "火焰伤害";
                break;
            case EEquipItemProperty.ThunderDamage:
                desc = "闪电伤害";
                break;
            case EEquipItemProperty.PoisonDamage:
                desc = "毒素伤害";
                break;
            case EEquipItemProperty.ForzenDamage:
                desc = "冰冷伤害";
                break;
            case EEquipItemProperty.AddDamagePercent:
                desc = "伤害强化";
                break;
            case EEquipItemProperty.Weight:
                desc = "重量";
                break;
            case EEquipItemProperty.MoveSpeed:
                desc = "移动速度";
                break;
            case EEquipItemProperty.AddDamage:
                desc = "伤害";
                break;
            case EEquipItemProperty.ArmPercent:
                desc = "护甲强化";
                break;
            case EEquipItemProperty.PowerDmg:
                desc = "蓄力伤害";
                break;
            case EEquipItemProperty.PowerSpeed:
                desc = "蓄力速度";
                break;
            default:
                break;
        }
        return desc;
    }
}
