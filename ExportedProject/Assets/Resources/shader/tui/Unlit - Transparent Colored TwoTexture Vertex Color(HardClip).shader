Shader "Unlit/Transparent Colored TwoTexture Vertex Color(HardClip)" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
 _Color ("Main Color", Color) = (1,1,1,1)
 _Rect ("Screen Rect", Vector) = (-1,-1,0,0)
 _BackTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
 _ColorB ("Back Color", Color) = (1,1,1,1)
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