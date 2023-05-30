using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using SD = System.Diagnostics;
using System.Linq;

namespace Wunderwunsch.HexGridSimplified
{
    //this is really bad performance wise to far but it seems to wrk^^
    public static class Pathfinder
    {
        //Just as first step to see if planned Structure works, then Dijkstra, then A*Star
        public static bool allowMoveIntoNegativeRemaining = false; // example: 1 movement point remaining, can still do 1 final move onto everything (like in civ5)
        public static List<PathNode> GetPath(Movement movement, Vector3Int origin, Vector3Int target, int minMovementCost) // should really return a Tuple<Vector3Int,int>()
        {
            bool wrapX = Hex.WrapsHorizontal;
            int mapSizeX = Hex.MapSize.x;
            int minCost = minMovementCost;
            //first thing we do is check if the target is a type which we just can't walk onto at all (mountain, or ocean for someone who can't embark
            //then we probably just return null?    
            if (!movement.TileIsWalkable(target)) return null; //Movement does not allow to enter that tile so why bother with pathfinding at all!as            

            int fullMovementPoints = movement.GetMaxMovementPoints();
            int remainingMovementPoints = movement.RemainingMovementPoints;
            //Debug.Assert(fullMovementPoints > 0);
            MovementCost runningCost = new MovementCost(0, fullMovementPoints - movement.RemainingMovementPoints); //unit might not have all movementpoints left when we start

            PathNode firstNode = new PathNode(origin, null, runningCost); //startNode gets itself as prevPos so we can identify it later
            PathNode targetNode = null; // we'll

            //SimplePriorityQueue<PathNode, int> frontier = new SimplePriorityQueue<PathNode, int>();
            FastPriorityQueue<PathNode> frontier = new FastPriorityQueue<PathNode>(2500);

            Dictionary<Vector3Int, MovementCost> costByVisitedPosition = new Dictionary<Vector3Int, MovementCost>();

            frontier.Enqueue(firstNode, 1);

            //SD.Stopwatch watch = new SD.Stopwatch();

            int safeGuard = 0;
            while (frontier.Count > 0)
            {
                safeGuard++;

                PathNode currentNode = frontier.Dequeue();

                runningCost = currentNode.costToThisNode;

                if (currentNode.position == target)
                {
                    targetNode = currentNode;
                    break; // we found target so lets not waste anytime!
                }
                List<Vector3Int> neighbours = Hex.GetNeighbours(currentNode.position, true);

                foreach (Vector3Int n in neighbours)
                {
                    // out of map bounds
                    //watch.Start();
                    int movementPointCost = movement.CalculateCostBetweenTiles(currentNode.position, n); //should we pass MapID here?
                    //watch.Stop();
                    MovementCost costToThisNode;

                    if (movementPointCost < 0) //NEGATIVE COST == not pathable
                    {
                        continue; //that should suffice, I mean we look at it multiple times then (once for each neighbour it has and we walk onto, but other checks also cost)
                    }

                    if (runningCost.movementPointCost == 0 && movementPointCost > fullMovementPoints)
                    {
                        movementPointCost = fullMovementPoints; // if we haven't moved that turn yet we set cost to fullMovement to allow every unit to move on anything pathable if it has full movementpoints
                    }

                    if (movementPointCost + runningCost.movementPointCost > fullMovementPoints) //that means we can't move on it this turn and do it next. This here is Civ6Style rules
                    {
                        costToThisNode = new MovementCost(runningCost.turnCost + 1, movementPointCost);
                    }
                    else costToThisNode = new MovementCost(runningCost.turnCost, runningCost.movementPointCost + movementPointCost);
                    PathNode node = new PathNode(n, currentNode, costToThisNode);


                    if (!costByVisitedPosition.ContainsKey(n) || costByVisitedPosition[n] > costToThisNode) // we only add a node to the queue if we either didn't visit it yet or 
                    {
                        costByVisitedPosition.Remove(n); //need to benchmark if that is cheap enough, trying to remove it when it isn't there just does nothing which is what we want
                        Vector3Int targetModified = target;                        
                        int astarRawPenalty = Hex.Distance(n, targetModified) * minCost; //a*star penalty needs to properly be converted into turns
                        int astarTurnsPenalty = astarRawPenalty / fullMovementPoints;
                        int astartMovePointPenalty = astarRawPenalty % fullMovementPoints;
                        int priority = 100000 * (costToThisNode.turnCost + astarTurnsPenalty) + (costToThisNode.movementPointCost + astartMovePointPenalty);
                        frontier.Enqueue(node, priority);
                        costByVisitedPosition.Add(node.position, node.costToThisNode);
                    }
                }
            }
            //Debug.Log("StopwatchedTime: " + watch.ElapsedMilliseconds);
            if (targetNode != null && origin != target)
            {
                List<PathNode> path = new List<PathNode>();
                PathNode p = targetNode;
                while (true)
                {
                    path.Add(p);
                    p = p.previousNode;
                    if (p.Equals(firstNode)) break;
                }
                path.Reverse();
                //Debug.Log("Dequeues: " + safeGuard);
                return path;
            }
            else return null;
        }

        public static Dictionary<Vector3Int, int> GetAllInTurnRange(Movement movement, Vector3Int origin)
        {
            int movementPointStart = movement.RemainingMovementPoints;
            int runningCost;

            PathNode firstNode = new PathNode(origin, null, new MovementCost(0, 0));
            FastPriorityQueue<PathNode> frontier = new FastPriorityQueue<PathNode>(5500);
            Dictionary<Vector3Int, int> costByVisitedPosition = new Dictionary<Vector3Int, int>();

            frontier.Enqueue(firstNode, 0);
            costByVisitedPosition.Add(firstNode.position, 0);

            while (frontier.Count > 0)
            {
                PathNode currentNode = frontier.Dequeue();
                runningCost = currentNode.costToThisNode.movementPointCost;

                List<Vector3Int> neighbours = Hex.GetNeighbours(currentNode.position, true);
                foreach (Vector3Int n in neighbours)
                {
                    int moveCostToThisTile = movement.CalculateCostBetweenTiles(currentNode.position, n);
                    if (moveCostToThisTile < 0) continue; // Unwalkable
                    moveCostToThisTile = moveCostToThisTile + runningCost;

                    if (moveCostToThisTile > movementPointStart) continue;

                    MovementCost cost = new MovementCost(0, moveCostToThisTile);
                    PathNode node = new PathNode(n, currentNode, cost);

                    if (!costByVisitedPosition.ContainsKey(n) || costByVisitedPosition[n] > moveCostToThisTile)
                    {
                        costByVisitedPosition.Remove(n);
                        int priority = moveCostToThisTile;
                        frontier.Enqueue(node, priority);
                        costByVisitedPosition.Add(node.position, node.costToThisNode.movementPointCost);
                    }
                }
            }           
            return costByVisitedPosition;
        }
    }
}
