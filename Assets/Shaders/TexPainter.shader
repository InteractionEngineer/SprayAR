Shader "Custom/BlendingPaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushTex ("Brush Texture", 2D) = "white" {}
        _BrushUV ("Brush UV", Vector) = (0, 0, 0, 0)
        _BrushSize ("Brush Size", Float) = 0.1
        _BrushColor ("Brush Color", Color) = (1, 0, 0, 1)
        _BrushStrength ("Brush Strength", Range(0.0, 1.0)) = 1.0
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
            float _BrushSize;
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
                float2 uv = i.uv;
                float dist = distance(uv, brushUV);

                half4 original = tex2D(_MainTex, uv);
                half4 brushColor = _BrushColor;
                brushColor.a *= _BrushStrength; // Adjust alpha based on brush strength

                if (dist < _BrushSize)
                {
                    float blendFactor = (1 - dist / _BrushSize) * brushColor.a;
                    // float blendFactor = smoothstep(_BrushSize * 1.5, 0, dist) * brushColor.a;
                    return lerp(original, brushColor, blendFactor);
                }
                return original;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
