// BallController.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class BallController : MonoBehaviour
{
    public float ballSpeed = 3f;
    public float speedIncreaseAmount = 0.5f; // 충돌 시 속도 증가량

    private Rigidbody2D rb;
    private Vector2 ballDirection;
    private bool isBallReleased = false;
    public float gameOverY = -6f; // 게임 오버가 될 Y 좌표 설정

    [SerializeField] private AudioClip blockHitSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.velocity = Vector2.zero; 

        ballDirection = Vector2.up;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isBallReleased)
        {
            Vector3 paddlePos = GameObject.Find("Paddle").transform.position; // GameObject.Find는 아직 deprecated가 아님
            transform.position = paddlePos + new Vector3(0, 0.185f, 0);

            if (Input.GetButtonDown("Fire1"))
            {
                isBallReleased = true;
                ballDirection = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
                rb.velocity = ballDirection * ballSpeed; 
            }
        }
        if (transform.position.y < gameOverY)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        if (isBallReleased)
        {
            rb.velocity = rb.velocity.normalized * ballSpeed; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        ballDirection = Vector2.Reflect(ballDirection, normal).normalized;
        rb.velocity = ballDirection * ballSpeed; 

        if (collision.gameObject.CompareTag("Paddle"))
        {
            float hitPoint = collision.contacts[0].point.x;
            float paddleCenter = collision.transform.position.x;
            float offset = (hitPoint - paddleCenter) / (collision.collider.bounds.size.x / 2f);
            ballDirection = new Vector2(offset, 1f).normalized;
            rb.velocity = ballDirection * ballSpeed; 
        }
        else if (collision.gameObject.CompareTag("Blocks"))
        {
            var block = collision.gameObject.GetComponent<BlockComponent>();
            if (block != null)
            {
                block.TakeDamage();

                if (blockHitSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(blockHitSound);
                }

                ballSpeed += speedIncreaseAmount;
                rb.velocity = ballDirection * ballSpeed; 
                Debug.Log($"블록 충돌! 공 속도 증가! 현재 속도: {ballSpeed}");
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene"); 
    }
}