using UnityEngine;
using System.Collections;

public class GObjLife : MonoBehaviour {

    public float lifeTime;
	// Use this for initialization

    public delegate void delegateOnDie(GameObject gobj);

    public delegateOnDie OnDie;

	void Start () {
        if (lifeTime > 0)
        {
            StartCoroutine(CoTime());
        }
	}
    IEnumerator CoTime()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyObject(gameObject);
        if (OnDie != null)
        {
            OnDie(gameObject);
        }
    }
}
