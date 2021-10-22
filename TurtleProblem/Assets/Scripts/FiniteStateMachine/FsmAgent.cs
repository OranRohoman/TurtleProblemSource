using UnityEngine;
using System;
using System.Collections.Generic;


// Finite State machine that handles state instructions.
// class is attached to physical property so that the front updates when new state is encountered.
public class FsmAgent: MonoBehaviour{
    public State CurrentState;
    public GraphHandler GHandler;
    // current node the agent is on.
    public GraphNode CurrentNode = new GraphNode((0,0));
    
    //relative rotation of the agent.
    public int _lookRotation = 0;

    // Queue of actions that need to be performed.
    public Queue<State> Actions{ set; get;}



    public void begin(Queue<State> states)
    {
        //Unity specific code for generating an instance of the Graph Handler
        //equivilant to GHandler = new GraphHandler();
        this.gameObject.AddComponent<GraphHandler>();
        GHandler = this.gameObject.GetComponent<GraphHandler>();
        
        //initialize origin of graph
        CurrentNode = GHandler.initialize();
        CurrentNode.origin = true;
        

        //mark origin
        CurrentNode.markWithColor(Color.magenta);
        //iterate through the states and activate their unique function.
        Actions = states;
        while(Actions.Count > 0)
        {
            CurrentState = Actions.Dequeue();
            CurrentState.Activate(this);
        }

        //mark final node
        VisualUpdate();
        CurrentNode.markWithColor(Color.cyan);
        
    }

    //Mechanic that turns Counter Clock Wise
    public virtual void TurnCCW(){}

    //Mechanic that turns Clock Wise
    public virtual void TurnCW(){}

    //Mechanic that Moves forward based on current _lookRotation
    public virtual void Traverse(){ }
   
    public (int,int) DetermineCoords()
    {
        (int,int) current = CurrentNode.Coordinates;
        int rotationCopy = _lookRotation;
        List<(int,int)> potentialCoords = new List<(int, int)>(){
            (current.Item1,current.Item2+1), // North
            (current.Item1+1,current.Item2), // East
            (current.Item1,current.Item2-1), //South
            (current.Item1-1,current.Item2), //West
        };
        
        double transformed_rotation = ((rotationCopy %= 360) < 0 ? rotationCopy + 360 : rotationCopy) / 90;
        
        int index = (int)(Math.Round(transformed_rotation) % 4);
        
        
        return potentialCoords[index];
    }
    public void VisualUpdate()
    {
        
        Vector3 Rotation = new Vector3(gameObject.transform.rotation.x, 
        gameObject.transform.rotation.y, 
        (-1*_lookRotation));

        gameObject.transform.eulerAngles =  Rotation;
    }
    public void destroySelf()
    {
        if(GHandler != null)
        {
            GHandler.selfDestruct();

        }
        Destroy(this.gameObject);
        
    }
}
