/// @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
Shader "WorldWizards/GridShader"
{
	Properties
	{
        _MainColor ("Main color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_ViewDistance ("View distance", Range(10, 5000)) = 100

	}
	SubShader
	{
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Blend One One
		Cull Off
	    ZWrite Off
		
		Offset -1, -1

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			    float4 worldPos : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainColor;
			float _ViewDistance;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _MainColor;
				
				float dist = distance(i.worldPos, _WorldSpaceCameraPos);
                col *= 1 - saturate(dist / _ViewDistance);
				return col;
			}
			ENDCG
		}
	}
}
