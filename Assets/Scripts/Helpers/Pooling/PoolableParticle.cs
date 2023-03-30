using UnityEngine;

namespace Helpers.Pooling
{
    public class PoolableParticle : MonoBehaviour, IPoolable
    {
        Transform IPoolable.Transform => transform;

        GameObject IPoolable.GameObject => gameObject;

        [field: SerializeField] public ParticleSystem Particle { get; private set; }

        private void OnValidate()
        {
            Particle ??= GetComponent<ParticleSystem>();
        }

        public virtual void OnParticleSystemStopped()
        {
        }
    }
}
