using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;
    [SerializeField] private float speed;
    [SerializeField] private float jumpDuration;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scaleValue;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float StartDamping;
    [SerializeField] private StatsManager statsManager;
    [SerializeField] private PushPlayer pushPlayer;
    private Vector3 moveDirection;
    public Rigidbody2D rb;
    public float Cnockback;
    public bool vulnerable;
    IEnumerator jump()
    {
        vulnerable = false;
        statsManager.gameObject.SetActive(false);
        pushPlayer.gameObject.SetActive(false);
        rb.excludeLayers += LayerMask.GetMask("Row");
        rb.excludeLayers += LayerMask.GetMask("Tree");
        rb.excludeLayers += LayerMask.GetMask("FallenTree");

        float elapsedTime = 0f;

        float startCameraSize = mainCamera.orthographicSize;
        float startSize = transform.localScale.x;

        float tempCameraSize = mainCamera.orthographicSize;
        float tempSize = transform.localScale.x;
        float tempDamping = rb.linearDamping;
        rb.linearDamping = StartDamping;

        float targetCameraSize = cameraHeight;
        float targetSize = scaleValue;

        while (elapsedTime < (jumpDuration / 2))
        {
            mainCamera.orthographicSize = Mathf.Lerp(startCameraSize, targetCameraSize, elapsedTime / (jumpDuration / 2));
            float addition = Mathf.Lerp(startSize, targetSize, elapsedTime / (jumpDuration / 2));
            transform.localScale = new Vector3(addition, addition, addition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = targetCameraSize;
        transform.localScale = new Vector3(targetSize, targetSize, targetSize);


        elapsedTime = 0f;

        startCameraSize = targetCameraSize;
        targetCameraSize = tempCameraSize;

        startSize = targetSize;
        targetSize = tempSize;

        while (elapsedTime < (jumpDuration / 2))
        {
            mainCamera.orthographicSize = Mathf.Lerp(startCameraSize, targetCameraSize, elapsedTime / (jumpDuration / 2));
            float addition = Mathf.Lerp(startSize, targetSize, elapsedTime / (jumpDuration / 2));
            transform.localScale = new Vector3(addition, addition, addition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = targetCameraSize;
        transform.localScale = new Vector3(targetSize, targetSize, targetSize);

        rb.linearDamping = tempDamping;
        statsManager.gameObject.SetActive(true);
        pushPlayer.gameObject.SetActive(true);
        rb.excludeLayers -= LayerMask.GetMask("Row");
        rb.excludeLayers -= LayerMask.GetMask("Tree");
        rb.excludeLayers -= LayerMask.GetMask("FallenTree");
        vulnerable = true;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        statsManager = GetComponentInChildren<StatsManager>();
        pushPlayer = GetComponentInChildren<PushPlayer>();
        mainCamera = Camera.main;
        vulnerable = true;
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(inputX, inputY);

        Jump();
        Rotate();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * speed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.z = 0;
        Vector3 sawDir = (transform.position - worldMousePosition).normalized;
        Vector3 rot = Quaternion.Euler(0, 0, 270) * sawDir;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, rot);
    }
    
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && vulnerable)
            StartCoroutine(jump());
    }

    public void Push(Vector2 dir, float speed)
    {
        rb.AddForce(dir * speed * Cnockback, ForceMode2D.Impulse);
    }

}