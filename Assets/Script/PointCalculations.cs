/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCalculations
{

    //Info: Lerp value must be between 0 and zero
    public static Vector3 ReturnLerpedPoint( Vector3 pointOne, Vector3 pointTwo, float lerpValue )
    {
#if UNITY_EDITOR
        if( lerpValue < 0 || lerpValue > 1 )
        {
            Debug.LogError( "Lerp Value must be between 0 and 1" );
			lerpValue = Mathf.Clamp( lerpValue, 0, 1 );
		}
#endif

		return pointOne + ( pointTwo - pointOne ) * lerpValue;
	}

	public static Vector3 ReturnQuadraticBeizerCurvedPoint( Vector3 pointOne, Vector3 pointTwo, Vector3 pointThree, float lerpValue )
	{
#if UNITY_EDITOR
		if( lerpValue < 0 || lerpValue > 1 )
		{
			Debug.LogError( "Lerp Value must be between 0 and 1" );
			lerpValue = Mathf.Clamp( lerpValue, 0, 1 );
		}
#endif

		var firstPoint  = ReturnLerpedPoint( pointOne, pointTwo, lerpValue );
		var secondPoint = ReturnLerpedPoint( pointTwo, pointThree, lerpValue );

		return ReturnLerpedPoint( firstPoint, secondPoint, lerpValue );
	}

	public static Vector3 ReturnCubicBeizerCurvedPoint( Vector3 pointOne, Vector3 pointTwo, Vector3 pointThree, Vector3 pointFour, float lerpValue )
	{
#if UNITY_EDITOR
		if( lerpValue < 0 || lerpValue > 1 )
		{
			Debug.LogError( "Lerp Value must be between 0 and 1" );
			lerpValue = Mathf.Clamp( lerpValue, 0, 1 );
		}
#endif

		var firstPoint  = ReturnLerpedPoint( pointOne, pointTwo, lerpValue );
		var secondPoint = ReturnLerpedPoint( pointTwo, pointThree, lerpValue );
		var thirdPoint  = ReturnLerpedPoint( pointThree, pointFour, lerpValue );

		var firstMiddlePoint  = ReturnLerpedPoint( firstPoint, secondPoint, lerpValue );
		var secondMiddlePoint = ReturnLerpedPoint( secondPoint, thirdPoint, lerpValue );

		return ReturnLerpedPoint( firstMiddlePoint, secondMiddlePoint, lerpValue );
	}
}