using UnityEngine;

[AddComponentMenu("ComponentsMythic/Animations/Hover")]
[DisallowMultipleComponent]
public class HoverAnimation : AnimationBase
{
    [Header("Target")]
    [Tooltip("If null, uses this transform.")]
    [SerializeField] private Transform target;

    [Header("Hover Motion")]
    [SerializeField] private float amplitude = 0.25f;     // how high it bobs
    [SerializeField] private float frequency = 1.5f;      // how fast it bobs
    [SerializeField] private float phaseOffset = 0f;      // randomize per object if desired
    [SerializeField] private bool useLocalSpace = true;   // local bob vs world bob

    [Header("Optional Sway")]
    [SerializeField] private bool sway = true;
    [SerializeField] private float swayDegrees = 5f;      // max tilt
    [SerializeField] private float swayFrequency = 1f;

    private Vector3 startPos;
    private Quaternion startRot;

    public float yOffset;

    void Awake()
    {
        if (target == null) target = transform;

        // Store starting pose
        startPos = useLocalSpace ? target.localPosition : target.position;
        startRot = useLocalSpace ? target.localRotation : target.rotation;

        // If you want automatic variation per instance:
        if (Mathf.Approximately(phaseOffset, 0f))
            phaseOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        update();
    }

    public override void update()
    {
        float t = Time.time + phaseOffset;

        // Bob up and down
        float bob = Mathf.Sin(t * frequency * Mathf.PI * 2f) * amplitude;
        Vector3 offset = new Vector3(0f, bob + this.yOffset, 0f);

        if (useLocalSpace)
            target.localPosition = startPos + offset;
        else
            target.position = startPos + offset;

        // Gentle sway/tilt
        if (sway)
        {
            float swayT = Mathf.Sin(t * swayFrequency * Mathf.PI * 2f) * swayDegrees;
            Quaternion tilt = Quaternion.Euler(swayT, 0f, -swayT * 0.5f);

            if (useLocalSpace)
                target.localRotation = startRot * tilt;
            else
                target.rotation = startRot * tilt;
        }
    }
}