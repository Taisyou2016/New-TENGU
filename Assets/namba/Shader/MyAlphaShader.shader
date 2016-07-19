Shader "Custom/MyAlphaShader" {
	Properties{
		_MainTex("Texture", 2D) = "white"{}
		_Alpha("Alpha", Range(0,1)) = 0.5
	}

		SubShader{
			Tags{ "Queue" = "Transparent" "RenderType" = "Opaque"}
			CGPROGRAM
#pragma surface surf Lambert alpha

			struct Input {
				float2 uv_MainTex;
};

			sampler2D _MainTex;
			float _Alpha;

			void surf(Input IN, inout SurfaceOutput o) {
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
				o.Alpha = _Alpha;
			}

			ENDCG
		}
	FallBack "Diffuse"
}
