using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(CharacterSO))]
public class CharacterCustomEditor : Editor {
    protected SerializedProperty Attributes;
    protected SerializedProperty Name;
    protected SerializedProperty Skills;
    protected SerializedProperty MaxHealth;
    protected SerializedProperty Health;
    private CharacterSO _targetSO;
    protected CharacterSO TargetSO {
        get { //! only directly access this when necessary
            UnityEngine.Debug.LogWarning("Editor accessing targetSO directly, are you sure you need to be doing this?");
            return _targetSO;} 
    }                                    
    bool showAttributes = true;
    bool showSkills = true;
    public bool debug;
    private void OnEnable() {
        Name = serializedObject.FindProperty("_name");
        Attributes = serializedObject.FindProperty("Attributes");
        Skills = serializedObject.FindProperty("Skills");
        MaxHealth = serializedObject.FindProperty("_maxHealth");
        Health = serializedObject.FindProperty("_health");
        _targetSO = (CharacterSO)serializedObject.targetObject;
    }
    public override void OnInspectorGUI() {
        /*if(GUILayout.Button("Open Editor Window")) {
            CharacterCustomEditorWindow.Open((CharacterSO)target);
        }*/
        //DrawDefaultInspector();
        serializedObject.Update();
        debug = EditorGUILayout.Toggle("Debug Editor",debug);
        GUILayout.Label(Name.stringValue ?? "Error Name Null");
        EditorGUILayout.Slider("Health", Health.floatValue, 0, MaxHealth.floatValue);
        showAttributes = EditorGUILayout.BeginFoldoutHeaderGroup(showAttributes, "Attributes");
        if(showAttributes) {
            int attribMin = 1;
            int attribMax = 10;
            GUILayout.BeginVertical();
            foreach (SerializedProperty item in Attributes) {
                string path = item.propertyPath;
                SerializedProperty attribName = FindChildProperty(item, "_name");
                SerializedProperty attribValue = FindChildProperty(item, "_value");
                attribValue.intValue = EditorGUILayout.IntSlider(attribName.stringValue, attribValue.intValue, attribMin, attribMax);
            }
            GUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        showSkills = EditorGUILayout.BeginFoldoutHeaderGroup(showSkills, "Skills");
        if(showSkills) {
            int skillMin = 1;
            int skillMax = 100;
            GUILayout.BeginVertical();
            foreach (SerializedProperty item in Skills) {
                string path = item.propertyPath;
                SerializedProperty skillName = FindChildProperty(item, "_name");
                SerializedProperty skillValue = FindChildProperty(item, "_value");
                skillValue.intValue = EditorGUILayout.IntSlider(skillName.stringValue, skillValue.intValue, skillMin, skillMax);
            }
            GUILayout.EndVertical();
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        if(GUILayout.Button("Initialize")) {InitializeCharacterSO();}
        serializedObject.ApplyModifiedProperties();
    }
    //probably needs input handeling 
    SerializedProperty FindChildProperty(SerializedProperty parent, string path) {
        string targetPath = $"{parent.propertyPath}.{path}";
        DebugMessage($"Looking up property at {targetPath}");
        return serializedObject.FindProperty(targetPath);    
    }
    void DebugMessage(string msg) {
        if(debug) {UnityEngine.Debug.Log(msg);}
    }
    void InitializeCharacterSO() {
        TargetSO.Init();
    }
}
/*public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line) {
        CharacterSO so = EditorUtility.InstanceIDToObject(instanceId) as CharacterSO;
        if (so != null)
        {
            CharacterCustomEditorWindow.Open(so);
            return true;
        }
        return false;
    }
}*/