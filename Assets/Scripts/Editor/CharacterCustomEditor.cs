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
    protected SerializedProperty inventory;
    protected SerializedProperty MaxHealth;
    protected SerializedProperty Health;
    private CharacterSO _targetSO;
    protected CharacterSO TargetSO {
        get { //! only directly access this when necessary, and only access its methods
            UnityEngine.Debug.LogWarning("Editor accessing targetSO directly, are you sure you need to be doing this?");
            return _targetSO;} 
    }                                    
    bool showAttributes = false;
    bool showSkills = false;
    bool showInv = false;
    public bool debug;
    private void OnEnable() {
        Name = serializedObject.FindProperty("_name");
        Attributes = serializedObject.FindProperty("Attributes");
        Skills = serializedObject.FindProperty("Skills");
        MaxHealth = serializedObject.FindProperty("_maxHealth");
        Health = serializedObject.FindProperty("_health");
        inventory = serializedObject.FindProperty("inventory");
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
            foreach (SerializedProperty attrib in Attributes) {
                string path = attrib.propertyPath;
                SerializedProperty attribName = FindChildProperty(attrib, "_name");
                SerializedProperty attribValue = FindChildProperty(attrib, "_value");
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
            foreach (SerializedProperty skill in Skills) {
                string path = skill.propertyPath;
                SerializedProperty skillName = FindChildProperty(skill, "_name");
                SerializedProperty skillValue = FindChildProperty(skill, "_value");
                skillValue.intValue = EditorGUILayout.IntSlider(skillName.stringValue, skillValue.intValue, skillMin, skillMax);
            }
            GUILayout.EndVertical();
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        showInv = EditorGUILayout.BeginFoldoutHeaderGroup(showInv, "Inventory");
        if(showInv) {
            foreach (SerializedProperty itemEntry in FindChildProperty(inventory,"contents")) {
                SerializedProperty Item = FindChildProperty(itemEntry,"Key");
                SerializedProperty Qty = FindChildProperty(itemEntry,"Value");
            }     
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        if(debug){ 
            if(GUILayout.Button("Initialize")) {
                if(UnityEditor.EditorUtility.DisplayDialog( $"Initialize {Name.stringValue}?",
                    "This will reset all values to that of a default CharacterSO. Are you sure you want to do this?",
                    "Initialize","Cancel"
                )
                ) 
                    {InitializeCharacterSO();
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
    //probably needs input handeling 
    SerializedProperty FindChildProperty(SerializedProperty parent, string path) {;
        string targetPath = $"{parent.propertyPath}.{path}";
        DebugMessage($"Looking up property at {targetPath}");
        if(serializedObject.FindProperty(targetPath) == null) {throw new System.Exception("child property not found. Is it marked as Serializable?");}
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