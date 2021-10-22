using UnityEngine;


public class Turtle: FsmAgent
{
    public override void TurnCCW()
    {
        _lookRotation -=90;
    }
    public override void TurnCW()
    {
        _lookRotation +=90;
    }
    public override void Traverse()
    {
        //Find new coordinates
        (int, int) newCoords =  DetermineCoords();
        GraphNode oldNode = CurrentNode;
        
        //update current node
        CurrentNode = GHandler.RetrieveNode(newCoords);
        CurrentNode.VisitCount++;
        CurrentNode.markWithColor();
        

        //Visualization of movement
        GHandler.drawPath(oldNode,CurrentNode,_lookRotation);
        this.gameObject.transform.position = new Vector3(newCoords.Item1,newCoords.Item2,-1);
        UiHelper.GetInstance().addToStack(CurrentNode);
    } 
}
