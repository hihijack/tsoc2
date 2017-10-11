using UnityEngine;
using System.Collections;
using System;

public class UINPCMutual : MonoBehaviour {

    public UIGrid gridItem;
    public UISprite bg;

    ItemNPC npc;

    ItemTransfer transfer;

    ItemDoor door;

    ItemGrilTip girlTip;

    public void Init(ItemNPC npc) 
    {
        this.npc = npc;
        ENPCActionType[] types = npc.arrActions;
        InitItemsByActions(types);
    }

    public void Init(ItemTransfer transfer)
    {
        this.transfer = transfer;
        ENPCActionType[] types = null;
        if (transfer.actived)
        {
            types = new ENPCActionType[2] { ENPCActionType.Transfer, ENPCActionType.Bye };
        }
        else
        {
            types = new ENPCActionType[2] { ENPCActionType.ActiveTransfer, ENPCActionType.Bye };
        }

        InitItemsByActions(types);
    }

    internal void Init(ItemDoor door)
    {
        this.door = door;
        ENPCActionType[] types = new ENPCActionType[2] { ENPCActionType.OpenDoor, ENPCActionType.Bye };
        InitItemsByActions(types);
    }

    public void Init(ItemGrilTip girlTip)
    {
        this.girlTip = girlTip;
        ENPCActionType[] types = new ENPCActionType[2] { ENPCActionType.TouchGirlTip, ENPCActionType.Bye };
        InitItemsByActions(types);
    }

    void InitItemsByActions(ENPCActionType[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            ENPCActionType actType = types[i];
            GameObject gobjItemAction = Tools.AddNGUIChild(gridItem.gameObject, IPath.UI + "item_npc_mutual");
            UILabel txtDesc = Tools.GetComponentInChildByPath<UILabel>(gobjItemAction, "txt");
            string strDesc = "";
            switch (actType)
            {
                case ENPCActionType.Bye:
                    strDesc = "再见";
                    break;
                case ENPCActionType.Active:
                    strDesc = "激活";
                    break;
                case ENPCActionType.ActiveTransfer:
                    strDesc = "激活传送站";
                    break;
                case ENPCActionType.Transfer:
                    strDesc = "传送";
                    break;
                case ENPCActionType.Talk:
                    strDesc = "交谈";
                    break;
                case ENPCActionType.Trade:
                    strDesc = "交易";
                    break;
                case ENPCActionType.Treat:
                    strDesc = "治疗我";
                    break;
                case ENPCActionType.Forge:
                    strDesc = "锻造";
                    break;
                case ENPCActionType.Trial:
                    strDesc = "试炼塔";
                    break;
                case ENPCActionType.Rest:
                    strDesc = "休息";
                    break;
                case ENPCActionType.TouchGirlTip:
                    strDesc = "触碰印记";
                    break;
                default:
                    break;
            }
            txtDesc.text = strDesc;
            UIButton btn = Tools.GetComponentInChildByPath<UIButton>(gobjItemAction, "bg");
            btn.onClick.Add(new EventDelegate(BtnClick_Action));
            btn.data = actType;
        }
        // 设置bg长度
        bg.height = 92 * types.Length + 18;
    }

    void BtnClick_Action()
    {
        ENPCActionType type = (ENPCActionType)UIButton.current.data;
        switch (type)
        {
            case ENPCActionType.Bye:
                UIManager.Inst.CloseNPCWords();
                UIManager.Inst.CloseUINPCMutual();
                break;
            case ENPCActionType.Talk:
                GameManager.gameView.OnTaldToANPC(npc);
                break;
            case ENPCActionType.Trade:
                // 交易
                {
                    UIManager.Inst.CloseUINPCMutual();
                    UITrade uiTrade = UIManager.Inst.ShowUITrade();
                    uiTrade.Init(npc);
                    UIManager.Inst.ToggleUI_Bag();
                    //UIManager._Instance.GetUIBag().SetWinPosInTrade();
                }
                break;
            case ENPCActionType.Treat:
                // 治疗
                break;
            case ENPCActionType.Forge:
                // 锻造
                break;
            case ENPCActionType.Trial:
                {
                    // 虚空钥匙
                    UIManager.Inst.CloseUINPCMutual();
                    UISmallBag bag = UIManager.Inst.ShowUISmallBag();
                    bag.Init(GameManager.gameView);
                }
                break;
            case ENPCActionType.Active:
                {
                    // 激活神坛
                    UIManager.Inst.CloseUINPCMutual();
                    GameManager.gameView.OnActiveANPC(npc);
                }
                break;
            case ENPCActionType.ActiveTransfer:
                {
                    //激活传送站
                    UIManager.Inst.CloseUINPCMutual();
                    GameView.Inst.ActiveTransfer(transfer);
                }
                break;
            case ENPCActionType.Transfer:
                {
                    //传送
                    UITransfer uiTransfer = UIManager.Inst.ShowUITransfer();
                    uiTransfer.Init();
                    UIManager.Inst.CloseUINPCMutual();
                }
                break;
            case ENPCActionType.Rest:
                {
                    //休息
                    UIManager.Inst.CloseUINPCMutual();
                    GameView.Inst.OnRest();
                }
                break;
            case ENPCActionType.OpenDoor:
                {
                    //开门
                    UIManager.Inst.CloseUINPCMutual();
                    GameView.Inst.OnOpenDoor(door);
                }
                break;
            case ENPCActionType.TouchGirlTip:
                {
                    //触碰印记
                    UIManager.Inst.CloseUINPCMutual();
                    GameView.Inst.OnTouchGirlTip(girlTip);
                }
                break;
            default:
                break;
        }
    }
}
