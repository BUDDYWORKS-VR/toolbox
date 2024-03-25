using UnityEngine;
using UnityEditor;

namespace BUDDYWORKS.ToolBox.AddToHierarchy
{
    public class InstantiateAndPingPrefabByGUID : MonoBehaviour
    {
        public static string prefabGUID = "0e07d7ed7da2cc1449f6cc1be73f35fe";

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Prefab...")]
        public static void SpawnToolBoxPrefab()
        {
            SpawnPrefab(prefabGUID);
        }

        private static void SpawnPrefab(string guid)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError("Prefab with GUID " + guid + " not found.");
                return;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            GameObject selectedObject = Selection.activeGameObject;

            if (prefab == null)
            {
                Debug.LogError("Failed to load prefab with GUID " + guid + " at path " + prefabPath);
                return;
            }

            GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (selectedObject != null)
            {
                instantiatedPrefab.transform.parent = selectedObject.transform;
            }

            if (instantiatedPrefab != null)
            {
                EditorGUIUtility.PingObject(instantiatedPrefab);
            }
            else
            {
                Debug.LogError("Failed to instantiate prefab with GUID " + guid);
            }
        }
    }
}
