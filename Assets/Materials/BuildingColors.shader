Shader "Unlit/BuildingColors"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Alpha("Alpha", Float) = 1
		_Color("Color", Color) = (1, 1, 1)
	}
		SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		Tags
	{
		"RenderType" = "Opaque"
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}
		LOD 100

		Pass
	{
		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
			// make fog work
	#pragma multi_compile_fog

	#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}
		float _Alpha;
		float3 _Color;

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
			float average = (col.r + col.g + col.b) / 3.0;
			col = float4(average * _Color.r, average * _Color.g, average * _Color.b, _Alpha);
			return col;
		}
			ENDCG
		}
	}
}