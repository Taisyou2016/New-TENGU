Shader "Custom/MyToneShader" {

	Properties{
		_MainTex("Texture", 2D) = "white"{}
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_ShadeDetail("Shade Detail", Range(1,20)) = 2
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
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
