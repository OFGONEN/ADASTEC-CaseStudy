using UnityEditor;
using UnityEngine;

[ CustomEditor( typeof( Curve ) ), CanEditMultipleObjects ]
public class CurveEditor : Editor
{
	protected virtual void OnSceneGUI()
	{
		Curve curve = target as Curve;

		var curve_left  = curve.CurveControlPointArrayLeft;
		var curve_right = curve.CurveControlPointArrayRight;

		for( var i = 0; i < curve_left.Length; i++ )
        {
		    EditorGUI.BeginChangeCheck();
		    Vector3 newPosition = Handles.PositionHandle( curve_left[ i ], Quaternion.identity );

		    if( EditorGUI.EndChangeCheck() )
		    {
				curve_left[ i ] = newPosition;
			}
        }

		for( var i = 0; i < curve_right.Length; i++ )
		{
			EditorGUI.BeginChangeCheck();
			Vector3 newPosition = Handles.PositionHandle( curve_right[ i ], Quaternion.identity );

			if( EditorGUI.EndChangeCheck() )
			{
				curve_right[ i ] = newPosition;
			}
		}
	}

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();

        if( GUILayout.Button( "Create Curve" ) )
			( target as Curve ).CreateCurve();
	}
}