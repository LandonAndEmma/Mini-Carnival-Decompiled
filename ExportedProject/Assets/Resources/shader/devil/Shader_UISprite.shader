Shader "Devil/Shader_UISprite" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
 _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Transparent+7" }
 Pass {
  Tags { "QUEUE"="Transparent+7" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
  }
  Color [_Color]
  ZTest Always
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * previous }
 }
}
}