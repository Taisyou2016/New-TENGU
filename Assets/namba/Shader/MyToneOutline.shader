Shader "Custom/MyToneOutline" {

	Properties{
		_MainTex("Texture", 2D) = "white"{}
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_ShadeDetail("Shade Detail", Range(1,20)) = 4
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(0, 0.03)) = 0.01

	}

	SubShader{

		Tags{ "RenderType" = "Opaque" }

		Pass{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _OutlineColor;
			float _OutlineWidth;

			float4 vert(appdata_base v) : SV_POSITION
			{
				float depth = mul(UNITY_MATRIX_MVP, v.vertex).z;	// ここで距離に応じたdepthを計算
				v.vertex.xyz += v.normal * _OutlineWidth * depth;	// _OutlineWidthにdepthをかける
				return mul(UNITY_MATRIX_MVP, v.vertex);
			}

			fixed4 frag() : SV_Target
			{
				return _OutlineColor;
			}

			ENDCG
		}

		CGPROGRAM
		#pragma surface surf MyLighting

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		half4 _Color;
		half _ShadeDetail;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		half4 LightingMyLighting(SurfaceOutput s, half3 lightDir, half atten) {
			half4 c;
			half NdotL = dot(s.Normal, lightDir);
			c.rgb = s.Albedo * _LightColor0.rgb
				* floor((NdotL * 0.5 + 0.5) * atten * _ShadeDetail) / _ShadeDetail;
			c.a = s.Alpha;
			return c;
		}

	ENDCG
	}


	FallBack "Diffuse"
}
