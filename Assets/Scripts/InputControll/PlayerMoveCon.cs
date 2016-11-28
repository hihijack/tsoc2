using UnityEngine;
/// <summary>
/// 玩家移动控制
/// </summary>
class PlayerMoveCon
{
    Hero hero;
    public PlayerMoveCon(Hero hero)
    {
        this.hero = hero;
    }

    /// <summary>
    /// 上方向键按下
    /// </summary>
    public void OnKeyArrow(EDirection dir)
    {
        if (hero._State == EActorState.Normal && !UIManager._Instance.HasUI())
        {
            MapGrid mgNext = hero.GetCurMapGrid().GetNextGrid(dir);
            if (mgNext != null)
            {
                hero.MoveToAGrid(mgNext);
            }
        }
    }
}