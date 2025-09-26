Shader "Devil/Shader_SolidColor" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
  }
  Color [_Color]
  Fog { Mode Off }
 }
}
}