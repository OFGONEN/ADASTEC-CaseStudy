/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EasyButtons;

public class Curve : MonoBehaviour
{
#region Fields

  [ Header( "Setup" ) ]
    [ Min( 2 ) ] public int curve_control_point_count = 2;
    [ Min( 0 ) ] public int curve_lane_offset_horizontal = 1;
    [ Min( 0.005f ) ] public float curve_step = 0.01f;

	[ SerializeField ] Vector3[] curve_control_point_array_left;
	[ SerializeField ] Vector3[] curve_control_point_array_right;
#endregion

#region Properties
	public Vector3[] CurveControlPointArrayLeft  => curve_control_point_array_left;
	public Vector3[] CurveControlPointArrayRight => curve_control_point_array_right;
#endregion

#region Unity API
#endregion

#region API
	public void CreateCurve()
	{
		var pointCount = 1 + ( curve_control_point_count - 1 ) * 3;
		curve_control_point_array_left  = new Vector3[ pointCount ];
		curve_control_point_array_right = new Vector3[ pointCount ];

		for( var i = 0; i < pointCount; i++ )
		{
			curve_control_point_array_left [ i ] = transform.position + transform.forward * i;
			curve_control_point_array_right[ i ] = transform.position + transform.forward * i + transform.right * curve_lane_offset_horizontal;
		}
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		if( curve_control_point_array_left == null ) // Curve is not initiated
			return;

		DrawCurve( curve_control_point_array_left );
		DrawCurve( curve_control_point_array_right );
	}

	void DrawCurve( Vector3[] curveControlPoints )
	{
		for( var i = 0; i < curveControlPoints.Length - 3; i += 3 )
		{
			Vector3 positionTemp = curveControlPoints[ i ];

			for( float delta = curve_step; delta <= 1; delta += curve_step )
			{
				var curvedPoint = PointCalculations.ReturnCubicBeizerCurvedPoint(
					curveControlPoints[ i ],
					curveControlPoints[ i + 1 ],
					curveControlPoints[ i + 2 ],
					curveControlPoints[ i + 3 ],
					delta
				);

				Handles.DrawLine( positionTemp, curvedPoint );
				positionTemp = curvedPoint;
			}
		}
	}
#endif
#endregion
}