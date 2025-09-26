Shader "Devil/Shader_Props-Alpha_rpg1" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord0
  }
  Color [_Color]
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * previous double, texture alpha * previous alpha }
 }
}
}