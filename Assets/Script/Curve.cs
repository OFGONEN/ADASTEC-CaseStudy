/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Curve : MonoBehaviour
{
#region Fields

  [ Header( "SETUP: Control Mode" ) ]
    [ Min( 2 ) ] public int curve_control_point_count = 2;
    [ Min( 0 ) ] public int curve_lane_offset_horizontal = 1;
    [ Min( 0.005f ) ] public float curve_step = 0.01f;

  [ Header( "SETUP: Distance Mode" ) ]
    [ Min( 0.005f ) ] public float curve_node_distance = 1f;

	[ SerializeField, HideInInspector ] Vector3[] curve_control_point_array_left;
	[ SerializeField, HideInInspector ] Vector3[] curve_control_point_array_right;
	[ SerializeField, HideInInspector ] List< Vector3 > curve_node_point_list_left;
	[ SerializeField, HideInInspector ] List< Vector3 > curve_node_point_list_right;
#endregion

#region Properties
	public Vector3[] CurveControlPointArrayLeft    => curve_control_point_array_left;
	public Vector3[] CurveControlPointArrayRight   => curve_control_point_array_right;
	public List< Vector3 > CurveNodePointListLeft  => curve_node_point_list_left;
	public List< Vector3 > CurveNodePointListRight => curve_node_point_list_right;
#endregion

#region Unity API
#endregion

#region API
	public void CreateCurve()
	{
		var pointCount = 1 + ( curve_control_point_count - 1 ) * 3;
		curve_control_point_array_left  = new Vector3[ pointCount ];
		curve_control_point_array_right = new Vector3[ pointCount ];

		curve_node_point_list_left  = new List< Vector3 >();
		curve_node_point_list_right = new List< Vector3 >();

		for( var i = 0; i < pointCount; i++ )
		{
			curve_control_point_array_left [ i ] = transform.forward * i + transform.right * curve_lane_offset_horizontal / 2f * -1f;
			curve_control_point_array_right[ i ] = transform.forward * i + transform.right * curve_lane_offset_horizontal / 2f;
		}
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		if( curve_control_point_array_left == null ) // IF Curve is not initiated
			return;

		DrawCurve( curve_control_point_array_left, curve_node_point_list_left );
		DrawCurve( curve_control_point_array_right, curve_node_point_list_right );
	}

	void DrawCurve( Vector3[] curveControlPoints, List< Vector3 > curveNodePoints )
	{
		float distanceDelta = 0f;

		curveNodePoints.Clear();
		curveNodePoints.Add( transform.TransformPoint( curveControlPoints[ 0 ] ) );

		Vector3 curvedPoint = Vector3.zero;

		for( var i = 0; i < curveControlPoints.Length - 3; i += 3 )
		{
			Vector3 positionTemp = transform.TransformPoint( curveControlPoints[ i ] );

			for( float delta = curve_step; delta <= 1; delta += curve_step )
			{
				curvedPoint = PointCalculations.ReturnCubicBeizerCurvedPoint(
					transform.TransformPoint( curveControlPoints[ i ] ),
					transform.TransformPoint( curveControlPoints[ i + 1 ] ),
					transform.TransformPoint( curveControlPoints[ i + 2 ] ),
					transform.TransformPoint( curveControlPoints[ i + 3 ] ),
					delta
				);

				var currentDelta = Vector3.Distance( positionTemp, curvedPoint );

				if( distanceDelta + currentDelta >= curve_node_distance )
				{
					var distanceDirection = ( curvedPoint - positionTemp ).normalized;
					var distanceStep      = curve_node_distance - distanceDelta;
					var nodePoint         = positionTemp + distanceDirection * distanceStep;

					curveNodePoints.Add( nodePoint );
					distanceDelta = 0;
				}
				else
					distanceDelta += currentDelta;

				Handles.DrawLine( positionTemp, curvedPoint );
				positionTemp = curvedPoint;
			}
		}

		// if( Vector3.Distance( curvedPoint, curveNodePoints[ curveNodePoints.Count - 1 ] ) > curve_step )
		if( !Mathf.Approximately( 0, Vector3.Distance( curvedPoint, curveNodePoints[ curveNodePoints.Count - 1 ] ) ) )
			curveNodePoints.Add( curvedPoint );
	}
#endif
#endregion
}