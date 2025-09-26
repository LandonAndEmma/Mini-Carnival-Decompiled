Shader "Devil/Shader_SolidColor-Alpha" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  BindChannels {
   Bind "vertex", Vertex
  }
  Color [_Color]
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
 }
}
}