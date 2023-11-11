
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(KeyCodeNameComponent.KeyCodeStringPair))]
public class KeyCodeStringPairDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        contentPosition.width *= 0.4f;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("keyCode"), GUIContent.none);
        contentPosition.x += contentPosition.width;
        contentPosition.width *= 1.5f;
        EditorGUIUtility.labelWidth = 12f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.EndProperty();
    }

}
