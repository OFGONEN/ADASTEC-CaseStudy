/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Curve : MonoBehaviour
{
#region Fields
    [ Min( 0.01f ) ]
    public float curve_step = 0.01f;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		var positionOne   = transform.GetChild( 0 ).position;
		var positionTwo   = transform.GetChild( 1 ).position;
		var positionThree = transform.GetChild( 2 ).position;

		Vector3 positionTemp = positionOne;

		for( float i = curve_step; i <= 1; i += curve_step )
        {
			var curvedPoint = PointCalculations.ReturnCurvedPoint( positionOne, positionTwo, positionThree, i );

			Handles.DrawLine( positionTemp, curvedPoint );
			positionTemp = curvedPoint;
		}
	}
#endif
#endregion
}