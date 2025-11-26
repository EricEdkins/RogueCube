using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePointPrefab; // Prefab for additional fire points
    [SerializeField] private float projectileSpeed = 25f;
    [SerializeField] private float fireCooldown = 1f;

    private float lastFireTime = -999f;

    // All current fire points
    private List<Transform> firePoints = new List<Transform>();

    // Singleton-style reference so PlayerHandler can call AddFirePoint()
    public static PlayerCombat Instance;

    private void Awake()
    {
        Instance = this;

        if (firePoints.Count == 0)
        {
            // Spawn the first fire point automatically
            Transform fp = Instantiate(firePointPrefab, transform);
            fp.name = "FirePoint_Start";
            fp.localPosition = Vector3.forward * 0.5f;
            fp.forward = Vector3.forward;
            firePoints.Add(fp);
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        lastFireTime = Time.time;

        foreach (Transform fp in firePoints)
        {
            GameObject proj = Instantiate(projectilePrefab, fp.position, fp.rotation);
            proj.transform.Rotate(90f, 0f, 0f); // tip capsule forward if you’re using capsules
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = fp.forward * projectileSpeed;
        }
    }

    // Called when player levels up
    public void AddFirePoint(int level)
    {
        // Four main directions (horizontal only)
        Vector3[] directions = {
        Vector3.forward,  // front
        Vector3.left,     // left
        Vector3.back,     // back
        Vector3.right     // right
    };

        // Choose which side this FirePoint belongs to
        int faceIndex = (level - 1) % directions.Length;
        Vector3 dir = directions[faceIndex];

        // Count how many already exist on this face
        int sameFaceCount = 0;
        foreach (Transform fp in firePoints)
        {
            if (Vector3.Dot(fp.forward, dir) > 0.9f)
                sameFaceCount++;
        }

        // 🔹 Always use a horizontal perpendicular for spacing (use Vector3.up)
        Vector3 perp = Vector3.Cross(Vector3.up, dir).normalized;

        // Alternate left/right offsets for symmetry
        float offsetStep = 0.3f;
        float offsetAmount = (sameFaceCount % 2 == 0 ? 1 : -1) * (sameFaceCount / 2 + 1) * offsetStep;
        Vector3 sideOffset = perp * offsetAmount;

        // Spawn slightly outside player surface
        Transform newFirePoint = Instantiate(firePointPrefab, transform);
        newFirePoint.name = $"FirePoint_{level}";
        newFirePoint.localPosition = dir * 0.6f + sideOffset;
        newFirePoint.localRotation = Quaternion.LookRotation(dir, Vector3.up);

        firePoints.Add(newFirePoint);
    }
    


}
