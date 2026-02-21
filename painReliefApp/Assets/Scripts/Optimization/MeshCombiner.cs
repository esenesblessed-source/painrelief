using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCombiner : MonoBehaviour
{
    // Combine meshes of all child MeshFilters into a single mesh to reduce draw calls.
    // Use for static geometry only.
    public void CombineChildMeshes(bool markStatic = true)
    {
        var filters = GetComponentsInChildren<MeshFilter>();
        if (filters == null || filters.Length == 0) return;

        var combine = new UnityEngine.Rendering.CombineInstance[filters.Length];
        int i = 0;
        foreach (var f in filters)
        {
            if (f.sharedMesh == null) continue;
            combine[i].mesh = f.sharedMesh;
            combine[i].transform = f.transform.localToWorldMatrix;
            f.gameObject.SetActive(false);
            i++;
        }

        var newMesh = new Mesh();
        newMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        newMesh.CombineMeshes(combine, true, true);

        var go = new GameObject("CombinedMesh");
        go.transform.SetParent(transform, false);
        var mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = newMesh;
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = filters[0].GetComponent<MeshRenderer>()?.sharedMaterial;

        if (markStatic) go.isStatic = true;
    }

#if UNITY_EDITOR
    [ContextMenu("Combine Child Meshes (Editor)")]
    void CombineContext()
    {
        if (!Application.isPlaying) CombineChildMeshes(true);
        else Debug.Log("Combine should be run in editor mode");
    }
#endif
}
