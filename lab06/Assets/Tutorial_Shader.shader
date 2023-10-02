Shader"Unlit/Tutorial_Shader" { 
  Properties {
    _Color ("Set the color!", Color) = (1,1,1,1)
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
v2f vert(appdata inbound)
{
    v2f outbound;
    outbound.pos = UnityObjectToClipPos(inbound.pos.xyz);
    outbound.uv = inbound.uv;
    return outbound;
}
fixed4 frag(v2f inbound) : SV_Target
{
    return _Color;
}
       ENDCG
    }
    
  }
}