using UnityEditor;
using UnityEngine;

[ CustomEditor( typeof( Curve ) ), CanEditMultipleObjects ]
public class CurveEditor : Editor
{
	protected virtual void OnSceneGUI()
	{
		Curve curve = target as Curve;

		if( curve.CurveNodePointListLeft == null || curve.CurveNodePointListRight == null )
			return;

		// Cache curve control point arrays
		var curve_left  = curve.CurveControlPointArrayLeft;
		var curve_right = curve.CurveControlPointArrayRight;

		// Draw Curve Left Control Point Handles
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

		// Draw Curve Right Control Point Handles
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

		//Draw Note Points
		if( curve.CurveNodePointListLeft != null )
		{
			for( var i = 0; i < curve.CurveNodePointListLeft.Count; i++ )
				Handles.DrawWireCube( curve.CurveNodePointListLeft[ i ], 0.1f * Vector3.one );

			for( var i = 0; i < curve.CurveNodePointListRight.Count; i++ )
				Handles.DrawWireCube( curve.CurveNodePointListRight[ i ], 0.1f * Vector3.one );
		}
	}

    public override void OnInspectorGUI()
    {
		DrawDefaultInspector();

        if( GUILayout.Button( "Create Curve" ) )
			( target as Curve ).CreateCurve();
		
		if( GUILayout.Button( "Distance Mode"))
			( target as Curve ).DistanceMode();
	}
}