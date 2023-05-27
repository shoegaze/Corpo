Shader "Custom/Intro Cinematic" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
    
    _NoiseAmplitude ("Noise Amplitude", Float) = 0.025
    _NoiseRate ("Noise Rate", Float) = 0.5
    
    _DistortionAmount ("Distortion Amount", Float) = 50.0
    _DistortionAmplitude ("Distortion Amplitude", Float) = 0.01
    _DistortionRate ("Distortion Rate", Float) = 0.5
  }
  SubShader {
    Tags { 
      "RenderType"="Opaque"
    }
    LOD 100

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;

      v2f vert(appdata v) {
        v2f o;
        
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        
        return o;
      }


      #define PI (acos(-1.0))

      float rand(in float2 vec) {
        float random = dot(vec, float2(12.9898, 78.233));
        random = frac(random * 143758.5453);
        
        return random;
      }

      float _NoiseAmplitude;
      float _NoiseRate;

      float _DistortionAmount;
      float _DistortionAmplitude;
      float _DistortionRate;
      
      fixed4 frag(v2f i) : SV_Target {
        float2 uv = i.uv;
        float2 st = i.uv;
   
        { // Noise
          float2 t = float2(
              0.5 * sin(PI * _Time.y * _NoiseRate) + 0.5,
              0.5 * cos(PI * _Time.y * _NoiseRate) + 0.5
          );
          float2 u = float2(-t.y, t.x);
          float2 dp = float2(
            rand(st + t),
            rand(st + u)
          );

          // DEBUG
          float ampl = _NoiseAmplitude;
          ampl *= 0.5 * sin(PI * _Time.y) + 0.5;
          ampl += 0.0025;
          
          uv += dp * ampl;
        }
     
        { // Horizontal Distortion
          uv.x += _DistortionAmplitude * sin(
            _DistortionAmount * st.y + PI * _Time.y * _DistortionRate
          ) + _DistortionAmplitude;
        }
    
        fixed4 col = tex2D(_MainTex, uv);
        return col;
      }
      ENDCG
    }
  }
}
