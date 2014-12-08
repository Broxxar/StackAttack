Shader "Custom/Sheen"
{
	Properties
	{
		_MainTex ("Sheen", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
		}
		
		Lighting Off
		Cull Off

		Pass
		{  
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float4 worldcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldcoord = mul(_Object2World, v.vertex);

				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.worldcoord /20);
				return col;
			}
				
			ENDCG
		}
	}
}
