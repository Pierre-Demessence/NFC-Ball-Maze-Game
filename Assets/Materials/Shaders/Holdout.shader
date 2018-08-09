// Modified from http://wiki.unity3d.com/index.php/DepthMask
// The SetRenderQueue script on the page is awful, an alternative has been implemented
Shader "Holdout" {
 
	SubShader {
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.
 
		Tags {"Queue" = "Geometry-1" }
 
		// Don't draw in the RGBA channels; just the depth buffer
 
		ColorMask 0
		ZWrite On
 
		// Do nothing specific in the pass:
 
		Pass {}
	}
}