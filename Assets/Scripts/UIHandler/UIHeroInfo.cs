using UnityEngine;
using System.Collections;

public class UIHeroInfo : MonoBehaviour {

    public UILabel txtName;
    public UILabel txtLevel;
    public UILabel txtJob;
    public UILabel txtStr;
    public UILabel txtAgi;
    public UILabel txtInt;
    public UILabel txtSta;
    public UILabel txtDamreduce;
    public UILabel txtHP;
    public UILabel txtTL;
    public UILabel txtResFire;
    public UILabel txtResPoision;
    public UILabel txtResThunder;
    public UILabel txtResForzen;
    public UILabel txtArm;
    public UILabel txtLoad;
    public UILabel txtMoveSpeed;

    public UILabel txtAtkPhy;
    public UILabel txtIAS;
    //public UILabel txtHit;
    //public UILabel txtDodge;
    public UILabel txtAtkMag;

    public UILabel txtPropAllotLeft;


    public UIButton btnAddStr;
    public UIButton btnAddAgi;
    public UIButton btnAddTen;
    public UIButton btnAddSta;

    GameView gameView;

    public void Init(GameView gameView) 
    {
        this.gameView = gameView;
        Hero hero = gameView._MHero;
        txtName.text = hero.nickname;
        txtLevel.text = hero.level.ToString();

        Refresh();

        btnAddStr.onClick.Clear();
        btnAddStr.onClick.Add(new EventDelegate(BtnAddStr));

        btnAddAgi.onClick.Clear();
        btnAddAgi.onClick.Add(new EventDelegate(BtnAddAgi));

        btnAddTen.onClick.Clear();
        btnAddTen.onClick.Add(new EventDelegate(BtnAddTen));

        btnAddSta.onClick.Clear();
        btnAddSta.onClick.Add(new EventDelegate(BtnAddSta));
    }

    void Refresh()
    {
        RefreshStrAndProp();
        RefreshAgiAndProp();
        RefreshTenAndProp();
        RefreshStaAndProp();
        RefreshMoveSpeed();
        txtResFire.text = Hero._Inst._Prop.ResFire.ToString();
        txtResPoision.text = Hero._Inst._Prop.ResPoision.ToString();
        txtResThunder.text = Hero._Inst._Prop.ResThunder.ToString();
        txtResForzen.text = Hero._Inst._Prop.ResForzen.ToString();
        txtArm.text = Hero._Inst._Prop.Arm.ToString();

        RefreshPropNeedAllot();
    }

    void RefreshPropNeedAllot()
    {
        txtPropAllotLeft.text = gameView._ProNeedAllot.ToString();
        if (gameView._ProNeedAllot == 0)
        {
            btnAddStr.gameObject.SetActive(false);
            btnAddAgi.gameObject.SetActive(false);
            btnAddTen.gameObject.SetActive(false);
            btnAddSta.gameObject.SetActive(false);
        }
    }

    void RefreshStrAndProp()
    {
        txtStr.text = Hero._Inst._Prop.Strength.ToString();
        txtAtkPhy.text = Hero._Inst._Prop.Atk.ToString();
    }

    void RefreshAgiAndProp() 
    {
        txtAgi.text = Hero._Inst._Prop.Agility.ToString();
        txtIAS.text = Hero._Inst._Prop.IAS.ToString("0.00") + "次/秒";
    }

    void RefreshMoveSpeed()
    {
        txtLoad.text = Hero._Inst._Prop.Load.ToString();
        txtMoveSpeed.text = Hero._Inst._Prop.MoveSpeed.ToString();
    }

    void RefreshTenAndProp()
    {
        txtInt.text = Hero._Inst._Prop.Tenacity.ToString();
        txtDamreduce.text = Hero._Inst._Prop.DamReduce.ToString("0.0%");
    }


    void RefreshStaAndProp()
    {
        txtSta.text = gameView._MHero._Prop.Stamina.ToString();
        txtHP.text = gameView._MHero._Prop.Hp + "/" + gameView._MHero._Prop.HpMax;
    }

    void BtnAddStr()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero._Prop.Strength++;
        gameView._PropHasAlltoToStr++;
        RefreshStrAndProp();
    }

    void BtnAddAgi()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        Hero._Inst._Prop.Agility++;
        gameView._PropHasAlltoToAgi++;
        RefreshAgiAndProp();
    }

    void BtnAddTen()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero._Prop.Tenacity++;
        gameView.IntToDirectProp(1,true);
        gameView._PropHasAllotToTen++;
        RefreshTenAndProp();
    }

    void BtnAddSta()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero._Prop.Stamina++;
        gameView._PropHasAllotToSta++;
        RefreshStaAndProp();
    }

    void Update()
    {
        Refresh();
    }
}
