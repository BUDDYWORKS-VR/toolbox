using UnityEngine;
using UnityEditor;

namespace BUDDYWORKS.ToolBox
{
    public class PrefabSpawner : MonoBehaviour
    {
        private static string legacyPrefabGUID = "0e07d7ed7da2cc1449f6cc1be73f35fe";

        private static string nameplateviewerPrefabGUID = "c6fad3e9a3186d344945448868fa1dbc";
        private static string lightsPrefabGUID = "11c4b6c2f5fe1bf4d81189e556188a8f";
        private static string wireframePrefabGUID = "94604ea2856ebe54f890f6a144cf8c3e";
        private static string greenscreenPrefabGUID = "00793a8c49981c84ca63b04b4734ed3c";

        private static string dependencyPackage = "Packages/com.vrcfury.vrcfury"; //Dependency check for presence of a package, in this case VRCFury.

        [MenuItem("BUDDYWORKS/Toolbox/Spawn All Prefabs... [VRCFury]", false, 0)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn All Prefabs... [VRCFury]")]
        private static void SpawnAllPrefabs() {
            var prefabGUIDs = new[] { nameplateviewerPrefabGUID, lightsPrefabGUID, wireframePrefabGUID, greenscreenPrefabGUID };
            foreach (var guid in prefabGUIDs)
            {
                SpawnPrefab(guid);
            }
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn NameplateViewer Prefab... [VRCFury]", false, 11)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn NameplateViewer Prefab... [VRCFury]")]
        private static void SpawnNPVPrefab() {
            SpawnPrefab(nameplateviewerPrefabGUID);
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Lights Prefab... [VRCFury]", false, 12)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Lights Prefab... [VRCFury]")]
        private static void SpawnLightsPrefab() {
            SpawnPrefab(lightsPrefabGUID);
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Wireframe Prefab... [VRCFury]", false, 13)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Wireframe Prefab... [VRCFury]")]
        private static void SpawnWireframePrefab() {
            SpawnPrefab(wireframePrefabGUID);
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn Greenscreen Prefab... [VRCFury]", false, 13)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Greenscreen Prefab... [VRCFury]")]
        private static void SpawnGreenscreenPrefab() {
            SpawnPrefab(greenscreenPrefabGUID);
        }

        [MenuItem("BUDDYWORKS/Toolbox/Spawn All Prefabs... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn All Prefabs... [VRCFury]", true)]
        [MenuItem("BUDDYWORKS/Toolbox/Spawn NameplateViewer Prefab... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn NameplateViewer Prefab... [VRCFury]", true)]
        [MenuItem("BUDDYWORKS/Toolbox/Spawn Lights Prefab... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Lights Prefab... [VRCFury]", true)]
        [MenuItem("BUDDYWORKS/Toolbox/Spawn Wireframe Prefab... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Wireframe Prefab... [VRCFury]", true)]
        [MenuItem("BUDDYWORKS/Toolbox/Spawn Greenscreen Prefab... [VRCFury]", true)]
        [MenuItem("GameObject/BUDDYWORKS/Toolbox/Spawn Greenscreen Prefab... [VRCFury]", true)]
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