using UnityEngine;
using System.Collections;

/// <summary>
/// 攻击进度条
/// </summary>
public class AtkBar : MonoBehaviour {

    public float atkPoint;
    public float afterPoint;

    UIProgressBar progBar;

    void Awake() { 
       progBar = GetComponent<UIProgressBar>();
    }

	// Use this for initialization
	void Start () {
       
	}
	

    public void ToAtkPoint(float dur) 
    {
        UITweenProgbar utp = GetComponent<UITweenProgbar>();
        utp.ResetToBeginning();
        utp.from = 0f;
        utp.to = atkPoint;
        utp.duration = dur;
        
        utp.PlayForward();
    }

    public void ToAfterPoint(float dur) 
    {
        UITweenProgbar utp = GetComponent<UITweenProgbar>();
        utp.ResetToBeginning();
        utp.from = atkPoint;
        utp.to = afterPoint;
        utp.duration = dur;

        utp.PlayForward();
    }

    public void ToEnd(float dur) 
    {
        UITweenProgbar utp = GetComponent<UITweenProgbar>();
        utp.ResetToBeginning();
        utp.from = afterPoint;
        utp.to = 1f;
        utp.duration = dur;
        utp.enabled = true;
        utp.PlayForward();
    }

    /// <summary>
    /// 进度重置为0
    /// </summary>
    public void ReStart() 
    {
        progBar.value = 0;
    }
}
