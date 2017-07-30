public class IAIState
{
    public IAI AI;
    public float dur;
    public virtual void OnInto()
    {
        dur = 0f;
    }

    public IAIState(IAI AI)
    {
        this.AI = AI;
    }
}