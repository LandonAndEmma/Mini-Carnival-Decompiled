Shader "Triniti/Character/COL_DO_AB" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
 _MainTex ("MainTex(RGB)", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * constant double }
 }
}
}