Shader "Devil/Shader_Lightmap" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
 _LightMap ("Lightmap (RGBA)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  Color [_Color]
  SetTexture [_MainTex] { combine texture * previous }
  SetTexture [_LightMap] { combine texture * previous, texture alpha * previous alpha }
 }
}
}