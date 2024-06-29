Shader "Custom/BlendingPaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushTex ("Brush Texture", 2D) = "white" {}
        _BrushUV ("Brush UV", Vector) = (0, 0, 0, 0)
        _BrushSizeX ("Brush Size X", Float) = 0.1
        _BrushSizeY ("Brush Size Y", Float) = 0.1
        _BrushColor ("Brush Color", Color) = (1, 0, 0, 1)
        _BrushStrength ("Brush Strength", Range(0.0, 1.0)) = 1.0
        _IsContinuousBrushStroke ("Is Continuous Brush Stroke", Float) = 0
        _PrevBrushUV ("Previous Brush UV", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BrushTex;
            float4 _BrushUV;
            float _BrushSizeX;
            float _BrushSizeY;
            float _IsContinuousBrushStroke;
            float4 _PrevBrushUV;
            fixed4 _BrushColor;
            float _BrushStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 brushUV = _BrushUV.xy;
                float2 prevBrushUV = _PrevBrushUV.xy;
                float2 uv = i.uv;


                half4 original = tex2D(_MainTex, uv);
                half4 brushColor = _BrushColor;
                brushColor.a *= _BrushStrength; // Adjust alpha based on brush strength

                
                // If continuous brush stroke is true, interpolate between previous and current brush UV
                if (_IsContinuousBrushStroke > 0)
                {
                    // Calculate distance between current and previous brush UV
                    float distXPrev = (uv.x - prevBrushUV.x) / _BrushSizeX;
                    float distYPrev = (uv.y - prevBrushUV.y) / _BrushSizeY;
                    float distPrev = sqrt(distXPrev * distXPrev + distYPrev * distYPrev);
                    float blendFactor = smoothstep(0, 1, distPrev);
                    brushUV = lerp(prevBrushUV, brushUV, blendFactor);
                }

                // Calculate distance between current UV and interpolated brush UV

                float distX = (uv.x - brushUV.x) / _BrushSizeX;
                float distY = (uv.y - brushUV.y) / _BrushSizeY;
                float dist = sqrt(distX * distX + distY * distY);

                if (dist < 1)
                {
                    float blendFactor = (1 - dist) * brushColor.a;
                    // float blendFactor = smoothstep(_BrushSize * 1.5, 0, dist) * brushColor.a;
                    half4 newColor = lerp(original, brushColor, blendFactor);
                    float alphaDiff = original.a - newColor.a;
                    if (alphaDiff < 0)
                    {
                        newColor.a += -alphaDiff;
                    } else 
                    {
                        newColor.a += alphaDiff;
                    }
                    newColor.rgb = lerp(original.rgb, brushColor.rgb, blendFactor);
                    newColor.a = clamp(newColor.a, 0, 1);
                    
                    return newColor;
                }
                return original;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
