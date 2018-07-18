Shader "[Chronicler]/FX/Water Simple" {
    Properties {
      _MainTex ("Texture", 2D) = "grey" {}
    }
 Subshader {
      Tags { "Queue"="Transparent-600" "RenderType"="Transparent" }
      ZWrite on Cull off
	  LOD 0
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0
      struct Input {
          float2 uv_MainTex;
      };
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    }  
  }