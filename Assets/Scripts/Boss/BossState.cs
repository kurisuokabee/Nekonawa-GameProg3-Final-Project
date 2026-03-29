using UnityEngine;

public abstract class BossState : MonoBehaviour
{   
    
    public abstract void Enter();
    
    public abstract void Action();
    
    public abstract void Exit();
}
