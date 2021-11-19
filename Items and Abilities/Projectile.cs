using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _projectileDirection;
    private float _projectileSpeed = 3;
    private float _remainingFlightDuration;
    private float _maxFlightDuration = 1.5f;

    void Update()
    {
        transform.position += _projectileDirection * _projectileSpeed * Time.deltaTime;
        _remainingFlightDuration -= 1 * Time.deltaTime;
        if (_remainingFlightDuration <= 0) DestroyProjectile();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            HitEnemy(collision.gameObject);
        }
    }
    private void HitEnemy(GameObject collisionGameObject)
    {
        collisionGameObject.gameObject.SetActive(false);   //-- If we hit an enemy. Disable the enemy.
        DestroyProjectile();
    }
    private void DestroyProjectile()
    {
        GameController.Instance.ObjectPool.StoreProjectile(this);  //-- When the projectile is to be destroyed. Call this method on our Object Pool class and have the pool
    }                                                              //-- decide if the object should be stored or destroyed
    public void ActivateProjectile(Vector3 position, Vector3 direction)
    {
        GameController.Instance.SpawnedObjects.Add(this.gameObject); //-- Adds this projectile to the list of spawned objects
        transform.position = position;                               //-- Move projectile to the assigned position
        _projectileDirection = direction;                            //-- Set the direction of the projectile to the assigned direction
        _remainingFlightDuration = _maxFlightDuration;               //-- Set the time before the projectile should be destroyed to the maximum time. (Stored projectiles will have a remaining time of 0)
    }
}
