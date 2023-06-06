Shader "Unlit/Tutorial"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half3 palette(half t)
            {
                half3 a = half3(0.5, 0.5, 0.5);
                half3 b = half3(0.5, 0.5, 0.5);
                half3 c = half3(1.0, 1.0, 1.0);
                half3 d = half3(0.263,0.416,0.557);

                return a + b * cos(6.28318 * (c * t + d));
            }

            half4 frag(Varyings input) : SV_Target
            {
                half2 uv = (input.uv * 2.0 - half2(1, 1)) / half2(min(_ScreenParams.x,_ScreenParams.y), min(_ScreenParams.x,_ScreenParams.y));
                
                half2 uv0 = uv;
                half3 finalColor = half3(0.0, 0.0, 0.0);

                for (half i = 0.0; i < 4.0; i++)
                {
                    uv = frac(uv * half2(1.5, 1.5)) - half2(0.5, 0.5);

                    uv = uv * 0.1;
                    half d = length(uv) * exp(-length(uv0));

                    half3 col = palette(length(uv0) + i * 0.4 + _Time.y * 0.4);

                    d = sin(d * 8. + _Time.y) / 8.;
                    d = abs(d);

                    d = pow(0.01 / d, 1.2);

                    finalColor += col * d;
                }

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
