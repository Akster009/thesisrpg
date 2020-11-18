using System;
using SFML.System;

namespace SZDRPG.Pathing
{
    public class WayPoint
    {
        public Vector2f Location { get; set; }
        
        public bool Closed { get; set; }
        
        public double Cost { get; set; }
        
        public WayPoint PreviousWayPoint { get; set; }

        public WayPoint()
        {
            Closed = false;
            Cost = 0;
            PreviousWayPoint = null;
        }

        public WayPoint(Vector2f location, double cost = 0, WayPoint previousWayPoint = null)
        {
            Location = location;
            PreviousWayPoint = previousWayPoint;
            Cost = cost;
        }

        public double DistanceToTarget(Vector2f target)
        {
            return Math.Sqrt((target.X - Location.X) * (target.X - Location.X) +
                             (target.Y - Location.Y) * (target.Y - Location.Y));
        }
    }
}