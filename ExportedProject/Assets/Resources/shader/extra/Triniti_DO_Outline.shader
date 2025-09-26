Shader "Triniti/Model/Triniti_DO_Outline" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _OutlineColor ("Outline Color", Color) = (0,0,0,1)
 _Outline ("Outline width", Range(0,5)) = 2
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _AtmoColor ("Atmosphere Color", Color) = (0.5,0.5,1,1)
 _TH ("Range TH", Range(0,1)) = 0
 _ZP ("Range Tp", Range(0,2)) = 1
 _LightDirect ("Light Direct", Vector) = (0,1,0,1)
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