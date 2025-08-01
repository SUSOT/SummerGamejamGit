Shader "Custom/RectFillSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 1.0
        _FillColor ("Fill Color", Color) = (0, 1, 0, 1)
        _BackgroundColor ("Background Color", Color) = (1, 0, 0, 1)
        [Enum(LeftToRight, 0, RightToLeft, 1, BottomToTop, 2, TopToBottom, 3, Radial360, 4, Radial180, 5)] 
        _FillDirection ("Fill Direction", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _FillAmount;
            fixed4 _FillColor;
            fixed4 _BackgroundColor;
            float _FillDirection;

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Fill 마스크 계산
                float fillMask = 0;
                
                if (_FillDirection == 0) // Left to Right
                {
                    fillMask = step(i.uv.x, _FillAmount);
                }
                else if (_FillDirection == 1) // Right to Left
                {
                    fillMask = step(1.0 - i.uv.x, _FillAmount);
                }
                else if (_FillDirection == 2) // Bottom to Top
                {
                    fillMask = step(i.uv.y, _FillAmount);
                }
                else if (_FillDirection == 3) // Top to Bottom
                {
                    fillMask = step(1.0 - i.uv.y, _FillAmount);
                }
                else if (_FillDirection == 4) // Radial 360 (시계방향)
                {
                    // 중심에서의 상대 좌표
                    float2 center = float2(0.5, 0.5);
                    float2 dir = i.uv - center;
                    
                    // 각도 계산 (0도를 위쪽으로, 시계방향)
                    float angle = atan2(dir.x, dir.y);  // atan2(x, y)로 순서 바꿔서 12시 방향이 0도
                    angle = angle / (2.0 * 3.14159265) + 0.5; // -0.5~0.5를 0~1로 변환
                    if (angle < 0) angle += 1.0; // 음수 각도 보정
                    
                    // 시계방향이므로 각도를 반전
                    angle = 1.0 - angle;
                    
                    fillMask = step(angle, _FillAmount);
                }
                else if (_FillDirection == 5) // Radial 180 (반원)
                {
                    float2 center = float2(0.5, 0.5);
                    float2 dir = i.uv - center;
                    
                    float angle = atan2(dir.x, dir.y);
                    angle = angle / 3.14159265 + 0.5; // -0.5~0.5를 0~1로 변환 (180도)
                    if (angle < 0) angle += 1.0;
                    
                    angle = 1.0 - angle;
                    fillMask = step(angle, _FillAmount);
                }
                
                // 색상 보간
                fixed4 finalColor = lerp(_BackgroundColor, _FillColor, fillMask);
                
                // 텍스처 알파와 버텍스 컬러 적용
                finalColor.a *= texColor.a * i.color.a;
                
                return finalColor;
            }
            ENDCG
        }
    }
}