using TMPro;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("Experience")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;

    [Header("Blink Settings")]
    [SerializeField] private float blinkDistance = 5f;
    [SerializeField] private float blinkCooldown = 2f;
    private float lastBlinkTime = -999f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private Vector3 lastMoveDirection = Vector3.forward;

    private int extraJumps = 2;
    private int currentJumps = 0;
    private int points = 0;
    private int coins = 0;
    private int coinThreshold = 3;

    private Collider playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    private void UpdateHUD()
    {
        pointsText.text =
            "Points: " + points +
            "\nCoins: " + coins +
            "\nExtra Jumps: " + extraJumps +
            "\nSpeed: " + playerSpeed +
            "\nLevel: " + level +
            "\nXP: " + currentXP + " / " + xpToNextLevel;
    }

    public void IncreasePoints()
    {
        coins++;
        points++;
        UpdateHUD();
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        UpdateHUD();
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;

        // 🔹 XP scaling rule:
        // Levels 1–9: +100 XP each level
        // Levels 10+: +1000 XP each level
        if (level < 10)
            xpToNextLevel += 100;
        else
            xpToNextLevel += 1000;

        // Reward scaling (still tweakable)
        playerSpeed += 0f;
        blinkDistance += 0f;

        if (PlayerCombat.Instance != null)
            PlayerCombat.Instance.AddFirePoint(level);

        Debug.Log($"Level Up! You’re now level {level}! Next level requires {xpToNextLevel} XP.");
        UpdateHUD();
    }



    void IncreaseJumps()
    {
        coins -= coinThreshold;
        extraJumps++;
        UpdateHUD();
    }

    void IncreaseSpeed()
    {
        coins -= coinThreshold;
        playerSpeed += 5f;
        UpdateHUD();
    }

    void Update()
    {
        if (rb.position.y < -25)
        {
            GameManager.Instance.GameOver();
            return;
        }

        if (rb.position.y <= 1.005f)
            currentJumps = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentJumps < extraJumps)
            {
                currentJumps++;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 5f, rb.linearVelocity.z);
            }
        }

        if (coins >= coinThreshold)
        {
            if (Input.GetKeyDown(KeyCode.J)) IncreaseJumps();
            if (Input.GetKeyDown(KeyCode.M)) IncreaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.B) && Time.time >= lastBlinkTime + blinkCooldown)
        {
            BlinkForward();
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        if (moveDir.magnitude > 0.1f)
        {
            lastMoveDirection = moveDir.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastMoveDirection), 0.2f);
            rb.MovePosition(rb.position + moveDir.normalized * playerSpeed * Time.fixedDeltaTime);
        }
    }

    void BlinkForward()
    {
        Vector3 blinkDirection = lastMoveDirection;
        if (blinkDirection.magnitude < 0.1f)
            blinkDirection = transform.forward;

        Vector3 targetPos = rb.position + blinkDirection * blinkDistance;

        playerCollider.enabled = false;
        rb.position = targetPos;
        Invoke(nameof(ReenableCollider), 0.05f);

        lastBlinkTime = Time.time;
    }

    void ReenableCollider()
    {
        playerCollider.enabled = true;
    }
}
