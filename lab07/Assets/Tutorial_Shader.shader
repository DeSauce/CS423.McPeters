Shader"Unlit/Tutorial_Shader" { 
  Properties {
    _Color ("Set the color!", Color) = (1,1,1,1)
    _MainTexture ("Main Texture", 2D) = "white" {}
    _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
    _DissolveCutoff ("Dissolve Cutoff", Range(0,1)) = 1
  }

  SubShader {
    Pass {
       CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
#include "UnityCG.cginc"
struct appdata
{
    float4 pos : POSITION;
    float2 uv : TEXCOORD0;
};
struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
};

float4 _Color; 
sampler2D _MainTexture;
sampler2D _DissolveTexture;
float _DissolveCutoff;

v2f vert(appdata inbound)
{
    v2f outbound;
    outbound.pos = UnityObjectToClipPos(inbound.pos.xyz);
    outbound.uv = inbound.uv;
    return outbound;
}
fixed4 frag(v2f inbound) : SV_Target
{
    float4 textureColor = tex2D(_MainTexture, inbound.uv);
    float4 dissolveColor = tex2D(_DissolveTexture, inbound.uv);
    clip(dissolveColor.rgb - _DissolveCutoff);
    return textureColor;
}
       ENDCG
    }
    
  }
}