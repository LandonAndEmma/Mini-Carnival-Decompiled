Shader "Triniti/Character/COL_2S" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex(RGB)", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" }
  Cull Off
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
 }
}
}