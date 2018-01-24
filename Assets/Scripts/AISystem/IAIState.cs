public class IAIState
{
    public IAI AI;
    public float dur;
    public virtual void OnInto()
    {
        dur = 0f;
    }

    public virtual void OnUpdate() { }

    public IAIState(IAI AI)
    {
        this.AI = AI;
    }
}