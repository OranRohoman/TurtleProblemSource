public class R : State{
    
  
    public override void Activate(FsmAgent agent) 
    {
        agent.TurnCW();
    }
    
}
