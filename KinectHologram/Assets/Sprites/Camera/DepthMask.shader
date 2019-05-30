// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "DepthMask"
{
	Properties{
		_Depth("Depth", Range(0,1)) = 1
	}

		SubShader
	{
		Tags { "Queue" = "Geometry-1" }

		Pass {
			Lighting Off
			ZWrite On
			ZTest Always
			ColorMask 0

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float _Depth;

			struct v2f
			{
				float4 position : POSITION;
			};

			struct fragOut
			{
				half4 color : COLOR;
				half depth : DEPTH;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fragOut frag(v2f i) {
				fragOut o;
				o.color = float4(1,0,0,0);
				o.depth = _Depth;
				return o;
			}
			ENDCG
		}
	}
}