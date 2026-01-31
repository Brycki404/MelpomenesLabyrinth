Shader "Custom/Sprite_MultiFX"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _FlashColor ("Flash Color", Color) = (1,0,0,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0

        _DissolveTex ("Dissolve Noise", 2D) = "gray" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _DissolveEdgeColor ("Dissolve Edge Color", Color) = (1,0.8,0.2,1)
        _DissolveEdgeWidth ("Dissolve Edge Width", Range(0,0.3)) = 0.1

        _PulseSpeed ("Pulse Speed", Range(0,20)) = 8
        _PulseStrength ("Pulse Strength", Range(0,1)) = 0.4

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0,4)) = 1
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
            Name "MultiFX"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);

            float4 _Color;
            float4 _FlashColor;
            float _FlashAmount;

            float _DissolveAmount;
            float4 _DissolveEdgeColor;
            float _DissolveEdgeWidth;

            float _PulseSpeed;
            float _PulseStrength;

            float4 _OutlineColor;
            float _OutlineThickness;

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

            float4 SampleSprite(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Base sprite
                float4 baseTex = SampleSprite(uv);
                float4 col = baseTex * IN.color;

                // Outline (simple 4‑tap)
                float2 texel = float2(ddx(uv.x), ddy(uv.y)) * _OutlineThickness;
                float a0 = SampleSprite(uv + float2(texel.x, 0)).a;
                float a1 = SampleSprite(uv + float2(-texel.x, 0)).a;
                float a2 = SampleSprite(uv + float2(0, texel.y)).a;
                float a3 = SampleSprite(uv + float2(0, -texel.y)).a;
                float outlineMask = step(0.01, a0 + a1 + a2 + a3) * (1 - step(0.01, baseTex.a));
                float4 outlineCol = _OutlineColor * outlineMask;

                // Dissolve
                float noise = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, uv).r;
                float edgeStart = _DissolveAmount - _DissolveEdgeWidth;
                float edgeEnd   = _DissolveAmount;
                float dissolveMask = step(noise, _DissolveAmount);
                float edgeMask = smoothstep(edgeStart, edgeEnd, noise) * (1 - dissolveMask);

                // Clip fully dissolved pixels
                clip(dissolveMask - 0.01);

                // Apply edge color
                col.rgb = lerp(col.rgb, _DissolveEdgeColor.rgb, edgeMask);
                col.a *= dissolveMask;

                // Pulse (time‑based flash boost)
                float pulse = (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5) * _PulseStrength;
                float flashTotal = saturate(_FlashAmount + pulse);
                col = lerp(col, _FlashColor, flashTotal);

                // Combine outline behind sprite
                float outAlpha = saturate(col.a + outlineCol.a);
                float3 outRGB = lerp(outlineCol.rgb, col.rgb, col.a / max(outAlpha, 1e-4));

                return float4(outRGB, outAlpha);
            }

            ENDHLSL
        }
    }
}