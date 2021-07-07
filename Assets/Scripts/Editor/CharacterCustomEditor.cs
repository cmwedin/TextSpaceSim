using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomEditor(typeof(CharacterSO))]
public class CharacterCustomEditor : Editor {
    protected SerializedProperty Attributes;
    protected SerializedProperty Name;
    protected SerializedProperty Skills;
    protected SerializedProperty inventory;
    protected SerializedProperty InventoryContents;
    protected SerializedProperty MaxHealth;
    protected SerializedProperty Health;
    private CharacterSO _targetSO;
    protected CharacterSO TargetSO {
        get { //! only directly access this when necessary, and only access its methods
            //UnityEngine.Debug.LogWarning("Editor accessing targetSO directly, are you sure you need to be doing this?");
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
        InventoryContents = FindChildProperty(inventory,"contents");
        _targetSO = (CharacterSO)serializedObject.targetObject;
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();
        // ? turns on console callouts of property lookup paths and enables the initialize button
        debug = EditorGUILayout.Toggle("Debug Editor",debug);
        // * Name & Health * //
        GUILayout.Label(Name.stringValue ?? "Error Name Null");
        EditorGUILayout.Slider("Health", Health.floatValue, 0, MaxHealth.floatValue);
        
        // * Attributes Section * //
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
        
        // * Skills Section * //
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
        // ! attempting to code this section through serialized objects and properties became a productivity black hole ! //
        // ! A record of which is recorded below ! //
        // ! for now i am going to do it the dirty way so i can move on to something else ! //
        // * Inventory Section * //
        EditorGUILayout.EndFoldoutHeaderGroup();
        showInv = EditorGUILayout.BeginFoldoutHeaderGroup(showInv, "Inventory");
        if(showInv) {
            // * button to add an item to the inventory
            if (GUILayout.Button("Add Item")) {
                string itemPath = EditorUtility.OpenFilePanel("Select Item","Assets/ScriptableObjects/Items/","asset");
                if (itemPath.Length == 0) throw new UnityException("Error loaded item path empty");
                Item item = AssetDatabase.LoadAssetAtPath<Item>(itemPath.Substring(itemPath.IndexOf("Assets")));
                item.GiveTo(TargetSO.inventory);
            }
            List<Item> Items = TargetSO.inventory.contents.Keys.ToList();
            List<int> Qtys = TargetSO.inventory.contents.Values.ToList();      
            if(Items.Count == 0) {GUILayout.Label("Inventory Empty");}
            else for (int _ = 0; _ < Items.Count; _ ++) {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{Items[_].Name}");
                Qtys[_] = EditorGUILayout.IntField(Qtys[_]);
                GUILayout.EndHorizontal();
                //! this is bad
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        //TODO Fix this
        /* // * Broken Inventory Section * //
        EditorGUILayout.EndFoldoutHeaderGroup();
        showInv = EditorGUILayout.BeginFoldoutHeaderGroup(showInv, "Inventory");
        if(showInv) {
            // * button to add an item to the inventory
            if (GUILayout.Button("Add Item")) {
                string itemPath = EditorUtility.OpenFilePanel("Select Item","Assets/ScriptableObjects/Items/","asset");
                if (itemPath.Length == 0) throw new UnityException("Error loaded item path empty");
                Item item = AssetDatabase.LoadAssetAtPath<Item>(itemPath.Substring(itemPath.IndexOf("Assets")));
                item.GiveTo(TargetSO.inventory);
            }
            SerializedProperty Items = FindChildProperty(InventoryContents,"_Keys");
            SerializedProperty Qtys = FindChildProperty(InventoryContents,"_Values");      
            if(Items.arraySize == 0) {GUILayout.Label("Inventory Empty");}
            else for (int _ = 0; _ < Items.arraySize; _ ++) {
                SerializedObject item =  //?this is a serialized object because the code didn't work when it was a property (propably bc its an SO)
                    new SerializedObject(Items.GetArrayElementAtIndex(_).objectReferenceValue);
                SerializedProperty qty = Qtys.GetArrayElementAtIndex(_);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{item.FindProperty("_name").stringValue}");
                qty.intValue = EditorGUILayout.IntField(qty.intValue);
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        */
        
        // ! Initialize Button ! //
        if(debug){ 
            if(GUILayout.Button("Initialize")) {
                if(UnityEditor.EditorUtility.DisplayDialog( $"Initialize {Name.stringValue}?",
                    "This will reset all values to that of a default CharacterSO. Are you sure you want to do this?",
                    "Initialize","Cancel" )
                ) { 
                    InitializeCharacterSO(); }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
    //TODO probably needs input handeling 
    SerializedProperty FindChildProperty(SerializedProperty parent, string path) {;
        string targetPath = $"{parent.propertyPath}.{path}";
        DebugMessage($"Looking up property at {targetPath}");
        if(serializedObject.FindProperty(targetPath) == null) {throw new System.Exception($"child property at {targetPath} not found. Is it marked as Serializable?");}
        return serializedObject.FindProperty(targetPath);    
    }
    void DebugMessage(string msg) {
        if(debug) {UnityEngine.Debug.Log(msg);}
    }
    void InitializeCharacterSO() {
        TargetSO.Init();
    }
}