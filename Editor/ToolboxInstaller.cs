using UnityEngine;
using UnityEditor;

namespace BUDDYWORKS.ToolBox
{
    public class SpawnToolboxPrefab : MonoBehaviour
    {
        private static string prefabGUID = "0e07d7ed7da2cc1449f6cc1be73f35fe"; //Define prefab to spawn by GUID.
        private static string dependencyPackage = "Packages/com.vrcfury.vrcfury"; //Dependency check for presence of a package, in this case VRCFury.

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Prefab... [VRCFury]", false, 0)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Prefab... [VRCFury]")]
        private static void SpawnToolBoxPrefab() {
            SpawnPrefab(prefabGUID); //Runs SpawnPrefab with the given GUID.
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Prefab... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Prefab... [VRCFury]", true)]
        private static bool ValidateSpawnToolBoxPrefab() {
            return AssetDatabase.IsValidFolder(dependencyPackage) != false; //Checks if the defined package is present.
        }

        private static void SpawnPrefab(string guid)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(guid); //Resolves the asset path, based on the provided guid.
            if (string.IsNullOrEmpty(prefabPath)) { Debug.LogError("[Toolbox] Prefab with GUID " + guid + " not found."); return;} //Checks if prefabPath is empty or null, implies that the asset is not present.
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath); //Loads the Asset into GameObject prefab.
            GameObject selectedObject = Selection.activeGameObject; //Writes the users current selection into selectedObject, causes prefab to spawn at selection if value != null.
            if (prefab == null) {Debug.LogError("[Toolbox] Failed to load prefab with GUID " + guid + " at path " + prefabPath); return;} //Checks if valid data got loaded into prefab.
            GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab); //Instantiates the prefab.
            if (selectedObject != null) {instantiatedPrefab.transform.parent = selectedObject.transform;} //Reparents the spawned prefab if selectedObject is given.
            if (instantiatedPrefab != null) {EditorGUIUtility.PingObject(instantiatedPrefab); return;} //If all went well, the prefab will get pinged to help the user find it.
            Debug.LogError("[Toolbox] Failed to instantiate prefab with GUID " + guid); //Error handler if something went wrong.
        }
    }
}