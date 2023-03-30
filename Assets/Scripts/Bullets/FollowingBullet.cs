using UnityEngine;

namespace Bullets
{
    public class FollowingBullet : Bullet
    {
        [SerializeField] private float dissolveError;
        [SerializeField] private float rotateRatio;
        private TrackingPoint trackingPoint;

        public void SetTarget(TrackingPoint trackingPoint)
        {
            this.trackingPoint = trackingPoint;
        }

        private void Update()
        {
            if ((trackingPoint.Position - transform.position).magnitude <=dissolveError)
            {
                Dissolve();
                return;
            }
            
            var targetDirection = trackingPoint.Position - transform.position;
            var forward = transform.forward;
            forward = new Vector3(
                Mathf.Lerp(forward.x, targetDirection.x, rotateRatio),
                Mathf.Lerp(forward.y, targetDirection.y, rotateRatio),
                Mathf.Lerp(forward.z, targetDirection.z, rotateRatio));
            transform.forward = forward;
            transform.position += transform.forward.normalized * (Time.deltaTime * Speed);
        }

        protected override void OnHit(IHittable hittable)
        {
            base.OnHit(hittable);
            Dissolve();
        }

        protected override void Dissolve()
        {
            SceneC.Instance.PoolsHolder.FollowingBulletsPool.PushItem(this);
        }
    }
}
