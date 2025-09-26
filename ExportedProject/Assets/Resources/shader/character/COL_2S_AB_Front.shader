Shader "Triniti/Character/COL_2S_AB_Front" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex(RGB)", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant }
 }
}
}