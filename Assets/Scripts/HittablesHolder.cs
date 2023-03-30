using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HittablesHolder
{
    private Dictionary<Collider, IHittable> hittableByCollider;

    public void Init()
    {
        hittableByCollider = new Dictionary<Collider, IHittable>();
    }

    public void RegisterHittable(IHittable hittable, Collider collider)
    {
        hittableByCollider.Add(collider, hittable);
    }

    public void UnregisterHittable(Collider collider)
    {
        hittableByCollider.Remove(collider);
    }

    public IHittable GetHittable(Collider collider)
    {
        if (hittableByCollider.TryGetValue(collider, out var hittable))
        {
            return hittable;
        }

        return null;
    }

    public bool TryGetHittable(Collider collider, out IHittable hittable)
    {
        return hittableByCollider.TryGetValue(collider, out hittable);
    }
}