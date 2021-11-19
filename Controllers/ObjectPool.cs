using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _playerProjectilePrefab; //-- This is the GameObject that will spawn when the player fire a projectile.
                                                                 //-- Set in the Inspector or through code. F.ex based on equipped weapon.
    private List<GameObject> _storedProjectiles;
    private const int _maxStoredProjectiles = 10;

    void Start()
    {
        _storedProjectiles = new List<GameObject>();
        GameController.Instance.ObjectPool = this;      //-- Set the reference to the Object Pool in our Singleton to this instance of Object Pool.
    }

    public void StoreProjectile(Projectile projectileToStore)
    {
        if (_storedProjectiles.Count <= _maxStoredProjectiles) Store();     //-- If we currently store less than the desired number of stored projectiles, store the projectile in the list
        else                                                   Delete();    //-- If we have above the desired number of stored projectiles. We instead delete the projectile from the game.

        void Store()
        {
            projectileToStore.gameObject.SetActive(false);                                  //-- Disable the GameObject
            GameController.Instance.SpawnedObjects.Remove(projectileToStore.gameObject);    //-- And remove it from the list of currently active objects
            _storedProjectiles.Add(projectileToStore.gameObject);                           //-- Finally add it to the list of stored objects
        }

        void Delete()
        {
            GameController.Instance.SpawnedObjects.Remove(projectileToStore.gameObject);    //-- Remove the projectile from the list of currently active objects
            Destroy(projectileToStore.gameObject);                                          //-- And delete the object from the game
        }
    }
    public Projectile SpawnProjectile()
    {
        return _storedProjectiles.Count > 0 ? ProjectileFromPool() : NewProjectile();       //-- Ternary operator. If we have projectiles spawn, we re-use the stored projectile.
                                                                                            //-- If not, we spawn a new projectile.

        Projectile ProjectileFromPool()
        {
            var projectile = _storedProjectiles[0];                                 //-- Take the first object from the pool of stored projectiles and create a new reference to it
            var projectileScript = projectile.GetComponent<Projectile>();                   //-- Get the Projectile Script component on the object
            projectile.SetActive(true);                                                     //-- Activate the GameObject
            _storedProjectiles.Remove(projectile);                                          //-- Remove the projectile from the list since we now have a direct reference to the object
            return projectileScript;                                                        //-- Return a reference to the Projectile Script back to the player
        }

        Projectile NewProjectile()
        {
            var projectile = Instantiate(_playerProjectilePrefab, transform.position, Quaternion.identity);   //-- Tell Unity to Spawn a new Instance of our GameObject prefab (Set in the Inspector)
            var projectileScript = projectile.GetComponent<Projectile>();                                             //-- Get the Projectile Script component on the object
            return projectileScript;                                                                                  // --Return a reference to the Projectile Script back to the player
        }
    }
}
