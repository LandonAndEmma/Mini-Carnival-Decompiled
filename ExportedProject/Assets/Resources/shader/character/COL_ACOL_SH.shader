Shader "Triniti/Character/COL_ACOL_SH" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex(RGB)", 2D) = "white" {}
 _AColor ("Additive Color", Color) = (0,0,0,0)
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}