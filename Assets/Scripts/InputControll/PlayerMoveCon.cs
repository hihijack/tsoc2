using UnityEngine;
/// <summary>
/// 玩家移动控制
/// </summary>
class PlayerMoveCtl
{
    Hero hero;
    public PlayerMoveCtl(Hero hero)
    {
        this.hero = hero;
    }

    /// <summary>
    /// 上方向键按下
    /// </summary>
    public void OnKeyArrow(EDirection dir)
    {
        if (hero._State == EActorState.Normal && !UIManager.Inst.HasUI())
        {
            MapGrid mgNext = hero.GetCurMapGrid().GetNextGrid(dir);
            if (mgNext != null && mgNext.IsEnablePass())
            {
                hero.MoveToAGrid(mgNext);
            }
        }
    }
}