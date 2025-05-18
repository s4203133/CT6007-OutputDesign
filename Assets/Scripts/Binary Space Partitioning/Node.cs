using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Node parent { get; private set; }

    public List<Node> children { get; private set; }

    public Vector2 position { get; private set; }

    public int width { get; private set; }

    public int length { get; private set; }

    public Node(Node parent, Vector2 position, int width, int length) {
        children = new List<Node>();

        this.parent = parent;
        if (parent != null) {
            parent.AddChild(this);
        }
        this.position = position;
        this.width = width;
        this.length = length;
    }

    public void AddChild(Node newChild) {
        children.Add(newChild);
    }

    public void RemoveChild(Node child) { 
        children.Remove(child);
    }
}
