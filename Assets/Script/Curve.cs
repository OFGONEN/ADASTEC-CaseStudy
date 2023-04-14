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
	public GameObject curve_node_prefab;
	public bool curve_draw_mesh = true;
	public bool curve_draw_curve = true;

	[ SerializeField ] Vector3[] curve_control_point_array_left;
	[ SerializeField ] Vector3[] curve_control_point_array_right;
	[ SerializeField ] List< Vector3 > curve_node_point_list_left;
	[ SerializeField ] List< Vector3 > curve_node_point_list_right;
	[ SerializeField ] Mesh curve_mesh;
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
		EditorUtility.SetDirty( gameObject );

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

	public void DistanceMode()
	{
		EditorUtility.SetDirty( gameObject );

		var listOfChildren = new List< Transform >();

		for( var i = 0; i < transform.childCount; i++ )
			listOfChildren.Add( transform.GetChild( i ) );
		
		for( var i = 0; i < listOfChildren.Count; i++ )
			DestroyImmediate( listOfChildren[ i ].gameObject );
		
		for( var i = 0; i < curve_node_point_list_left.Count; i++ )
		{
			var nodeObject = GameObject.Instantiate( curve_node_prefab );
			nodeObject.transform.SetParent( transform );

			nodeObject.name               = "node_left_" + i;
			nodeObject.transform.position = curve_node_point_list_left[ i ];
			nodeObject.transform.rotation = Quaternion.identity;
		}

		for( var i = 0; i < curve_node_point_list_right.Count; i++ )
		{
			var nodeObject = GameObject.Instantiate( curve_node_prefab );
			nodeObject.transform.SetParent( transform );

			nodeObject.name               = "node_right_" + i;
			nodeObject.transform.position = curve_node_point_list_right[ i ];
			nodeObject.transform.rotation = Quaternion.identity;
		}

		CreateMesh();
	}
#endregion

#region Implementation
	void CreateMesh()
	{
		curve_mesh = new Mesh();

		var vertices = GetCurveMeshVertices();
		curve_mesh.vertices  = vertices;
		curve_mesh.triangles = GetCurveTriangles( vertices );

		curve_mesh.RecalculateNormals();
		curve_mesh.RecalculateBounds();
	}

	Vector3[] GetCurveMeshVertices()
	{
		var verticesArray = new List< Vector3 >( curve_node_point_list_left.Count * 2 );

		// One line can have more node point than the other so clamp the for loop iteration count with shorter line's node count
		int clamp = Mathf.Min( curve_node_point_list_left.Count, curve_node_point_list_right.Count );

		for( var i = 0; i < clamp; i++ )
		{
			verticesArray.Add( curve_node_point_list_left[ i ] );
			verticesArray.Add( curve_node_point_list_right[ i ] );
		}

		// If one line is longer than other, their node count can differ from eachother. Find the longer line and add its last node point to vertices list
		if( clamp + 1 < curve_node_point_list_left.Count )
			verticesArray.Add( curve_node_point_list_left[ clamp + 1 ] );
		
		if( clamp + 1 < curve_node_point_list_right.Count )
			verticesArray.Add( curve_node_point_list_right[ clamp + 1 ] );

		return verticesArray.ToArray();
	}

	int[] GetCurveTriangles( Vector3[] verticesArray )
	{
		var triesArray = new List< int >();

		for( var i = 0; i < verticesArray.Length - 3; i += 2 )
		{
			triesArray.Add( i );
			triesArray.Add( i + 2 );
			triesArray.Add( i + 1 );

			triesArray.Add( i + 1 );
			triesArray.Add( i + 2 );
			triesArray.Add( i + 3 );
		}

		return triesArray.ToArray();
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		// IF Curve is not initiated
		if( curve_control_point_array_left == null || 
			curve_control_point_array_left.Length == 0 || 
			curve_control_point_array_right.Length == 0 || 
			curve_control_point_array_right == null || 
			curve_node_point_list_left == null || 
			curve_node_point_list_right == null ) 
			return;

		DrawCurve( curve_control_point_array_left, curve_node_point_list_left );
		DrawCurve( curve_control_point_array_right, curve_node_point_list_right );
		DrawMesh();
	}

	void DrawMesh()
	{
		if( curve_mesh == null )
			return;

		if( curve_draw_mesh )
			Gizmos.DrawMesh( curve_mesh, transform.position, Quaternion.identity );

		//Info: Un-comment this to debug mesh creation
		// var vertices  = curve_mesh.vertices;
		// var triangles = curve_mesh.triangles;

		// var verticalOffset = Vector3.up * 0.1f;

		// for( var i = 0; i < vertices.Length; i++ )
		// {
		// 	Handles.Label( vertices[ i ] + verticalOffset, "Vert " + i );
		// }

		// for( var i = 0; i < triangles.Length - 3; i++ )
		// {
		// 	Handles.DrawLine( vertices[ triangles[ i ] ] + verticalOffset, vertices[ triangles[ i + 1 ] ] + verticalOffset );
		// 	Handles.DrawLine( vertices[ triangles[ i + 1 ] ] + verticalOffset, vertices[ triangles[ i + 2 ] ] + verticalOffset );
		// 	Handles.DrawLine( vertices[ triangles[ i + 2 ] ] + verticalOffset, vertices[ triangles[ i ] ] + verticalOffset );
		// }
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

				if( curve_draw_curve )
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