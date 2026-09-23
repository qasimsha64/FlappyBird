using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floatSpeed = 4f;


    [Header("Movement")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private Rigidbody2D rg;

    private Vector3 startPosition;


    private void Awake()
    {
       rg = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPosition = Vector3.zero;
        rg.simulated = false;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Home || GameManager.Instance.GameState == GameState.GetReady)
        {
            FloatIdle();
        }
    }
    private void FloatIdle()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void RotateBird()
    {
        float angle = Mathf.Clamp(rg.linearVelocityY * 5f, -90f, 30f);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0,0,angle),
            rotationSpeed*Time.deltaTime
);
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (GameManager.Instance.GameState == GameState.Home || GameManager.Instance.GameState == GameState.GameOver) return;

        if (GameManager.Instance.GameState == GameState.GetReady)
        {
            GameManager.Instance.GamePlay();
            rg.simulated = true;
        }

       rg.linearVelocity = new Vector2(rg.linearVelocity.x, 0f);
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AudioMnager.Instance.Fly();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.GameState == GameState.GameOver) return;
        if (!collision.collider.CompareTag("Obstacles"))
            return;
        AudioMnager.Instance.Hit();
        StartCoroutine(DieSoundDelay());

        GameManager.Instance.Gameover();
    }
    IEnumerator DieSoundDelay()
    {
        yield return new WaitForSeconds(0.3f);
        AudioMnager.Instance.Die();
    }







    public void ResetPlayer()
    {
        rg.simulated = false;
        rg.linearVelocity = Vector2.zero;
        rg.angularVelocity = 0f;
        startPosition = new Vector3(-0, 5f, 0f);
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }

}
