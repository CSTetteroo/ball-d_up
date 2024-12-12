using UnityEngine;
using UnityEditor;

public class MeshColliderReducerEditor : EditorWindow
{
    [MenuItem("Tools/Mesh Collider Reducer")]
    public static void ShowWindow()
    {
        GetWindow<MeshColliderReducerEditor>("Mesh Collider Reducer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Mesh Collider Reducer", EditorStyles.boldLabel);
        if (GUILayout.Button("Reduce Mesh Colliders"))
        {
            ReduceMeshColliders();
        }
    }

    private void ReduceMeshColliders()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No objects selected!");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            MeshCollider[] meshColliders = obj.GetComponents<MeshCollider>();
            if (meshColliders.Length > 1)
            {
                CombineMeshColliders(obj, meshColliders);
            }
        }

        Debug.Log("Mesh colliders reduced for selected objects!");
    }

    private void CombineMeshColliders(GameObject obj, MeshCollider[] meshColliders)
    {
        // Keep the first mesh collider and remove the rest
        MeshCollider primaryCollider = meshColliders[0];
        Mesh combinedMesh = new Mesh();

        // Combine meshes from all colliders
        CombineInstance[] combine = new CombineInstance[meshColliders.Length];
        for (int i = 0; i < meshColliders.Length; i++)
        {
            if (meshColliders[i].sharedMesh != null)
            {
                combine[i].mesh = meshColliders[i].sharedMesh;
                combine[i].transform = meshColliders[i].transform.localToWorldMatrix;
            }

            if (i > 0)
            {
                DestroyImmediate(meshColliders[i]); // Remove additional mesh colliders
            }
        }

        // Combine into one mesh
        combinedMesh.CombineMeshes(combine, true, true);
        primaryCollider.sharedMesh = combinedMesh;

        Debug.Log($"Reduced mesh colliders on {obj.name}");
    }
}
