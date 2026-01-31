Shader "Custom/DamageFlash_Simple2D"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,0,0,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "UnlitFlash"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _Color;
            float4 _FlashColor;
            float _FlashAmount;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float4 texCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 baseCol = texCol * IN.color;
                float4 flashed = lerp(baseCol, _FlashColor, _FlashAmount);
                return flashed;
            }

            ENDHLSL
        }
    }
}