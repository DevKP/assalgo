using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AssAlgo
{
    internal class Program
    {
        private static List<Node> field;
        private static Vector2f start = new Vector2f(0, 59);
        private static Vector2f end = new Vector2f(29, 0);

        static List<Node> openList = new List<Node>();
        static List<Node> closedList = new List<Node>();
        static List<Node> path = new List<Node>();

        private static void Main(string[] args)
        {
            TomasEngine engine = new TomasEngine("FUCK", 800,600,VideoMode.DesktopMode);

            var button = engine.CreateEntity<Button>();
            button.Size = new Vector2f(100, 50);
            button.Position = new Vector2f(300, 300);

            //var slider = engine.CreateEntity<Slider>();
            //slider.Position = new Vector2f(300, 300);

            FpsCounter fpsCounter = new FpsCounter();
            engine.HandleEntity(fpsCounter);
            engine.CreateEntity<GameController>();
            engine.Run();
            return;

            field = new List<Node>(60*60);
            for (var y = 0; y < 60; y++)
            for (var x = 0; x < 60; x++)
                field.Add(new Node(x, y));

            for (int i = 0; i < 30; i++)
            {
                
                var tmp = field[60 * 15 + i+5];
                tmp.State = NodeState.Obstacle;
                //field[30 * 15 + i] = tmp;
            }

            //for (int i = 0; i < 20; i++)
            //{

            //    var tmp = field[60 * 5 + i + 20];
            //    tmp.State = NodeState.Obstacle;
            //    //field[30 * 15 + i] = tmp;
            //}

            field[90].State = NodeState.Start;
            field[29].State = NodeState.Destination;

            //var path = AStarAlgorithm(new Vector2f(0,9),new Vector2f(9, 0) );

            using (var window = new RenderWindow(VideoMode.DesktopMode, "A* PathFinding"))
            {
                var rec = new RectangleShape();
                
                window.Closed += (o, _) => window.Close();

                Task.Run((() => AStarAlgorithm(start, end)));

                while (window.IsOpen)
                {
                    window.DispatchEvents();
                    window.Clear(new Color(70,70,70));

                    Update();

                    //window.Display();
                    Draw(window);
                    window.Display();
                }
            }
        }

        private static void Draw(RenderWindow window)
        {
            
            var openListCopy = openList.ToArray();
            var closedListCopy = closedList.ToArray();

            CircleShape cirleShape = new CircleShape(6);
            foreach (var node in field)
            {
                if (node.State == NodeState.Obstacle)
                {
                    cirleShape.Position = node.Position * 15;
                    cirleShape.FillColor = Color.Black;
                    window.Draw(cirleShape);
                }
            }
            cirleShape.FillColor = Color.White;

            foreach (var node in openListCopy)
            {
                if (node == null)
                    continue;
                if (node.Parent != null)
                {
                    Vertex[] line = {new Vertex(new Vector2f(node.Position.X * 15 + 6,node.Position.Y * 15 + 6),Color.Black),
                        new Vertex(new Vector2f(node.Parent.Position.X * 15 + 6,node.Parent.Position.Y * 15 + 6),Color.Black)};
                    window.Draw(line, PrimitiveType.Lines);
                }
            }

            foreach (var node in openListCopy)
            {
                if (node == null)
                    continue;

                cirleShape.Position = node.Position * 15;
                window.Draw(cirleShape);
            }


            cirleShape.FillColor = Color.Green;
            foreach (var node in closedListCopy)
            {
                if (node == null)
                    continue;
                if (node.Parent != null)
                {
                    Vertex[] line = {new Vertex(new Vector2f(node.Position.X * 15 + 6,node.Position.Y * 15 + 6),Color.White),
                        new Vertex(new Vector2f(node.Parent.Position.X * 15 + 6,node.Parent.Position.Y * 15 + 6),Color.White)};
                    window.Draw(line, PrimitiveType.Lines);
                }
            }
            foreach (var node in closedListCopy)
            {
                if (node == null)
                    continue;

                cirleShape.Position = node.Position * 15;
                window.Draw(cirleShape);
            }

            if (openListCopy.Length > 0)
            {
                cirleShape.FillColor = Color.Red;
                cirleShape.Position = openListCopy.Last().Position * 15;
                window.Draw(cirleShape);
            }

            var pathVertices = path.ConvertAll((input => new Vertex(new Vector2f(input.Position.X * 15 + 6, input.Position.Y * 15 + 6), Color.Black))).ToArray();
            window.Draw(pathVertices, PrimitiveType.LineStrip);
        }

        private static void Show(RenderWindow w)
        {

        }

        private static void Update()
        {
        }

        private static List<Node> AStarAlgorithm(Vector2f start, Vector2f end)
        {
            var startNode = new Node(start.X, start.Y);
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                var currentNode = openList.OrderBy(node => node.F).First();
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                Thread.Sleep(15);

                if (currentNode.State == NodeState.Destination)
                {
                    path.Clear();
                    Node current = currentNode;
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Parent;
                    }

                    return path;
                    
                    //Thread.Sleep(1000*60*60);
                }


                for (var localX = 0; localX < 3; localX++)
                    for (var localY = 0; localY < 3; localY++)
                    {
                        var x = (int)currentNode.Position.X - 1 + localX;
                        var y = (int)currentNode.Position.Y - 1 + localY;

                        if (x < 0 || x >= 60)
                            continue;
                        if (y < 0 || y >= 60)
                            continue;

                        var child = new Node(field[60 * y + x], currentNode);//Coping child node
                        if (currentNode.Position == child.Position)
                            continue;

                        if (child.State == NodeState.Obstacle)
                            continue;

                        if (closedList.Any(node => node.Position == child.Position))
                            continue;

                        //child.G = currentNode.G + MathF.Sqrt(
                        //    ((currentNode.Position.X - child.Position.X) *
                        //     (currentNode.Position.X - child.Position.X)) +
                        //    ((currentNode.Position.Y - child.Position.Y) *
                        //     (currentNode.Position.Y - child.Position.Y)));
                        child.G = currentNode.G + 1f;
                        child.H = MathF.Sqrt(((child.Position.X - end.X) * (child.Position.X - end.X)) +
                                             ((child.Position.Y - end.Y) * (child.Position.Y - end.Y)));
                        child.F = child.G + child.H;

                        if (openList.Any((node => node.Position == child.Position && node.G > child.G)))
                        {
                            continue;
                        }

                        openList.Add(child);


                        Thread.Sleep(15);
                    }
            }

            return null;
        }
    }
}