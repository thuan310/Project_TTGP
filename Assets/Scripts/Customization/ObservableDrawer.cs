using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Observable<>), true)]
public class ObservableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Find the serialized field "_value"
        SerializedProperty valueProperty = property.FindPropertyRelative("_value");

        // Draw the field in the Inspector
        EditorGUI.PropertyField(position, valueProperty, label, true);
    }
}