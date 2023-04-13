/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ CustomPropertyDrawer( typeof( ButtonAttribute ) )]
public class ButtonDrawer : PropertyDrawer
{
	public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
	{
		string methodName = ( attribute as ButtonAttribute ).MethodName;
		Object target = property.serializedObject.targetObject;
		System.Type type = target.GetType();
		System.Reflection.MethodInfo method = type.GetMethod( methodName );
		if( method == null )
		{
			GUI.Label( position, "Method could not be found. Is it public?" );
			return;
		}
		if( method.GetParameters().Length > 0 )
		{
			GUI.Label( position, "Method cannot have parameters." );
			return;
		}
		if( GUI.Button( position, method.Name ) )
		{
			method.Invoke( target, null );
		}
	}
}

public class ButtonAttribute : PropertyAttribute
{
	public string MethodName { get; }
	public ButtonAttribute( string methodName )
	{
		MethodName = methodName;
	}
}