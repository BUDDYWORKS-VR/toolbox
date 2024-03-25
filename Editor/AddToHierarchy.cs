using UnityEngine;
using UnityEditor;

// Warning: This code has been written using ChatGPT.
// While it behaves just fine, 100% correctness can't be guaranteed.

namespace BUDDYWORKS.Toolbox.AddToHierarchy
{
public class InstantiateAndPingPrefabByGUID : MonoBehaviour
{
    public static string prefabGUID = "0e07d7ed7da2cc1449f6cc1be73f35fe";

    [MenuItem("BUDDYWORKS/Toolbox/Spawn Prefab...")]
    public static void Start()
    {
        // Get prefab path from GUID
        string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);

        if (string.IsNullOrEmpty(prefabPath))
        {
            Debug.LogError("Prefab with GUID " + prefabGUID + " not found.");
            return;
        }

        // Load prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        GameObject selectedObject = Selection.activeGameObject;

        if (prefab == null)
        {
            Debug.LogError("Failed to load prefab with GUID " + prefabGUID + " at path " + prefabPath);
            return;
        }

        // Instantiate to selection if possible
            if (selectedObject != null)
            {
                GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instantiatedPrefab.transform.parent = selectedObject.transform;
                EditorGUIUtility.PingObject(instantiatedPrefab);
            }

            else
            {

        // Instantiate the prefab as a prefab
        GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        // Ping the newly instantiated prefab in the Unity Editor
        if (instantiatedPrefab != null)
        {
            EditorGUIUtility.PingObject(instantiatedPrefab);
        }
        else
        {
            Debug.LogError("Failed to instantiate prefab with GUID " + prefabGUID);
        }
      }
    }
  }
}