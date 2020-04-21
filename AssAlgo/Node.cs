using SFML.System;

namespace AssAlgo
{
    public enum NodeState
    {
        Empty,
        Visited,
        Start,
        Destination,
        Obstacle
    }
    public class Node
    {
        private Vector2f _position;
        public Vector2f Position
        {
            get => _position;
            set => _position = value;
        }

        public float F { get; set; } = 0;
        public float G { get; set; } = 0;
        public float H { get; set; } = 0;
        public NodeState State { get; set; } = NodeState.Empty;
        public Node Parent { get; set; }

        public Node(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }
        public Node(Node node, Node parent = null)
        {
            _position = node.Position;
            F = node.F;
            G = node.G;
            H = node.H;
            State = node.State;
            Parent = parent ?? node.Parent;
        }
    }
}