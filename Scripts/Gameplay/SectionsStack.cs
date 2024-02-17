using UnityEngine;

public class SectionsStack : MonoBehaviour, ISubscriber
{
    [SerializeField] private Transform _spawnPosition;

    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _increaseSpeed = 5f;

    private bool _isMoving;
    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _spawnPosition = GameObject.FindGameObjectWithTag("SpawnPos").transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        Spawn();
    }
    private void Update()
    {
        if (_isMoving)
        {
            Vector2 movement = new Vector2(-_speed * Time.deltaTime, 0f);

            _rigidBody.velocity = movement;
        }
    }
    private void StartMove()
    { 
        _isMoving = true;
    }
    private void StopMove()
    {
        _isMoving = false;
    }
    private void Spawn()
    {
        transform.position = _spawnPosition.position;
        _speed = _speed + _increaseSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Spawn();
        }
    }

    public void SubscribeAll()
    {
        GameState.Instance.GameStarted += StartMove;
    }

    public void UnsubscribeAll()
    {
        GameState.Instance.GameStarted -= StartMove;
    }
}
