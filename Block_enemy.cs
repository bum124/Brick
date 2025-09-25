using JetBrains.Annotations;
using UnityEngine;

public class Block_enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int row;
    [SerializeField] private int col;

    void Start()
    {
        CreateBlock();
        Debug.Log("�� �̸���: " + gameObject.name);
    }

    void CreateBlock()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Vector2 spawnPos = new Vector2(
                    pos.x + (j * offset.x),
                    pos.y - (i * offset.y)
                );

                int randomIndex = Random.Range(0, blocks.Length);
                GameObject brick = Instantiate(blocks[randomIndex], spawnPos, Quaternion.identity);
            }
        }
    }

    void Update()
    {
        // ���⿡ �ʿ� �� ������Ʈ ���� �߰�
    }
}