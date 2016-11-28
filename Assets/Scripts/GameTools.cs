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
            case EEquipItemProperty.Int:
                desc = "精神力";
                break;
            case EEquipItemProperty.Sta:
                desc = "体能";
                break;
            case EEquipItemProperty.MaxLife:
                desc = "生命值";
                break;
            case EEquipItemProperty.TL:
                desc = "体力";
                break;
            case EEquipItemProperty.MaxMp:
                desc = "魔法值";
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
            case EEquipItemProperty.Hit:
                desc = "命中";
                break;
            case EEquipItemProperty.Dodge:
                desc = "躲闪";
                break;
            case EEquipItemProperty.Parry:
                desc = "格挡";
                break;
            case EEquipItemProperty.ParryDamage:
                desc = "格挡伤害";
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
            case EEquipItemProperty.AddDamage:
                desc = "增强伤害";
                break;
            default:
                break;
        }
        return desc;
    }
}
