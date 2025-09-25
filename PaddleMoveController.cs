using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PaddleMoveController : MonoBehaviour
{
    [SerializeField] private AudioClip blockHitSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    public float speed = 6;
    float x = 0;

    private Vector3 paddlePosition;

    // Start is called before the first frame update
    void Start()
    {
        paddlePosition = transform.position;


        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        PaddleMove();
    }
    private void PaddleMove()
    {
        paddlePosition.x += x * speed * Time.deltaTime;
        paddlePosition.x = Mathf.Clamp(paddlePosition.x, -1.9f, 1.9f);
        transform.position = paddlePosition;
    }
    public void ModifySpeed(float multiplier)
    {
        speed *= multiplier;
    }

    public void Enlarge()
    {
        transform.localScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            audioSource.PlayOneShot(blockHitSound);
        }
    }
}