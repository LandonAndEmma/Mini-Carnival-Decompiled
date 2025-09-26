Shader "Devil/Shader_Character" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
  }
  Color [_Color]
  Cull Off
  SetTexture [_MainTex] { combine texture * previous double }
 }
}
Fallback Off
}