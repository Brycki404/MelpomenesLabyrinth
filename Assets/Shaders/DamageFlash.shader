Shader "Custom/DamageFlash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlashColor ("Flash Color", Color) = (1,0,0,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP include
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _FlashColor;
            float _FlashAmount;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return lerp(col, _FlashColor, _FlashAmount);
            }

            ENDHLSL
        }
    }
}