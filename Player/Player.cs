using UnityEngine;

public class Player : MonoBehaviour, IInputs, IRestart
{
    #region Fields and Properties
    [SerializeField] float _speed = 5;
    [SerializeField] readonly float _maxHealth = 50;
    [SerializeField] LayerMask _solidCollisionLayer;

    private Vector3 _spawnPoint;
    private Vector3 _currentDirection;
    private float _playerHalfSize;
    private float _health;
    public float HealthPercentage => (_health / _maxHealth) * 100;
    #endregion

    #region Unity Methods - Unity Call will call these metods
    void Start()
    {
        GameController.Instance.ActiveGameObjects.Add(this.gameObject);
        GameController.Instance.Inputs.ControllableObjects.Add(this.gameObject.GetComponent<IInputs>()); //-- Adds the Interface component to the list of Controllable Objects so plauer will receive Inputs

        _spawnPoint = transform.position;
        _currentDirection = Vector3.up;
        _playerHalfSize = GetComponent<BoxCollider2D>().bounds.extents.x;  //-- Store half the players size. Used for RayCasting.
        _health = _maxHealth;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)                       //-- Check the tag of the GameObject we collided with and call the relevant Method
        {
            case "enemy"  :   HitEnemy(collision.gameObject);   break;
            case "powerup":   HitPowerUp(collision.gameObject); break;
            default:                                            break;
        }

        void HitPowerUp(GameObject powerUp)
        {
            _health = _maxHealth;
            powerUp.SetActive(false);  //-- Disable the PowerUp.
            GameController.Instance.GameUI.UpdateText();
        }

        void HitEnemy(GameObject enemy)
        {
            var damage = enemy.GetComponent<Enemy>().Damage;    //-- Get the Enemy script component on the GameObject we collided with, and access it's Damage property
            _health -= damage;
            GameController.Instance.GameUI.ToggleHurtEffect(); //-- Tell the User Interface to play the hurt effect
            GameController.Instance.GameUI.UpdateText();

            if (_health <= 0)
                GameController.Instance.RestartGame();
        }
    }
    void OnDrawGizmos()     //-- OnDrawGizmos used to draw visual cues inside Unity. Often used to visualize what our code does
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.cyan;

        var from = transform.position;
        var to = transform.position + (_currentDirection.normalized * _playerHalfSize);  //-- Take players current position, add the current direction normalized (means length of the direction is set 1), and multiply by half of the players size.
        Gizmos.DrawLine(from, to);
    }
    #endregion

    #region IInput Methods
    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        _currentDirection = direction.normalized; //--Normalize the direction so it has a length of 1. Else player will move faster on diagonals (See Vector Math video on Vector addition for explanation)

        if (!CollidedWithSolid())
            transform.position += direction * _speed * Time.deltaTime;
    }
    public void ShootProjectile()
    {
        var projectile = GameController.Instance.ObjectPool.SpawnProjectile();  //-- Gets a projectile either from the pool of stored projectiles, or by spawning a new projectile Object.
        projectile.ActivateProjectile(transform.position, _currentDirection);   //-- Sets properties on the projectile so it will spawn at the correct location and direction.
    }
    #endregion

    #region Other Methods
    bool CollidedWithSolid() //-- Raycasting Method that returns true if player collided with the wall, and false if not.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _currentDirection, _playerHalfSize, _solidCollisionLayer);
        if (hit == true)
        {
            if (hit.collider.gameObject.tag == "solid")
                return true;
        }
        return false;
    }
    public void Respawn()  //-- Resets the player back to it's original spawn position
    {
        transform.position = _spawnPoint;
        _currentDirection = Vector3.up;
        _health = _maxHealth;
    }
    #endregion
}