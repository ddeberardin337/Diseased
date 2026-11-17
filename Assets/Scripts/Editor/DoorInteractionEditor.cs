
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(DoorInteraction))]
public class DoorInteractionEditor : Editor
{
    private SerializedProperty scenePath;
    private SerializedProperty spawnPoint;

    private List<string> cachedSpawnPoints = new List<string>();
    private string cachedScenePath = "";

    private void OnEnable()
    {
        scenePath = serializedObject.FindProperty("destinationScenePath");
        spawnPoint = serializedObject.FindProperty("destinationSpawnPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("interactionPrompt"),
            new GUIContent("Interaction Prompt")
        );

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "Door Destination",
            EditorStyles.boldLabel
        );

        SceneAsset currentScene =
            AssetDatabase.LoadAssetAtPath<SceneAsset>(
                scenePath.stringValue
            );

        SceneAsset selectedScene =
            (SceneAsset)EditorGUILayout.ObjectField(
                "Destination Scene",
                currentScene,
                typeof(SceneAsset),
                false
            );

        if (selectedScene != currentScene)
        {
            scenePath.stringValue =
                selectedScene != null
                    ? AssetDatabase.GetAssetPath(selectedScene)
                    : "";

            spawnPoint.stringValue = "";

            cachedSpawnPoints.Clear();
            cachedScenePath = scenePath.stringValue;
        }

        if (!string.IsNullOrEmpty(scenePath.stringValue))
        {
            if (cachedScenePath != scenePath.stringValue)
            {
                cachedScenePath = scenePath.stringValue;
                cachedSpawnPoints.Clear();
            }

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
            {
                if (GUILayout.Button("Refresh Destination Doors"))
                {
                    string previousSelection = spawnPoint.stringValue;

                    cachedSpawnPoints =
                        GetDestinationDoors(scenePath.stringValue);

                    // Keep the previous selection if it still exists.
                    if (!cachedSpawnPoints.Contains(previousSelection))
                    {
                        spawnPoint.stringValue = "";
                    }
                }
            }

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "Destination doors can only be refreshed outside Play Mode.",
                    MessageType.Info
                );
            }

            if (cachedSpawnPoints.Count > 0)
            {
                List<string> options =
                    new List<string> { "-- Select Destination Door --" };

                options.AddRange(cachedSpawnPoints);

                int selectedIndex =
                    options.IndexOf(spawnPoint.stringValue);

                if (selectedIndex < 0)
                    selectedIndex = 0;

                int newIndex = EditorGUILayout.Popup(
                    "Destination Door",
                    selectedIndex,
                    options.ToArray()
                );

                if (newIndex != selectedIndex)
                {
                    spawnPoint.stringValue =
                        newIndex > 0 ? options[newIndex] : "";
                }
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "Click Refresh Destination Doors to discover available doors in the selected scene.",
                    MessageType.Info
                );
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private List<string> GetDestinationDoors(string path)
    {
        List<string> results = new List<string>();

        Scene scene = SceneManager.GetSceneByPath(path);

        bool openedByInspector =
            !scene.IsValid() || !scene.isLoaded;

        if (openedByInspector)
        {
            scene = EditorSceneManager.OpenScene(
                path,
                OpenSceneMode.Additive
            );
        }

        if (scene.IsValid())
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                DoorInteraction[] doors =
                    root.GetComponentsInChildren<DoorInteraction>(true);

                foreach (DoorInteraction door in doors)
                {
                    string doorName = door.gameObject.name;

                    if (!results.Contains(doorName))
                        results.Add(doorName);
                }
            }
        }

        if (openedByInspector && scene.IsValid())
        {
            EditorSceneManager.CloseScene(scene, true);
        }

        results.Sort();

        return results;
    }
}
