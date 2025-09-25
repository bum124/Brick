using System.Collections;
using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    [SerializeField] int hp = 1;
    [SerializeField] public GameObject[] itemPrefabs;
    [Range(0f, 1f)]
    [SerializeField] float itemDropProbability = 0.2f; // ������ ��� Ȯ�� (30%)

    private bool isDestroyed = false;

    void Start()
    {
        BlockManager.Instance?.RegisterBlock();
    }

    public void TakeDamage()
    {
        if (isDestroyed) return;

        hp--;
        Debug.Log($"{gameObject.name} �ǰݵ�! ���� HP: {hp}");

        if (hp <= 0)
        {
            isDestroyed = true;
            Debug.Log($"{gameObject.name} ���ŵ�!");
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

            // ������ �����ۿ� ��������Ʈ ������ �ִٸ� ���� (������ ������ ���� �ٸ� �� ����)
            SpeedDownItemController itemController = droppedItem.GetComponent<SpeedDownItemController>();
            if (itemController != null)
            {
                // ���� BlockComponent�� ��������Ʈ ������ �ִٸ� ������ �� ����
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
            Debug.Log("���̶� �浹��!");
            TakeDamage();
        }
    }
}