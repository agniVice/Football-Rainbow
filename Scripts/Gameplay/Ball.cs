using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _particlesPrefab;
    [SerializeField] private Sprite[] _ballSprites;

    [Space]
    [Header("BallSettings")]
    [SerializeField] private Type _ballType;
    [SerializeField] private float _jumpHeight = 5f;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private bool _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _rigidbody.simulated = false;

        SetRandomType();
    }
    private void OnEnable()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
    }
    private void OnDisable()
    {
        PlayerInput.Instance.PlayerMouseDown -= OnPlayerMouseDown;
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlesPrefab[(int)_ballType]).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y+0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    private void OnPlayerMouseDown()
    {
        if (_isMoving == false)
        {
            _isMoving = true;
            _rigidbody.simulated = true;

            Jump();
        }
        else
            Jump();
    }
    private void SetRandomType()
    {
        _ballType = (Type)Random.Range(0, 4);

        _spriteRenderer.sprite = _ballSprites[(int)_ballType];
    }
    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpHeight);
        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.PopUp, 1, 0.8f);
        SpawnParticle();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            GameState.Instance.FinishGame();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Section"))
        {
            if (collision.gameObject.GetComponent<Section>().SectionType == _ballType)
            {
                PlayerScore.Instance.AddScore();
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, 1f);
                SetRandomType();
            }
            else
            {
                GameState.Instance.FinishGame();
            }
        }
    }
}
