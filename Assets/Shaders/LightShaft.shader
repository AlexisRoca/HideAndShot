Shader "Unlit/LightShaft"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "Queue" = "Transparent"
			   "RenderType" = "Transparent" }
		
		//Blend SrcAlpha OneMinusSrcAlpha

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
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float alpha = 0.5; // i.pos.z;
				
				// white color
				fixed4 white = (1.0,1.0,1.0,alpha);

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				if (length(col.rgb) < 0.9) discard;

				return white;
			}

			ENDCG
		}
	}
	Fallback "Transparent"
}
