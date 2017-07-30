using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 嘲讽
/// </summary>
public class Sneer : ISkill {
    IActor target;

    public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(3));
        range = GetBaseData().jdData["range"].AsInt;
    }

    public override void SetCaster(IActor _caster)
    {
        caster = _caster;
    }

    public override void SetTarget(IActor _target)
    {
        target = _target;
    }

    public override IEnumerator Act()
    {
        if (target != null && CheckCost())
        {
            //StartCD();
            caster.IsSkilling = true;
            //caster.PlayAnim(GetBaseData().anim);

            if (GetBaseData().intoBattle)
            {
               
            }

            // 施法前摇
            yield return new WaitForSeconds(0.5f);

            // 计算目标要移动的目标格子
            bool success = true;
            bool needCheckPath = true;
            MapGrid mgTarget = target.GetCurMapGrid();
            MapGrid mgCaster = caster.GetCurMapGrid();
            List<MapGrid> path = new List<MapGrid>();
            if (mgTarget.GetX() == mgCaster.GetX())
            {
                if (mgTarget.GetY() > mgCaster.GetY())
                {
                    path.Add(GameManager.gameView.GetMapGridByXY(mgCaster.GetX(), mgCaster.GetY() + 1));
                }
                else
                {
                    path.Add(GameManager.gameView.GetMapGridByXY(mgCaster.GetX(), mgCaster.GetY() - 1));
                }
            }
            else if (mgTarget.GetY() == mgCaster.GetY())
            {
                if (mgTarget.GetX() > mgCaster.GetX())
                {
                    path.Add(GameManager.gameView.GetMapGridByXY(mgCaster.GetX() + 1, mgCaster.GetY()));
                }
                else
                {
                    path.Add(GameManager.gameView.GetMapGridByXY(mgCaster.GetX() - 1, mgCaster.GetY()));
                }
            }
            else
            {
                int r = Random.Range(1, 3);// 随机取一个方向
                MapGrid mgMoveTargetA = GameManager.gameView.GetMapGridByXY(mgCaster.GetX(), mgTarget.GetY());
                MapGrid mgMoveTargetB = GameManager.gameView.GetMapGridByXY(mgTarget.GetX(), mgCaster.GetY());
                if (r == 1 && mgMoveTargetA.Type == EGridType.None)
                {
                    // 取 x 方向
                    path.Add(mgMoveTargetA);
                }
                else if(mgMoveTargetB.Type == EGridType.None)
                {
                    // 取y方向
                    path.Add(mgMoveTargetB);
                }
                else
                {
                    success = false;// 找不到行进路径
                }
                needCheckPath = false;
            }

            if (needCheckPath)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    MapGrid mgTemp = path[i];
                    if (mgTemp.Type != EGridType.None)
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (success)
            {
                //GameManager.gameView._RoundLogicState = GameRoundLogicState.DisableControll;
                caster.IsSkilling = false;
                StartCoroutine(target.CoMoveByGrids(path, false));
                //caster.PlayAnim("Stand");
            }
            else
            {
                UIManager.Inst.GeneralTip("找不到行进路径", Color.red);
            }
        }
    }
}
