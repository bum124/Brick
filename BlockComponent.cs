using System.Collections;
using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    [SerializeField] int hp = 1;
    [SerializeField] public GameObject[] itemPrefabs;
    [Range(0f, 1f)]
    [SerializeField] float itemDropProbability = 0.2f; // 아이템 드롭 확률 (30%)

    private bool isDestroyed = false;

    void Start()
    {
        BlockManager.Instance?.RegisterBlock();
    }

    public void TakeDamage()
    {
        if (isDestroyed) return;

        hp--;
        Debug.Log($"{gameObject.name} 피격됨! 현재 HP: {hp}");

        if (hp <= 0)
        {
            isDestroyed = true;
            Debug.Log($"{gameObject.name} 제거됨!");
            DropItem();
            StartCoroutine(Death());
        }
    }

    void DropItem()
    {
        if (itemPrefabs.Length > 0 && Random.value < itemDropProbability)
        {
            int rand = Random.Range(0, itemPrefabs.Length);
            GameObject droppedItem = Instantiate(itemPrefabs[rand], transform.position, Quaternion.identity);
            Debug.Log("Item dropped at: " + transform.position);

            // 생성된 아이템에 스프라이트 정보가 있다면 전달 (프리팹 설정에 따라 다를 수 있음)
            SpeedDownItemController itemController = droppedItem.GetComponent<SpeedDownItemController>();
            if (itemController != null)
            {
                // 만약 BlockComponent에 스프라이트 정보가 있다면 전달할 수 있음
                // itemController.itemSprite = ...;
            }
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.01f);
        BlockManager.Instance?.UnregisterBlock();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("공이랑 충돌함!");
            TakeDamage();
        }
    }
}