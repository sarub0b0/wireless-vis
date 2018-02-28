//------------------------------------------------------------------------
//
// (C) Copyright 2016 Urahimono Project Inc.
//
//------------------------------------------------------------------------
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class LayerTypeFieldAttribute : PropertyAttribute
{

}
 // class TagAttribute

#if UNITY_EDITOR

[CustomPropertyDrawer( typeof( LayerTypeFieldAttribute ) )]
public class LayerTypeFieldDrawer : PropertyDrawer
{
    public override void OnGUI( Rect i_position, SerializedProperty i_property, GUIContent i_label )
    {
        i_property.intValue = EditorGUI.LayerField( i_position, i_label, i_property.intValue );
    }

    public override float GetPropertyHeight( SerializedProperty i_property, GUIContent i_label )
    {
        return EditorGUI.GetPropertyHeight( i_property );
    }

} // class TagAttribute

#endif // UNITY_EDITOR
