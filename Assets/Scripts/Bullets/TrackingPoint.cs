using UnityEngine;

namespace Bullets
{
    public class TrackingPoint
    {
        private Transform movingPoint;
        private Vector3 staticPoint;
        private bool attached;

        public bool Attached
        {
            get => attached;
            set
            {
                if (value == false)
                {
                    staticPoint = movingPoint.position;
                }
                attached = value;
            }
        }

        public Vector3 Position => Attached
            ? movingPoint.position
            : staticPoint;

        public TrackingPoint(Transform movingPoint)
        {
            attached = true;
            this.movingPoint = movingPoint;
        }
    }
}
