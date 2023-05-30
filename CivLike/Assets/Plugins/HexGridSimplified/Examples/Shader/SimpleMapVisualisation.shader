//ALSO USES VERTEX COLORS FOR FOG OF WAR // Makes unexplored completely Transparent and 
Shader "Wunderwunsch/SimpleMapVisualisation"
{
	Properties
	{
		_MainTex("BaseTerrain", 2D) = "white" {}
		_SecTex("VegetationLayer", 2D) = "black" {}
		_ThirdTex("TopographyLayer", 2D) = "black" {} //should probably be other way around if we stack them on top , could change pass but still more logical to properly name it
		_FoW("FoWBrightness", Range(0,1)) = 0
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

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
		half4 color : COLOR;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	uniform float _FoW;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 vals = i.color;
		if (vals.r > 0.05f && vals.r < 0.99f)
		{
			vals.r = _FoW;
			vals.g = _FoW;
			vals.b = _FoW;
			vals.a = 1;
		}

	fixed4 col = tex2D(_MainTex, i.uv) *vals;
	//fixed4 col2 = tex2D(_SecTex, i.uv) *i.color.a;

	return col;
	}
		ENDCG
	}

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv2 : TEXCOORD1;
		half4 color : COLOR;
	};

	struct v2f
	{
		float2 uv2 : TEXCOORD1;
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
	};

	sampler2D _SecTex;

	float4 _SecTex_ST;
	uniform float _FoW;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv2 = TRANSFORM_TEX(v.uv2, _SecTex);
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture

		//fixed4 col = tex2D(_MainTex, i.uv) *i.color;
		fixed4 vals = i.color;
		if (vals.r > 0.05f && vals.r < 0.99)
		{
			vals.r = _FoW;
			vals.g = _FoW;
			vals.b = _FoW;
			vals.a = 1;
		}
		fixed4 col2 = tex2D(_SecTex, i.uv2) * vals;
		//col2.a = .1f;

	return col2;
	}
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv3 : TEXCOORD2;
		half4 color : COLOR;
	};

	struct v2f
	{
		float2 uv3 : TEXCOORD2;
		float4 vertex : SV_POSITION;
		float4 color : COLOR;
	};

	sampler2D _ThirdTex;

	float4 _ThirdTex_ST;
	uniform float _FoW;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv3 = TRANSFORM_TEX(v.uv3, _ThirdTex);
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture

		//fixed4 col = tex2D(_MainTex, i.uv) *i.color;
		fixed4 vals = i.color;
	if (vals.r > 0.05f && vals.r < 0.99)
	{
		vals.r = _FoW;
		vals.g = _FoW;
		vals.b = _FoW;
		vals.a = 1;
	}
	fixed4 col2 = tex2D(_ThirdTex, i.uv3) * vals;
	//col2.a = .1f;

	return col2;
	}
		ENDCG
	}

	}
}
