using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 속성 설정")]
    public float moveSpeed;        // 좌우 이동 속도
    public float jumpForce;        // 점프 힘
    private Rigidbody2D rb;
    public float airMoveSpeed; // 공중에서의 방향 변경 속도

    [Header("점프 관련 설정값")]
    public bool isGrounded = false;    // 땅에 닿았는지 여부
    public Transform groundCheck;       // 땅 체크 포인트
    public float checkRadius; // 땅 체크 범위
    public LayerMask groundLayer;
    public float maxRotationSpeed;  // 회전 속도 제한
    private bool wasGroundedLastFrame;

    [Header("점프 쿨타임 설정")]
    public float jumpCooldown; // 점프 쿨타임 0.5초
    private float lastJumpTime = -Mathf.Infinity;

    [Header("Audio")]
    public AudioClip[] footstepSounds;
    private AudioSource audioSource;
    
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // 물리 상태 초기화
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.g_State == gameState.Setting)
        {
            return;
        }

        audioSource.volume = SoundManager.Instance.SfxVolume;

        float moveInput = Input.GetAxisRaw("Horizontal");

        // 점프 쿨타임 체크
        bool canJump = (Time.time - lastJumpTime) >= jumpCooldown;

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            lastJumpTime = Time.time; // 마지막 점프 시간 갱신
        }

        // 이동 처리
        if (isGrounded)
        {
            // 땅에서는 회전력을 줌
            rb.AddTorque(-moveInput * moveSpeed);
        }
        else
        {
            // 공중에서는 velocity.x로 좌우 이동 제어
            rb.velocity = new Vector2(moveInput * airMoveSpeed, rb.velocity.y);
        }
    }

    void FixedUpdate()
    {
        float elapsed = Time.time - spawnTime;
        
        wasGroundedLastFrame = isGrounded;

        if (elapsed > 0.2f) // 씬 시작 0.2초 후부터 체크
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }
        else
        {
            isGrounded = false; // 임의로 false 처리
        }

        // 회전 속도 제한 (공중에서 너무 빠르게 회전하지 않도록)
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxRotationSpeed, maxRotationSpeed);
    
        // 땅에 새로 닿았을 때 소리 재생
        if (!wasGroundedLastFrame && isGrounded)
        {
            PlayRandomFootstep();
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index]);
        }
    }
}
