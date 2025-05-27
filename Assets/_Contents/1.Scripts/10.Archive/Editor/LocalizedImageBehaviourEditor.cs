using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.ResourceManagement.AsyncOperations;

[CustomEditor(typeof(LocalizedImageBehaviour))]
public class LocalizedImageBehaviourEditor : Editor
{
    SerializedProperty targetElementNameProp;
    SerializedProperty localizedTextureProp;

    void OnEnable()
    {
        targetElementNameProp = serializedObject.FindProperty("targetElementName");
        localizedTextureProp = serializedObject.FindProperty("localizedTexture");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetElementNameProp, new GUIContent("UI Element Name"));
        EditorGUILayout.PropertyField(localizedTextureProp, new GUIContent("Localized Texture"));

        serializedObject.ApplyModifiedProperties();
    }
}
