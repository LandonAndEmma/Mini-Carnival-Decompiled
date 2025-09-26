Shader "Triniti/Extra/LightHalo" {
Properties {
 _HaloColor ("Halo Color", Color) = (1,1,1,1)
 _Halo ("Halo (RGBA)", 2D) = "black" {}
 _MainTex ("MainTex (RGB)", 2D) = "" {}
 _LightMap ("Lightmap (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
   Bind "texcoord", TexCoord2
   Bind "texcoord", TexCoord3
   Bind "texcoord1", TexCoord4
  }
  SetTexture [_Halo] { combine texture * texture double }
  SetTexture [_Halo] { ConstantColor [_HaloColor] combine constant * previous }
  SetTexture [_MainTex] { combine texture * previous double }
  SetTexture [_MainTex] { combine previous + texture }
  SetTexture [_LightMap] { combine texture * previous }
 }
}
}