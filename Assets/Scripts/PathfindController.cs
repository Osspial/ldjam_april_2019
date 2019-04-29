using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathfindController : MonoBehaviour
{
    public static PathfindController instance;

    public float nodeDistance = 1.0f;
    public Vector2 nodeOffset = Vector2.zero;
    public LayerMask wallLayerMask;

    void OnEnable()
    {
        instance = this;
    }

    struct Node
    {
        public Vector2 position;
    }
    struct NodeConnection
    {
        public Vector2Int id;
        public Vector2 position;
        public bool passable;
    }
    Node NodeAt(Vector2Int id)
    {
        var position = new Vector2(id.x * nodeDistance, id.y * nodeDistance) + nodeOffset;
        return new Node
        {
            position = position
        };
    }
    Vector2Int NodeAtPos(Vector2 pos)
    {
        pos -= nodeOffset;
        return new Vector2Int(Mathf.RoundToInt(pos.x / nodeDistance), Mathf.RoundToInt(pos.y / nodeDistance));
    }
    public bool Passable(Vector2 a, Vector2 b, LayerMask wallLayerMask, LayerMask floorLayerMask) =>
        !(Physics2D.Linecast(a, b, wallLayerMask) || Physics2D.Linecast(b, a, wallLayerMask)) &&
        ((Physics2D.Linecast(a, b, floorLayerMask) && Physics2D.Linecast(b, a, floorLayerMask)) || floorLayerMask == new LayerMask());

    NodeConnection[] NeighborNodes(Vector2Int id, LayerMask layerMask, LayerMask floorLayerMask)
    {
        Vector2 origin = NodeAt(id).position;
        NodeConnection Connection(Vector2Int b)
        {
            var bPos = NodeAt(b).position;
            return new NodeConnection
            {
                id = b,
                position = bPos,
                passable = Passable(origin, bPos, layerMask, floorLayerMask)
            };
        }
        return new NodeConnection[]{
            Connection(new Vector2Int(id.x + 1, id.y + 0)),
            Connection(new Vector2Int(id.x + 1, id.y + 1)),
            Connection(new Vector2Int(id.x + 0, id.y + 1)),
            Connection(new Vector2Int(id.x - 1, id.y + 1)),
            Connection(new Vector2Int(id.x - 1, id.y + 0)),
            Connection(new Vector2Int(id.x - 1, id.y - 1)),
            Connection(new Vector2Int(id.x + 0, id.y - 1)),
            Connection(new Vector2Int(id.x + 1, id.y - 1))
        };
    }

    struct DescendFloatComparer : IComparer<float>
    {
        int IComparer<float>.Compare(float x, float y)
        {
            var val = -Comparer<float>.Default.Compare(x, y);
            if (val == 0)
            {
                val = 1;
            }
            return val;
        }
    }

    float HeuristicDistance(Vector2 a, Vector2 b)
    {
        var xDiff = b.x - a.x;
        var yDiff = b.y - a.y;
        var minDiff = Mathf.Min(Mathf.Abs(xDiff), Mathf.Abs(yDiff));

        var diagonal = a + new Vector2(minDiff * Mathf.Sign(xDiff), minDiff * Mathf.Sign(yDiff));
        return (a - diagonal).magnitude + (b - diagonal).magnitude;
    }

    struct NeighborUnvisited
    {
        public Vector2Int neighbor;
        public Vector2Int parent;
    }
    struct NodeDistance
    {
        public Vector2Int parent;
        public float distance;
    }
    // public int limit = 0;
    // List<Vector2Int> visitedNodes = new List<Vector2Int>();
    public List<Vector2> Search(Vector2 fromPos, Vector2 toPos, LayerMask layerMask, LayerMask floorLayerMask)
    {
        var from = NodeAtPos(fromPos);
        var to = NodeAtPos(toPos);

        if (from == to)
        {
            return new List<Vector2>();
        }
        // Map of nodes to their parent nodes
        var distance = new Dictionary<Vector2Int, NodeDistance>();
        var unvisitedNeighbors = new SortedList<float, Vector2Int>(new DescendFloatComparer { });
        var currentNode = from;
        var currentDistance = 0f;

        var limit = 1000;
        while (currentNode != to)
        {
            limit -= 1;
            if (limit == 0)
            {
                return null;
            }
            var currentPos = NodeAt(currentNode).position;
            foreach (var neighbor in NeighborNodes(currentNode, layerMask, floorLayerMask))
            {
                if (neighbor.passable)
                {
                    var neighborDistance = currentDistance + (currentPos - neighbor.position).magnitude;
                    if (!distance.ContainsKey(neighbor.id))
                    {
                        distance.Add(neighbor.id, new NodeDistance
                        {
                            distance = neighborDistance,
                            parent = currentNode
                        });
                        // var heuristic = (neighbor.position - toPos).magnitude * Vector2.Angle(neighbor.position - toPos, currentPos - neighbor.position);
                        var heuristic = HeuristicDistance(neighbor.position, toPos);
                        unvisitedNeighbors.Add(heuristic, neighbor.id);
                    }
                    else if (distance[neighbor.id].distance > neighborDistance)
                    {
                        distance[neighbor.id] = new NodeDistance
                        {
                            distance = neighborDistance,
                            parent = currentNode
                        };
                    }
                }
            }


            if (unvisitedNeighbors.Count == 0)
            {
                return null;
            }
            currentNode = unvisitedNeighbors.Last().Value;
            currentDistance = distance[currentNode].distance;
            unvisitedNeighbors.RemoveAt(unvisitedNeighbors.Count - 1);
        }

        List<Vector2> path = new List<Vector2>();

        path.Add(NodeAt(to).position);
        Vector2Int nextNode = to;
        do
        {
            var parent = distance[nextNode].parent;
            if (!Passable(NodeAt(parent).position, NodeAt(currentNode).position, layerMask, floorLayerMask))
            {
                path.Add(NodeAt(nextNode).position);
                currentNode = nextNode;
            }
            nextNode = parent;
        } while (nextNode != from);

        return path;
    }

    void OnDrawGizmosSelected()
    {
        for (var x = -10; x <= 10; x++)
        {
            for (var y = -10; y <= 10; y++)
            {
                Gizmos.DrawWireSphere(NodeAt(new Vector2Int(x, y)).position, 0.1f);
            }
        }
        // var path = Search(from, to);
        // if (path != null)
        // {
        //     for (int a = 0; a + 1 < path.Count; a++)
        //     {
        //         Gizmos.DrawLine(NodeAt(path[a]).position, NodeAt(path[a + 1]).position);
        //     }
        // }

        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(NodeAt(from).position, 0.1f);
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(NodeAt(to).position, 0.1f);
    }
}
