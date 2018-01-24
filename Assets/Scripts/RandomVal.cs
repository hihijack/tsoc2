[System.Serializable]
public class RandomVal
{
    public float min;
    public float max;

    public float RanVal()
    {
        return UnityEngine.Random.Range(min, max);
    }
}