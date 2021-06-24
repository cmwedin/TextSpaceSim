using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterCustomEditorWindow : ExtendedEditorWindow {

    public static void Open(CharacterSO character) {
        CharacterCustomEditorWindow window = GetWindow<CharacterCustomEditorWindow>("Character Editor");
        //window.titleContent = new GUIContent("CharacterCustomWindow");
        window.seriealizedObject = new SerializedObject(character);
        //window.Show();
    }

    private void OnGUI() {
        currentProperty = seriealizedObject.FindProperty(propertyPath: "Attributes");
        DrawProperties(currentProperty, true);
    }
}
