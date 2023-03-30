using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;

namespace Helpers
{
    public static class StatFuncs
    {
        [Pure]
        public static float Interpolate(
            float v, float oldMin, float oldMax, float newMin, float newMax)
            => Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(oldMin, oldMax, v));

        [Pure]
        public static float Interpolate01(float v, float oldMin, float oldMax)
            => Mathf.Lerp(0f, 1f, Mathf.InverseLerp(oldMin, oldMax, v));

        [Pure]
        public static T GetClosestToPos<T>(IEnumerable<Component> collection, Vector3 pos)
            where T : Component => (T) collection.Aggregate((a, b) =>
            Vector3.Distance(pos, a.transform.position) <=
            Vector3.Distance(pos, b.transform.position)
                ? a
                : b);

        [Pure]
        public static Transform GetClosestToPos(IEnumerable<Transform> collection,
            Vector3 pos)
            => collection.Aggregate((a, b) =>
                Vector3.Distance(pos, a.position) <=
                Vector3.Distance(pos, b.position)
                    ? a
                    : b);

        [Pure]
        public static float GetRandomCoefficient(float range)
            => Random.Range(-range, range);

        [Pure]
        public static Vector3 GetRandomVector3Rotation()
        {
            const float maxRot = 360f;
            const float minRot = 0f;

            return new Vector3(
                Random.Range(minRot, maxRot),
                Random.Range(minRot, maxRot),
                Random.Range(minRot, maxRot));
        }

        [Pure]
        public static Quaternion GetRandomQuaternionRotation()
            => Quaternion.Euler(GetRandomVector3Rotation());

        [Pure]
        public static Vector3 GetRandomTorqueRotation()
        {
            const float maxRot = 200f;
            const float minRot = -200f;

            return new Vector3(
                Random.Range(minRot, maxRot),
                Random.Range(minRot, maxRot),
                Random.Range(minRot, maxRot));
        }

        [Pure]
        public static Vector2 GetRandomPointOnCircle()
        {
            var r = Random.Range(0f, 1f);
            var theta = Random.Range(0, 2 * Mathf.PI); 
            var x = r * Mathf.Cos(theta);
            var y = r * Mathf.Sin(theta);
            return new Vector2(x, y);
        }

        [Pure]
        public static Vector2 GetRandomPointOnCircle(float radius)
            => radius * GetRandomPointOnCircle();
    }
}
