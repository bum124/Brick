using UnityEngine;
using UnityEngine.SceneManagement;
public class Test : MonoBehaviour
{
    public string gameSceneName = "GameStart";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
