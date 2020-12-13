using System;
using System.Collections.Generic;
using SFML.System;
using SZDRPG.Model;

namespace SZDRPG.Pathing
{
    public class Path
    {
        public List<Vector2f> PathEdges { get; set; } = new List<Vector2f>();
        public Vector2f StartPoint { get; set; }
        public Vector2f EndingPoint { get; set; }

        public PEntity Owner { get; set; }
        
        public Game Game { get; set; }

        public Path()
        {
            
        }

        public Path(Vector2f startPoint, Vector2f endingPoint, Game game= null, PEntity owner = null)
        {
            StartPoint = startPoint;
            EndingPoint = endingPoint;
            Game = game;
            Owner = owner;
            FindPath();
        }

        public List<Vector2f> FindPath()
        {
            PathEdges.Clear();
            List<WayPoint> wayPoints = new List<WayPoint>();
            wayPoints.Add(new WayPoint(StartPoint));
            WayPoint current = wayPoints[0];
            WayPoint previous = null;
            while (wayPoints.Count != 0)
            {
                previous = current;
                current = FindBestWaypoint(wayPoints, current.Location, EndingPoint);
                if (current == null)
                {
                    Vector2f collision = FindCollision(previous.Location, EndingPoint); 
                    int x = 0;
                    int y = 0;
                    if (EndingPoint.X < collision.X)
                        x++;
                    else if(EndingPoint.X > collision.X)
                    {
                        x--;
                    }
                    if (EndingPoint.Y < collision.Y)
                        y++;
                    else if(EndingPoint.Y > collision.Y)
                    {
                        y--;
                    }
                    EndingPoint = new Vector2f(collision.X+x, collision.Y+y);
                    WayPoint wayPoint = new WayPoint(FindCollision(previous.Location, EndingPoint));
                    wayPoint.Cost = previous.Cost + wayPoint.DistanceToTarget(EndingPoint);
                    wayPoint.PreviousWayPoint = previous;
                    wayPoints.Add(wayPoint);
                    current = FindBestWaypoint(wayPoints, previous.Location, EndingPoint);
                }
                else if (current.Location == EndingPoint)
                {
                    foreach (var wayPoint in wayPoints)
                    {
                        wayPoint.Closed = false;
                    }
                    return ReconstructPath(current);
                }
                current.Closed = true;
                AddWayPoints(ref wayPoints, current, EndingPoint);
            }

            return null;
        }

        public List<Vector2f> ReconstructPath(WayPoint current)
        {
            while(current.PreviousWayPoint!=null)
            {
                Vector2f edge = new Vector2f(current.Location.X - current.PreviousWayPoint.Location.X,
                    current.Location.Y - current.PreviousWayPoint.Location.Y);
                PathEdges.Add(edge);
                current = current.PreviousWayPoint;
            }

            return PathEdges;
        }

        public void AddWayPoints(ref List<WayPoint> wayPoints, WayPoint current, Vector2f end)
        {
            Vector2f collision = FindCollision(current.Location, end);
            if (collision != end)
            {
                if(Game.EntityAt(collision) != null)
                {
                    if (Game.EntityAt(collision) == Game.EntityAt(EndingPoint))
                    {
                        int x = 0;
                        int y = 0;
                        if (EndingPoint.X < collision.X)
                            x++;
                        else if(EndingPoint.X > collision.X)
                        {
                            x--;
                        }
                        if (EndingPoint.Y < collision.Y)
                            y++;
                        else if(EndingPoint.Y > collision.Y)
                        {
                            y--;
                        }
                        EndingPoint = new Vector2f(collision.X+x, collision.Y+y);
                    }
                    else
                    {
                        foreach (var travelNode in Game.EntityAt(collision).hitMesh.TravelNodes)
                        {
                            if (travelNode != current)
                            {
                                Vector2f collisionInner = FindCollision(current.Location, travelNode.Location);
                                if (collisionInner != travelNode.Location)
                                {
                                    if (Game.EntityAt(collision) != Game.EntityAt(collisionInner))
                                        AddWayPoints(ref wayPoints, current, collisionInner);
                                }
                                else if (!wayPoints.Contains(travelNode))
                                {
                                    travelNode.Cost = current.Cost + travelNode.DistanceToTarget(current.Location);
                                    travelNode.PreviousWayPoint = current;
                                    wayPoints.Add(travelNode);
                                }
                                else
                                {
                                    double newCost = current.Cost + travelNode.DistanceToTarget(current.Location);
                                    if (newCost < wayPoints[wayPoints.IndexOf(travelNode)].Cost)
                                    {
                                        travelNode.PreviousWayPoint = current;
                                        travelNode.Cost = newCost;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                wayPoints.Add(new WayPoint(end,current.DistanceToTarget(end),current));
            }
        }

        public WayPoint FindBestWaypoint(List<WayPoint> wayPoints, Vector2f location, Vector2f target)
        {
            if (wayPoints == null)
                return null;
            WayPoint best = null;
            foreach (var wayPoint in wayPoints)
            {
                if (best == null)
                {
                    if (!wayPoint.Closed)
                        best = wayPoint;
                }
                else if (!wayPoint.Closed && wayPoint.DistanceToTarget(target) + wayPoint.Cost < best.DistanceToTarget(target) + best.Cost)
                    best = wayPoint;
            }

            return best;
        }

        private Vector2f FindCollision(Vector2f start, Vector2f end)
        {
            Vector2f currentPoint = new Vector2f(start.X, start.Y);
            Vector2f movementVector = new Vector2f(end.X - start.X,end.Y-start.Y);
            Vector2f deltaFirst = new Vector2f(0,0);
            Vector2f deltaSecond = new Vector2f(0,0);
            if (movementVector.X < 0)
            {
                deltaFirst.X = -1;
                deltaSecond.X = -1;
            }
            else if (movementVector.X > 0)
            {
                deltaFirst.X = 1;
                deltaSecond.X = 1;
            }
            if (movementVector.Y < 0)
            {
                deltaFirst.Y = -1;
            }
            else if (movementVector.Y > 0)
            {
                deltaFirst.Y = 1;
            }
            float longer = Math.Abs(movementVector.X);
            float shorter = Math.Abs(movementVector.Y);
            if (!(longer > shorter))
            {
                longer = shorter;
                shorter = Math.Abs(movementVector.X);
                if (movementVector.Y < 0)
                    deltaSecond.Y = -1;
                else if (movementVector.Y > 0)
                    deltaSecond.Y = 1;
                deltaSecond.X = 0;
            }

            float numerator = longer / 2;
            for (int i = 0; i <= longer; i++)
            {
                PEntity found = Game.EntityAt(currentPoint);
                if(found != null && found != Owner && found.IsCollidable())
                {
                    return currentPoint;
                }
                numerator += shorter;
                if (!(numerator < longer))
                {
                    numerator -= longer;
                    currentPoint.X += deltaFirst.X;
                    currentPoint.Y += deltaFirst.Y;
                }
                else
                {
                    currentPoint.X += deltaSecond.X;
                    currentPoint.Y += deltaSecond.Y;
                }
            }

            return end;
        }

        public double EdgeLength(int idx)
        {
            if (PathEdges.Count > idx)
                return Math.Sqrt(PathEdges[idx].X * PathEdges[idx].X + PathEdges[idx].Y * PathEdges[idx].Y);
            return 0;
        }

        public Vector2f PopLastEdge()
        {
            Vector2f ret = PathEdges[PathEdges.Count - 1];
            PathEdges.RemoveAt(PathEdges.Count-1);
            return ret;
        }
    }
}