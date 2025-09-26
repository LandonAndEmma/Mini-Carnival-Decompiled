Shader "Triniti/Scene/COL_LM_BR_BM2" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex (RGB)", 2D) = "white" {}
 _BrightMap ("Brightmap (RGB)", 2D) = "white" {}
 _LightMap ("Lightmap (RGB)", 2D) = "white" {}
 _Channel1 ("ChannelMap1 (A)", 2D) = "black" {}
 _BlendTex1 ("BlendTex1 (RGB)", 2D) = "black" {}
 _Channel2 ("ChannelMap2 (A)", 2D) = "black" {}
 _BlendTex2 ("BlendTex2 (RGB)", 2D) = "black" {}
}
SubShader { 
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord", TexCoord1
   Bind "texcoord", TexCoord2
   Bind "texcoord", TexCoord3
   Bind "texcoord", TexCoord4
   Bind "texcoord1", TexCoord5
   Bind "texcoord1", TexCoord6
  }
  Color [_Color]
  SetTexture [_MainTex] { combine texture }
  SetTexture [_Channel2] { combine previous, texture alpha }
  SetTexture [_BlendTex2] { combine texture lerp(previous) previous }
  SetTexture [_Channel1] { combine previous, texture alpha }
  SetTexture [_BlendTex1] { combine texture lerp(previous) previous }
  SetTexture [_BrightMap] { combine texture * previous double }
  SetTexture [_LightMap] { combine texture * previous }
 }
}
}