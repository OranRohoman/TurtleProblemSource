using System.Collections.Generic;
using UnityEngine;

//World handler for the finite state machines instance.
public class GraphHandler: MonoBehaviour
{
    // hash of all nodes based on unique position.
    Dictionary<(int,int), GraphNode> NodeDatabase = new Dictionary<(int, int), GraphNode>();

    //UI items.
    private GameObject _nodeTemplate;
    private GameObject _arrow;
    List<GameObject> _arrows = new List<GameObject>();

    public GraphNode initialize()
    {
        //load UI elements
        _nodeTemplate = Resources.Load<GameObject>("Node");
        _arrow = Resources.Load<GameObject>("Arrow");
        
        //generate the origin of the graph        
        return RetrieveNode((0,0));
        
    }

    //function creates and stores a new Node if it doesnt exist, else it returns the node.
    public GraphNode RetrieveNode((int, int) newCoords)
    {
        if(NodeDatabase.ContainsKey(newCoords))
        {
            
            return NodeDatabase[newCoords];
        }
        GraphNode newNode = new GraphNode(newCoords); 
        NodeDatabase[newCoords] = newNode;
        newNode.PhysicalNode = generateBody(newCoords);
        
        return newNode;
    }

    //draws an arc between nodes.
    public void drawPath(GraphNode oldNode, GraphNode newNode,int _rotation)
    {
        Vector3 pos = new Vector3((newNode.Coordinates.Item1),
        (newNode.Coordinates.Item2),
        -0.1f);
        Vector3 rotation = new Vector3(0,0,-1*_rotation);
        GameObject Spawned = Instantiate(_arrow,pos,Quaternion.identity);
        _arrows.Add(Spawned);
        Spawned.transform.eulerAngles = rotation;
    }

    //visualization function for creating an object at coordinates
    public GameObject generateBody((int, int) coords)
    {
        return Instantiate(_nodeTemplate, 
        new Vector3(coords.Item1, coords.Item2, 0), 
        Quaternion.identity);
    }

    //UI tidy up
    public void selfDestruct()
    {

        foreach( KeyValuePair<(int,int), GraphNode> node in NodeDatabase)
        {
            Destroy(node.Value.PhysicalNode);
        }
        for(int i = 0 ; i < _arrows.Count ; i++)
        {
            
            Destroy(_arrows[i]);
        }
    }
    
}
