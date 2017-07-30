using UnityEngine;
using System.Collections;
using System;

public class LifeTimer : MonoBehaviour
{
    public float lifeTime;

    /// <summary>
    /// 创建时自动开启生命计时
    /// </summary>
    public bool autoLiftTime;

    public Action onLifeEnd;

    Transform tf;

    bool lifeing = false;
    float curLifeTime;

    void Awake()
    {
        this.tf = transform;
    }

    // Use this for initialization
    void Start()
    {
        if (autoLiftTime)
        {
            StartLifeTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeing)
        {
            curLifeTime += Time.deltaTime;
            if (curLifeTime >= lifeTime)
            {
                if (onLifeEnd != null)
                {
                    onLifeEnd();
                }
                DestroyObject(gameObject);
            }
        }
    }

    public void StartLifeTimer(float lifeTime)
    {
        this.lifeTime = lifeTime;
        StartLifeTimer();
    }

    /// <summary>
    /// 开始生命计时。以当前设置的lifeTime
    /// </summary>
    public void StartLifeTimer()
    {
        lifeing = true;
    }
}
