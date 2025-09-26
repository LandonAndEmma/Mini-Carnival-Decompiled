Shader "Triniti/Extra/ScreenEffect" {
Properties {
 R ("R", Range(0,1)) = 1
 G ("G", Range(0,1)) = 1
 B ("B", Range(0,1)) = 1
 _Alpha ("Alpha", Range(0,1)) = 0.5
 _MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Overlay" }
 Pass {
  Tags { "QUEUE"="Overlay" }
  ZTest Always
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { ConstantColor ([R],[G],[B],[_Alpha]) combine constant * texture double }
 }
}
}