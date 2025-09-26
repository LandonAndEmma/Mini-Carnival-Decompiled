Shader "Devil/Shader_Props-Alpha-IconCover" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
 _Color ("Main Color", Color) = (0,0,0,0)
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  Color [_Color]
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine previous, texture alpha }
  SetTexture [_MainTex] { combine texture lerp(previous) previous }
 }
}
}