using UnityEngine;
using System.Collections;
using System;

enum EBattleUIState
{
    Atk,
    Power,
    UnControl,
    Skilling
}

public class UIBattle : MonoBehaviour
{
    public UIProgressBar progBar;
    public UISprite txuDef;

    UITweenProgbar utp;

    public GameObject pointAtk;
    public GameObject pointPower;
    public GameObject rootUnControl;

    public UIVisibleCtl vcAtk;
    public UIVisibleCtl vcPower;
    public UIVisibleCtl vcUnControl;
    public UIVisibleCtl vcSkill;

    public UILabel tip;

    public UILabel txtUnContrlTime;
    public UILabel txtSkillName;

    public GameObject gobjEffDodge;

    void Awake()
    {
        utp = progBar.GetComponent<UITweenProgbar>();
    }

    public void OnShow()
    {
        gobjEffDodge.SetActive(false);
    }

    internal void ToNormalState()
    {
        utp.enableAct = false;
        progBar.value = 1f;
        progBar.foregroundWidget.color = Color.green;
        SetState(EBattleUIState.Atk);
    }

    internal void ToAtkPoint(float time)
    {
        SetState(EBattleUIState.Atk);
        progBar.foregroundWidget.color = Color.yellow;
        progBar.value = 0f;
        utp.enableAct = true;
        utp.ResetToBeginning();
        utp.from = 0f;
        utp.to = 0.5f;
        utp.duration = time;
        utp.PlayForward();
    }

    internal void ToSkill(ISkill skill)
    {
        utp.enableAct = false;
        progBar.value = 1f;
        progBar.foregroundWidget.color = new Color32(255,20,147,255);
        SetState(EBattleUIState.Skilling);
        txtSkillName.text = skill.GetBaseData().name;
    }

    internal void UpdateUnControlTime(float dur)
    {
        txtUnContrlTime.text = dur.ToString("0.0");
    }

    internal void ToUnControl(float dur)
    {
        utp.enableAct = false;
        progBar.value = 1f;
        progBar.foregroundWidget.color = Color.gray;
        SetState(EBattleUIState.UnControl);
        txtUnContrlTime.text = dur.ToString("0.0");
    }

    internal void ToPowering()
    {
        progBar.foregroundWidget.color = Color.red;
        progBar.value = 0f;
        utp.enableAct = true;
        SetState(EBattleUIState.Power);
    }

    internal void ShowDef(bool show)
    {
        if (show)
        {
            txuDef.alpha = 1f;
        }
        else
        {
            txuDef.alpha = 0f;
        }
    }

    internal void UpdatePowerVal(float powerVal)
    {
        // powerVal : [0,3]
        progBar.value = powerVal / 3f;
    }

    internal void ToAfterPoint(float time)
    {
        progBar.foregroundWidget.color = Color.gray;
        utp.enableAct = true;
        utp.ResetToBeginning();
        utp.from = 0.5f;
        utp.to = 1f;
        utp.duration = time;
        utp.PlayForward();
    }

    void SetState(EBattleUIState state)
    {
        vcAtk.SetVisible(false);
        vcPower.SetVisible(false);
        vcUnControl.SetVisible(false);
        vcSkill.SetVisible(false);
        switch (state)
        {
            case EBattleUIState.Atk:
                vcAtk.SetVisible(true);
                break;
            case EBattleUIState.Power:
                vcPower.SetVisible(true);
                break;
            case EBattleUIState.UnControl:
                vcUnControl.SetVisible(true);
                break;
            case EBattleUIState.Skilling:
                vcSkill.SetVisible(true);
                break;
            default:
                break;
        }
    }

    internal void RefreshUIDodge(bool show)
    {
        gobjEffDodge.SetActive(show);
    }
}
