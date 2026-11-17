

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    private SerializedProperty goodEndingScene;
    private SerializedProperty badEndingScene;

    private void OnEnable()
    {
        goodEndingScene =
            serializedObject.FindProperty("goodEndingScene");

        badEndingScene =
            serializedObject.FindProperty("badEndingScene");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(
            "Ending Scene Selection",
            EditorStyles.boldLabel
        );

        DrawSceneField("Good Ending", goodEndingScene);
        DrawSceneField("Bad Ending", badEndingScene);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSceneField(
        string label,
        SerializedProperty property)
    {
        SceneAsset currentScene = null;

        if (!string.IsNullOrEmpty(property.stringValue))
        {
            currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(
                property.stringValue
            );
        }

        SceneAsset selectedScene =
            (SceneAsset)EditorGUILayout.ObjectField(
                label,
                currentScene,
                typeof(SceneAsset),
                false
            );

        if (selectedScene != currentScene)
        {
            property.stringValue = selectedScene != null
                ? AssetDatabase.GetAssetPath(selectedScene)
                : "";
        }
    }
}
