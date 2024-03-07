using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Shader_Util : MonoBehaviour
{
    // OutIine
    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }
    private Mode outlineMode_ = Mode.OutlineAll;
    public  Mode outlineMode
    {
        get { return outlineMode_; }
        set
        {
            outlineMode_ = value;
            needsUpdate = true;
        }
    }

    //private Color outlineColor = Color.white;
    private Color outlineColor_ = new Color32(255, 0, 45, 200);
    public Color outlineColor
    {
        get { return outlineColor_; }
        set
        {
            outlineColor_ = value;
            needsUpdate = true;
        }
    }

    private float outlineWidth_ = 2f;
    public float outlineWidth
    {
        get { return outlineWidth_; }
        set
        {
            outlineWidth_ = value;
            needsUpdate = true;
        }
    }

    bool isOutlineEnable = false;
    bool needsUpdate     = true; // Materia 특성 즉시 적용 여부
    //bool precomputeOutline = true;

    Material outlineMaskMaterial = null;
    Material outlineFillMaterial = null;

    Renderer[] renderers;
    static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
    class ListVector3 { public List<Vector3> data; }
    List<Mesh> bakeKeys = new List<Mesh>();
    List<ListVector3> bakeValues = new List<ListVector3>();


    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // Outline materials
        outlineMaskMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineMask"));
        outlineFillMaterial = Instantiate(Resources.Load<Material>("Materials/OutlineFill"));
        outlineMaskMaterial.name = "OutlineMask (Instance)";
        outlineFillMaterial.name = "OutlineFill (Instance)";

        LoadSmoothNormals();

        isOutlineEnable = false;
        needsUpdate = true;
    }

    void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;
            UpdateMaterialProperties();
        }
    }

    void OnDestroy()
    {
        string shaderName = "Standard";
        SetMateriasColorTransparency(1f, shaderName);

        // Destroy material instances
        Destroy(outlineMaskMaterial);
        Destroy(outlineFillMaterial);
    }

    //void OnEnable()
    //{
    //    OnEnableOutline();
    //}

    //void OnDisable()
    //{
    //    OnDisableOutline();
    //}

    //void OnValidate()
    //{

    //    // Update material properties
    //    needsUpdate = true;

    //    // Clear cache when baking is disabled or corrupted
    //    if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
    //    {
    //        bakeKeys.Clear();
    //        bakeValues.Clear();
    //    }

    //    // Generate smooth normals when baking is enabled
    //    if (precomputeOutline && bakeKeys.Count == 0)
    //    {
    //        Bake();
    //    }
    //}

    #region Shader

    [Obsolete("임시")]
    public void SetMateriasColorAlpha(float pAlpha, bool pOn)
    {
        OnDisableOutline();

        string shaderName = (pOn) ? "UI/Unlit/Transparent" : "Standard";
        SetMateriasColorTransparency(pAlpha, shaderName);
    }

    void SetMateriasColorTransparency(float pAlpha, string pShaderName)
    {
        if (renderers == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        Shader shader = Shader.Find(pShaderName);
        if (shader == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }


        Material material;
        Color color;
        foreach (Renderer renderer in renderers)
        {
            material = new Material(renderer.sharedMaterial);
            material.shader = shader;

            color = material.color;
            color.a = color.a = pAlpha;
            material.color = color; ;
            renderer.sharedMaterial = material;
        }
    }

    public void OnEnableOutline()
    {
        if (isOutlineEnable)
            return;

        isOutlineEnable = true;
        foreach (var renderer in renderers)
        {

            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    public void OnDisableOutline()
    {
        if (isOutlineEnable == false)
            return;

        isOutlineEnable = false;
        foreach (var renderer in renderers)
        {

            // Remove outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    void UpdateMaterialProperties()
    {
        // Apply properties according to mode
        outlineFillMaterial.SetColor("_OutlineColor", outlineColor_);

        switch (outlineMode_)
        {
            case Mode.OutlineAll:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth_);
                break;

            case Mode.OutlineVisible:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth_);
                break;

            case Mode.OutlineHidden:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth_);
                break;

            case Mode.OutlineAndSilhouette:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth_);
                break;

            case Mode.SilhouetteOnly:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                break;
        }
    }

    #endregion Shader

    #region Load

    /// <summary>
    /// Smooth 법선을 로드합니다.
    /// </summary>
    void LoadSmoothNormals()
    {

        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {

            // Skip if UV3 has already been reset
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Combine submeshes
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {

        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    void Bake()
    {
        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {

            // Skip duplicates
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Serialize smooth normals
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(new ListVector3() { data = smoothNormals });
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {
        if (!mesh.isReadable)
        {
            Debug.LogWarning("Failed : Mesh is not readable. Enable 'Read/Write Enabled' in import settings.");
            return null;
        }

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    #endregion Load
}