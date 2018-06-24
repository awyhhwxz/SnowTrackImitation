Shader "Unlit/SnowSplatMapDrawer"
{
	Properties
	{
		_SplatMap ("Texture", 2D) = "white" {}
		_DrawColor("Draw Color", Color) = (0, 0, 0, 1)
		_Coordination("Coordination", Vector) = (0, 0, 0, 0)
		_ShaderSize("Shader Size", Range(0, 500)) = 4
		_ShaderStrength("Shader Strength", Range(0, 1)) = 1
		_ShaderRadius("Shader Radius", Range(0, 200)) = 80
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _SplatMap;
			float4 _SplatMap_ST;

			fixed4 _DrawColor;
			fixed4 _Coordination;
			fixed _ShaderSize;
			fixed _ShaderStrength;
			fixed _ShaderRadius;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _SplatMap);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_SplatMap, i.uv);
				float draw = pow(saturate(1 - distance(i.uv, _Coordination.xy) * _ShaderRadius), _ShaderSize);
				fixed4 drawCol = _DrawColor * draw * _ShaderStrength;
				return saturate(col + drawCol);
			}
			ENDCG
		}
	}
}
