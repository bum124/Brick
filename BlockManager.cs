using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    private int totalBlocks = 0;

    public static BlockManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterBlock()
    {
        totalBlocks++;
    }

    public void UnregisterBlock()
    {
        totalBlocks--;
        if (totalBlocks <= 0)
        {
            Debug.Log("모든 블록 제거됨 → 클리어!");
            SceneManager.LoadScene("ClearScene");
        }
    }
}