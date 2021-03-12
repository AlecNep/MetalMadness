using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Node
{
    /* Child node to evaluate */
    private Node m_node;

    public Node node
    {
        get { return m_node; }
    }

    /* The constructor requires the child node that this inverter decorator 
     * wraps*/
    public Repeater(Node node)
    {
        m_node = node;
    }

    //Consider using a regular for loop for sequence and selector and don't increment if the current node is a repeater
    //use "is" keyword!
    public override NodeStates Evaluate() //DEFINITELY NOT THE WAY TO DO IT. THIS JUST TOTALLY BRICKS UNITY
    {
        /*while (true)
        {
            switch (m_node.Evaluate())
            {
                case NodeStates.FAILURE:
                    return NodeStates.FAILURE;
                default:
                    continue;
            }
        }*/
        return NodeStates.FAILURE;
    }
}
