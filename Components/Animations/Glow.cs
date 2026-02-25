using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[AddComponentMenu("ComponentsMythic/Animations/Glow (Hierarchy)")]
[DisallowMultipleComponent]
public class GlowAnimation : Animation
{
    public bool loaded = false;
    [SerializeField] public string objectName{get => this.ObjectName; set => this.ObjectName = value;}
    [Header("Targets")]
    [Tooltip("If true, includes disabled (inactive) children as well.")]
    [SerializeField] private bool includeInactiveChildren = true;

    [Tooltip("If true, glow affects every material slot on each renderer. If false, only materialIndex is affected.")]
    [SerializeField] private bool affectAllMaterialSlots = true;

    [Tooltip("Used only when Affect All Material Slots is false.")]
    [SerializeField] private int materialIndex = 0;

    [Header("Glow")]
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor = Color.cyan;

    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private float glowIntensity = 5f;

    [Header("State")]
    [SerializeField] private bool playOnStart = true;

    public bool playing;

    // We store the exact instanced materials we modified, across all renderers in the hierarchy
    private readonly List<Material> instancedMats = new List<Material>(64);

    void Awake()
    {
        RebuildTargets();
        CacheHierarchyMaterials();
        playing = playOnStart;

        if (!playing)
        {
            SetEmission(Color.black);
        }
    }

    public void load()
    {
        this.RebuildTargets();
        this.loaded = true;
    }

    void Update()
    {
        if (!loaded)
        {
            this.load();
        }

        update();
    }

    public override void update()
    {
        if (!playing) return;

        float emission = Mathf.PingPong(Time.time * glowSpeed, glowIntensity);
        SetEmission(glowColor * emission);
    }

    // Call this if you add/remove child objects at runtime and need to refresh
    public void RebuildTargets()
    {
        CacheHierarchyMaterials();
    }

    public void Play() => playing = true;

    public void Stop(bool clearEmission = true)
    {
        playing = false;
        if (clearEmission) SetEmission(Color.black);
    }

    private void CacheHierarchyMaterials()
    {
        instancedMats.Clear();

        // Get all renderers in this object's hierarchy
        Renderer[] renderers = GetComponentsInChildren<Renderer>(includeInactiveChildren);

        for (int r = 0; r < renderers.Length; r++)
        {
            Renderer rend = renderers[r];
            if (rend == null) continue;

            // Instanced copies for this renderer only
            Material[] mats = rend.materials;
            if (mats == null || mats.Length == 0) continue;

            if (affectAllMaterialSlots)
            {
                for (int i = 0; i < mats.Length; i++)
                {
                    Material m = mats[i];
                    if (m == null) continue;

                    m.EnableKeyword("_EMISSION");
                    instancedMats.Add(m);
                }
            }
            else
            {
                int idx = Mathf.Clamp(materialIndex, 0, mats.Length - 1);
                Material m = mats[idx];
                if (m == null) continue;

                m.EnableKeyword("_EMISSION");
                instancedMats.Add(m);
            }
        }
    }

    private void SetEmission(Color emissionColor)
    {
        // Apply to every instanced material we collected
        for (int i = 0; i < instancedMats.Count; i++)
        {
            Material m = instancedMats[i];
            if (m == null) continue;

            m.SetColor("_EmissionColor", emissionColor);
        }
    }
}


[CustomEditor(typeof(GlowAnimation))]
public class GlowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Glow adds a hierarchical emission effect to child renderers.",
            MessageType.Info
        );

        DrawDefaultInspector();
    }
}