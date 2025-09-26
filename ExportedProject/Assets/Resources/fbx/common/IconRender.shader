Shader "Z/IconRender" {
Properties {
 _MainTex ("MainTex", 2D) = "" {}
 _Color ("Main Color", Color) = (0,0,0,0)
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  Color [_Color]
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture, previous alpha }
  SetTexture [_MainTex] { combine texture lerp(previous) previous }
 }
}
}