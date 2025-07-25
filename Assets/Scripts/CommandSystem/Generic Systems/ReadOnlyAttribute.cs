// 7/23/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Makes any field fully read-only in the inspector, including arrays and lists.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.DisabledScope(true))
        {
            // Draw property, including children (true = recursive)
            EditorGUI.PropertyField(position, property, label, includeChildren: true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate height recursively
        return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
    }
}
#endif