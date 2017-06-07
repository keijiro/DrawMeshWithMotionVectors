Shader "Hidden/MotionVectors"
{
    CGINCLUDE

    #include "UnityCG.cginc"

    float4x4 _NonJitteredVP;
    float4x4 _PreviousVP;
    float4x4 _PreviousM;

    struct MotionVectorData
    {
        float4 transferPos : TEXCOORD0;
        float4 transferPosOld : TEXCOORD1;
        float4 pos : SV_POSITION;
    };

    struct MotionVertexInput
    {
        float4 vertex : POSITION;
        float3 oldPos : NORMAL;
    };

    MotionVectorData VertMotionVectors(MotionVertexInput v)
    {
        MotionVectorData o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.transferPos = mul(_NonJitteredVP, mul(unity_ObjectToWorld, v.vertex));
        o.transferPosOld = mul(_PreviousVP, mul(_PreviousM, v.vertex));
        return o;
    }

    half4 FragMotionVectors(MotionVectorData i) : SV_Target
    {
        float3 hPos = (i.transferPos.xyz / i.transferPos.w);
        float3 hPosOld = (i.transferPosOld.xyz / i.transferPosOld.w);

        // V is the viewport position at this pixel in the range 0 to 1.
        float2 vPos = (hPos.xy + 1.0f) / 2.0f;
        float2 vPosOld = (hPosOld.xy + 1.0f) / 2.0f;

#if UNITY_UV_STARTS_AT_TOP
        vPos.y = 1.0 - vPos.y;
        vPosOld.y = 1.0 - vPosOld.y;
#endif
        return half4(vPos - vPosOld, 0, 1);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest LEqual ZWrite Off
            CGPROGRAM
            #pragma vertex VertMotionVectors
            #pragma fragment FragMotionVectors
            ENDCG
        }
    }
}
