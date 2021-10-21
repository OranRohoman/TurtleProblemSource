using System;
using UnityEngine;


// representation of a node coordinate
public class GraphNode
{
    // Coordinate in tuple form where origin is (0,0)
    public (int,int) Coordinates{ set; get; }
    // Physical represntation of node for front-end
    public GameObject PhysicalNode {set; get;}
    // flag denoting if it is the origin
    public bool origin {get;set;}
    // How many times has the turtle visited this node?
    public int VisitCount{ set;get;}

    public GraphNode( (int , int) coord )
    {
        Coordinates = coord;
        origin = false;
    }

    // visually mark a node with a color
    public void markWithColor(Color color)
    {
        PhysicalNode.GetComponent<SpriteRenderer>().color = color;
    }
    public void markWithColor()
    {
        if(!origin)
        {
            Color color = heatMap();
            PhysicalNode.GetComponent<SpriteRenderer>().color = color;
        }
    }
    // improvised Heat map function. Visit count determines severity of the color.
    private Color heatMap()
    {
        if(VisitCount == 1)
        {
            return Color.white;
        }
        else if(VisitCount >1 && VisitCount <=2)
        {
            return Color.yellow;
        }
        else if(VisitCount > 2 && VisitCount <=4)
        {
            return Color.red;
        }
        else if(VisitCount > 4 && VisitCount <= 6)
        {
            return new Color( 89f/255f , 39f/255f ,32f/255f);
        }
        else
        {
            return Color.black;
        }
    }
}
