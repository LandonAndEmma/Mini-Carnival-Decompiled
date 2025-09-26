Shader "Triniti/Extra/LightHalo_Color" {
Properties {
 _HaloColor ("Halo Color", Color) = (1,1,1,1)
 _Halo ("Halo (RGBA)", 2D) = "black" {}
 _Color ("Halo Color", Color) = (1,1,1,1)
 _MainTex ("MainTex (RGB)", 2D) = "" {}
 _LightMap ("Lightmap (RGB)", 2D) = "white" {}
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