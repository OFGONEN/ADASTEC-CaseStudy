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
		    Vector3 newPosition = Handles.PositionHandle( curve.transform.TransformPoint( curve_left[ i ] ), Quaternion.identity );

		    if( EditorGUI.EndChangeCheck() )
		    {
				EditorUtility.SetDirty( curve );
				curve_left[ i ] = curve.transform.InverseTransformPoint( newPosition );
			}
        }

		for( var i = 0; i < curve_right.Length; i++ )
		{
			EditorGUI.BeginChangeCheck();
			Vector3 newPosition = Handles.PositionHandle( curve.transform.TransformPoint( curve_right[ i ] ), Quaternion.identity );

			if( EditorGUI.EndChangeCheck() )
			{
				EditorUtility.SetDirty( curve );
				curve_right[ i ] = curve.transform.InverseTransformPoint( newPosition );
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