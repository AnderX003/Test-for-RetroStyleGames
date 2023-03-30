using Bullets;

public interface IHittable
{
    public bool IsAlive { get; }
    public void Hit(BulletDamage damage);
}