using UnityEngine;

public class SpeedDownItemController : MonoBehaviour
{
    public Sprite itemSprite; // 인스펙터에서 스프라이트 할당용 (렌더러에 자동 할당)
    public float speedDecreaseAmount = 1f;
    public float itemBounceSpeed = 3f; // 벽에서 튕겨나갈 때의 속도
    public float destroyY = -6f; // 아이템이 사라질 Y 좌표

    private Rigidbody2D rb2d;

    void Start()
    {
      
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>(); // 또는 다른 적절한 콜라이더
        }
        collider.isTrigger = false; // 물리적 충돌을 위해 false로 설정

        // 3. Rigidbody2D 설정
        rb2d = GetComponent<Rigidbody2D>();
      
        rb2d.velocity = new Vector2(Random.Range(-itemBounceSpeed * 0.5f, itemBounceSpeed * 0.5f), 0);
    }

    

    void Update()
    {
        // 아이템이 특정 Y 좌표 아래로 내려가면 파괴
        if (transform.position.y < destroyY)
        {
            Debug.Log($"<color=red>아이템이 화면 밖으로 나감! 파괴됨: {gameObject.name}</color>");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 벽과의 충돌 처리: X 또는 Y 방향 속도 반전
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 currentVelocity = rb2d.velocity;
            Vector2 normal = collision.contacts[0].normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // 좌우 벽
            {
                // X 방향 속도를 반전시키고, 수평 속도 크기는 itemBounceSpeed로 고정
                // 수직 속도는 현재 속도를 유지 (중력에 영향받도록)
                rb2d.velocity = new Vector2(-Mathf.Sign(currentVelocity.x) * itemBounceSpeed, currentVelocity.y);
            }
            else // 상하 벽 (바닥이나 천장)
            {
                // Y 방향 속도를 반전시키되, Y 속도의 크기는 현재 속도 크기를 유지
                // 수평 속도는 현재 속도를 유지
                rb2d.velocity = new Vector2(currentVelocity.x, -Mathf.Sign(currentVelocity.y) * Mathf.Abs(currentVelocity.y));
            }
            Debug.Log($"<color=orange>아이템이 벽({collision.gameObject.name})과 충돌! 방향 반전됨!</color>");
            return;
        }

        // 패들과의 충돌 처리: 공 속도 감소 및 아이템 파괴
        if (collision.gameObject.CompareTag("Paddle"))
        {
            BallController ballController = FindFirstObjectByType<BallController>();
            if (ballController != null)
            {
                ballController.ballSpeed -= speedDecreaseAmount;
                if (ballController.ballSpeed < 1f)
                {
                    ballController.ballSpeed = 1f;
                }
                Debug.Log($"<color=blue>속도 감소 아이템 획득!</color> 현재 공 속도: {ballController.ballSpeed}");
            }
            else
            {
                Debug.LogWarning("SpeedDownItemController: 씬에서 BallController를 찾을 수 없습니다.");
            }
            Destroy(gameObject);
        }
    }
}