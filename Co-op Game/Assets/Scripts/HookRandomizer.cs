using UnityEngine;

public class HookRandomizer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Vertical Settings")]
    public float minY = -1.29f;
    public float maxY = 9f;
    public float verticalCooldown = 1f; // cooldown in seconden na omhoog/omlaag

    [Header("Horizontal Settings")]
    public float horizontalRange = 2f;      // groter bereik voor meer links/rechts
    public float horizontalSpeedMin = 0.5f; // minimale horizontale snelheid
    public float horizontalSpeedMax = 2f;   // maximale horizontale snelheid

    [Header("Spacing")]
    public float minDistance = 1f; // minimale afstand tot andere haken

    private Vector3 startPos;
    private float verticalDirection;
    private float verticalSpeed;

    private Vector3 horizontalTarget;
    private float horizontalSpeed;

    private float verticalCooldownTimer = 0f;

    private HookRandomizer[] allHooks;

    void Start()
    {
        startPos = transform.position;

        verticalDirection = Random.value < 0.5f ? -1f : 1f;
        verticalSpeed = Random.Range(minSpeed, maxSpeed);

        ChooseNewHorizontalTarget();

        // Haal alle haken in de scene op zodat we afstand kunnen controleren
        allHooks = FindObjectsByType<HookRandomizer>(FindObjectsSortMode.None);

        // Zorg dat startpositie minimaal minDistance van andere haken is
        foreach (var hook in allHooks)
        {
            if (hook == this) continue;
            if (Vector3.Distance(transform.position, hook.transform.position) < minDistance)
            {
                transform.position += new Vector3(minDistance, 0, 0);
            }
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Cooldown aftellen
        if (verticalCooldownTimer > 0f)
        {
            verticalCooldownTimer -= Time.deltaTime;
        }
        else
        {
            // Verticaal bewegen alleen als cooldown voorbij is
            pos.y += verticalDirection * verticalSpeed * Time.deltaTime;

            // Check verticale grenzen
            if (pos.y >= maxY)
            {
                pos.y = maxY;
                verticalDirection = -1f;
                verticalCooldownTimer = verticalCooldown;
                ChooseNewHorizontalTarget();
            }
            else if (pos.y <= minY)
            {
                pos.y = minY;
                verticalDirection = 1f;
                verticalCooldownTimer = verticalCooldown;
                ChooseNewHorizontalTarget();
            }
        }

        // Horizontaal bewegen richting target
        pos = Vector3.MoveTowards(pos, horizontalTarget, horizontalSpeed * Time.deltaTime);

        // Controleer minimale afstand tot andere haken
        foreach (var hook in allHooks)
        {
            if (hook == this) continue;
            if (Vector3.Distance(pos, hook.transform.position) < minDistance)
            {
                pos = transform.position; // stop beweging als te dichtbij
                break;
            }
        }

        transform.position = pos;

        // Als horizontaal target bereikt is, kies een nieuwe
        if (Vector3.Distance(pos, horizontalTarget) < 0.01f)
        {
            ChooseNewHorizontalTarget();
        }
    }

    void ChooseNewHorizontalTarget()
    {
        float newX = startPos.x + Random.Range(-horizontalRange, horizontalRange);
        horizontalTarget = new Vector3(newX, transform.position.y, 0);
        horizontalSpeed = Random.Range(horizontalSpeedMin, horizontalSpeedMax);
    }
}