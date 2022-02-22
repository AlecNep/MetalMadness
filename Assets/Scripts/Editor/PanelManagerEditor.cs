using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


/// <summary>
/// Source: https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
/// </summary>
[CustomEditor(typeof(PanelManager))]
public class PanelManagerEditor : Editor
{
    SerializedProperty interactables;

    ReorderableList list;

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Update the array property's representation in the inspector

        list.DoLayoutList(); // Have the ReorderableList do its work

        // We need to call this so that changes on the Inspector are saved by Unity.
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        interactables = serializedObject.FindProperty("interactables");

        list = new ReorderableList(serializedObject, interactables, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list

        //Create a property field and label field for each property. 

        //The 'mobs' property. Since the enum is self-evident, I am not making a label field for it. 
        //The property field for mobs (width 100, height of a single line)
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("mobs"),
            GUIContent.none
        );


        //The 'level' property
        //The label field for level (width 100, height of a single line)
        EditorGUI.LabelField(new Rect(rect.x + 120, rect.y, 100, EditorGUIUtility.singleLineHeight), "Level");

        // The property field for level. Since we do not need so much space in an int, width is set to 20, height of a single line.
        EditorGUI.PropertyField(
            new Rect(rect.x + 160, rect.y, 20, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("level"),
            GUIContent.none
        );

        // The 'quantity' property
        // The label field for quantity (width 100, height of a single line)
        EditorGUI.LabelField(new Rect(rect.x + 200, rect.y, 100, EditorGUIUtility.singleLineHeight), "Quantity");

        // The property field for quantity (width 20, height of a single line)
        EditorGUI.PropertyField(
            new Rect(rect.x + 250, rect.y, 20, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("quantity"),
            GUIContent.none
        );

    }

    void DrawHeader(Rect rect)
    {
        string name = "Interactables";
        EditorGUI.LabelField(rect, name);
    }
}
