Shader "Triniti/Mask shader" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Mask ("Culling Mask", 2D) = "white" {}
 _Cutoff ("Alpha cutoff", Range(0,1)) = 0.1
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest GEqual [_Cutoff]
  SetTexture [_Mask] { combine texture }
  SetTexture [_MainTex] { combine texture, previous alpha }
 }
}
}