using UnityEngine;
using System;
using System.Collections.Generic;


// Finite State machine that handles state instructions.
// class is attached to physical property so that the front updates when new state is encountered.
public class FsmAgent: MonoBehaviour{
    public State CurrentState;
    private GraphHandler _GHandler;
    // current node the agent is on.
    private GraphNode _currentNode = new GraphNode((0,0));
    
    //relative rotation of the agent.
    public int _lookRotation = 0;

    // Queue of actions that need to be performed.
    public Queue<State> Actions{ set; get;}



    public void begin(Queue<State> states)
    {
        //Unity specific code for generating an instance of the Graph Handler
        //equivilant to _GHandler = new GraphHandler();
        this.gameObject.AddComponent<GraphHandler>();
        _GHandler = this.gameObject.GetComponent<GraphHandler>();
        
        //initialize origin of graph
        _currentNode = _GHandler.initialize();
        _currentNode.origin = true;
        

        //mark origin
        _currentNode.markWithColor(Color.magenta);
        //iterate through the states and activate their unique function.
        Actions = states;
        while(Actions.Count > 0)
        {
            CurrentState = Actions.Dequeue();
            CurrentState.Activate(this);
        }

        //mark final node
        VisualUpdate();
        _currentNode.markWithColor(Color.cyan);
        
    }

    //Mechanic that turns Counter Clock Wise
    public void TurnCCW()
    {
        _lookRotation -= 90;   
    }

    //Mechanic that turns Clock Wise
    public void TurnCW()
    {
        _lookRotation +=90;
    }

    //Mechanic that Moves forward based on current _lookRotation
    public void Traverse()
    {
        //Find new coordinates
        (int, int) newCoords =  DetermineCoords();
        GraphNode oldNode = _currentNode;
        
        //update current node
        _currentNode = _GHandler.RetrieveNode(newCoords);
        _currentNode.VisitCount++;
        _currentNode.markWithColor();
        

        //Visualization of movement
        _GHandler.drawPath(oldNode,_currentNode,_lookRotation);
        this.gameObject.transform.position = new Vector3(newCoords.Item1,newCoords.Item2,-1);
        UiHelper.GetInstance().addToStack(_currentNode);
    }
   
    public (int,int) DetermineCoords()
    {
        (int,int) current = _currentNode.Coordinates;
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
        if(_GHandler != null)
        {
            _GHandler.selfDestruct();

        }
        Destroy(this.gameObject);
        
    }
}
