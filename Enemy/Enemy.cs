using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IRestart
{
    public float Damage = 5f;
    private readonly float _speed = 2;
    private float _turnTime = 1;
    private int _direction;
    private Vector3 _spawnPoint;

    void Start()
    {
        _spawnPoint = transform.position;
        GameController.Instance.ActiveGameObjects.Add(this.gameObject);

        _direction = 1;
        _turnTime = Random.Range(1, 10);

        StartCoroutine("ChangeDirection");
    }

    void Update()
    {
        transform.position += Vector3.left * _direction * _speed * Time.deltaTime;

        if (transform.position.x < Map.Left || transform.position.x > Map.Right)
            _direction *= -1;
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(_turnTime);
        var randomDirection = Random.Range(0, 2);
        _direction = randomDirection == 0 ? -1 : 1;
        _turnTime = Random.Range(1, 10);
        StartCoroutine("ChangeDirection");
    }

    public void Respawn()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        transform.position = _spawnPoint;
        _direction = 1;
    }
}
