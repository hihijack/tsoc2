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
    public UILabel txtMP;
    public UILabel txtHP;
    public UILabel txtTL;
    public UILabel txtResFire;
    public UILabel txtResPoision;
    public UILabel txtResThunder;
    public UILabel txtResForzen;
    public UILabel txtArm;

    public UILabel txtAtkPhy;
    public UILabel txtIAS;
    public UILabel txtHit;
    public UILabel txtDodge;
    public UILabel txtAtkMag;

    public UILabel txtPropAllotLeft;


    public UIButton btnAddStr;
    public UIButton btnAddAgi;
    public UIButton btnAddInt;
    public UIButton btnAddSta;

    GameView gameView;

    public void Init(GameView gameView) 
    {
        this.gameView = gameView;
        Hero hero = gameView._MHero;
        txtName.text = hero.nickname;
        txtLevel.text = hero.level.ToString();

        RefreshStrAndProp();
        RefreshAgiAndProp();
        RefreshIntAndProp();
        RefreshStaAndProp();

        txtResFire.text = hero.GetResFire().ToString();
        txtResPoision.text = hero.GetResPoision().ToString();
        txtResThunder.text = hero.GetResThunder().ToString();
        txtResForzen.text = hero.GetResForzen().ToString();
        txtArm.text = hero.GetArm().ToString();

       
       
        txtAtkMag.text = hero.atkMag.ToString();

        RefreshPropNeedAllot();

        btnAddStr.onClick.Clear();
        btnAddStr.onClick.Add(new EventDelegate(BtnAddStr));

        btnAddAgi.onClick.Clear();
        btnAddAgi.onClick.Add(new EventDelegate(BtnAddAgi));

        btnAddInt.onClick.Clear();
        btnAddInt.onClick.Add(new EventDelegate(BtnAddInt));

        btnAddSta.onClick.Clear();
        btnAddSta.onClick.Add(new EventDelegate(BtnAddSta));
    }

    void RefreshPropNeedAllot()
    {
        txtPropAllotLeft.text = gameView._ProNeedAllot.ToString();
        if (gameView._ProNeedAllot == 0)
        {
            btnAddStr.gameObject.SetActive(false);
            btnAddAgi.gameObject.SetActive(false);
            btnAddInt.gameObject.SetActive(false);
            btnAddSta.gameObject.SetActive(false);
        }
    }

    void RefreshStrAndProp()
    {
        txtStr.text = gameView._MHero._Strength.ToString();
        txtAtkPhy.text = gameView._MHero.GetAtk().ToString();
    }

    void RefreshAgiAndProp() 
    {
        txtAgi.text = gameView._MHero.agility.ToString();
        txtIAS.text = gameView._MHero._IAS.ToString("0.00") + "次/秒";
        txtHit.text = gameView._MHero.hit.ToString();
        txtDodge.text = gameView._MHero.dodge.ToString();
    }

    void RefreshIntAndProp()
    {
        txtInt.text = gameView._MHero.intell.ToString();
        txtMP.text = gameView._MHero._Mp + "/" + gameView._MHero.mpMax;
    }


    void RefreshStaAndProp()
    {
        txtSta.text = gameView._MHero.stamina.ToString();
        txtHP.text = gameView._MHero.hp + "/" + gameView._MHero._HpMax;
        txtTL.text = gameView._MHero.tl + "/" + gameView._MHero.tlMax;
    }

    void BtnAddStr()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero._Strength++;
        gameView._PropHasAlltoToStr++;
        RefreshStrAndProp();
    }

    void BtnAddAgi()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero.agility++;
        gameView.AgiToDirectProp(1, true);
        gameView._PropHasAlltoToAgi++;
        RefreshAgiAndProp();
    }

    void BtnAddInt()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero.intell++;
        gameView.IntToDirectProp(1,true);
        gameView._PropHasAllotToInt++;
        RefreshIntAndProp();
    }

    void BtnAddSta()
    {
        gameView._ProNeedAllot--;
        RefreshPropNeedAllot();
        gameView._MHero.stamina++;
        gameView.StaToDirectProp(1, true);
        gameView._PropHasAllotToSta++;
        RefreshStaAndProp();
    }
}
