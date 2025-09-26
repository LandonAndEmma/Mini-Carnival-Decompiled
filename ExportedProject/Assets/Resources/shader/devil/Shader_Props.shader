Shader "Devil/Shader_Props" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
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
  SetTexture [_MainTex] { combine texture * previous double }
 }
}
}