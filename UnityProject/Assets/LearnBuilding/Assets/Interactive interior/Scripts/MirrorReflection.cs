using UnityEngine;
using System.Collections;
 
// This is in fact just the Water script from Pro Standard Assets,
// just with refraction stuff removed.
 
[ExecuteInEditMode] // Make mirror live-update even when not in play mode
public class MirrorReflection : MonoBehaviour
{
    public bool m_DisablePixelLights = true;
    public int m_TextureSize = 256;
    public float m_ClipPlaneOffset = 0.07f;
 
	public float custom_scale = 1;
    public LayerMask m_ReflectLayers = -1;
	
	public float zFarPlane = 100;
 
    private Hashtable m_ReflectionCameras = new Hashtable(); // Camera -> Camera table
 
    private RenderTexture m_ReflectionTexture = null;
    private int m_OldReflectionTextureSize = 0;
 
    private static bool s_InsideRendering = false;
  
	
    // This is called when it's known that the object will be rendered by some
    // camera. We render reflections and do other updates here.
    // Because the script executes in edit mode, reflections for the scene view
    // camera will just work!
    public void OnWillRenderObject()
    {
        if( !enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial || !GetComponent<Renderer>().enabled )
            return; 
		
        Camera cam = Camera.current;
		//褋芯褏褉邪薪懈屑 褋褌邪褉芯械 蟹薪邪褔械薪懈械 DA
		if( !cam )
            return;
 
        // Safeguard from recursive reflections.        
        if( s_InsideRendering )
            return;
        s_InsideRendering = true;
 
        Camera reflectionCamera; // 胁芯褌 褝褌芯 褋芯蟹写邪械褌褋褟 泻邪屑械褉邪
        CreateMirrorObjects( cam, out reflectionCamera );
 
		cam.farClipPlane = Camera.main.GetComponent<Camera>().farClipPlane;
		
        // find out the reflection plane: position and normal in world space
        Vector3 pos = transform.position;
        Vector3 normal = transform.up;
 
        // Optionally disable pixel lights for reflection
        int oldPixelLightCount = QualitySettings.pixelLightCount;
        if( m_DisablePixelLights )
            QualitySettings.pixelLightCount = 0;
 
        UpdateCameraModes( cam, reflectionCamera );
 
		
        // Render reflection
        // Reflect camera around reflection plane
        float d = -Vector3.Dot (normal, pos) - m_ClipPlaneOffset;
        Vector4 reflectionPlane = new Vector4 (normal.x, normal.y, normal.z, d);
 
        Matrix4x4 reflection = Matrix4x4.zero;
        CalculateReflectionMatrix (ref reflection, reflectionPlane);
        Vector3 oldpos = cam.transform.position;
        Vector3 newpos = reflection.MultiplyPoint( oldpos );
        reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;
 
		
        // Setup oblique projection matrix so that near plane is our reflection
        // plane. This way we clip everything below/above it for free.
        Vector4 clipPlane = CameraSpacePlane( reflectionCamera, pos, normal, 1.0f );
        Matrix4x4 projection = cam.projectionMatrix;
        CalculateObliqueMatrix (ref projection, clipPlane);
        reflectionCamera.projectionMatrix = projection;
 
        reflectionCamera.cullingMask = ~(1<<4) & m_ReflectLayers.value; // never render water layer
        reflectionCamera.targetTexture = m_ReflectionTexture;
        GL.SetRevertBackfacing (true);
        reflectionCamera.transform.position = newpos;
        Vector3 euler = cam.transform.eulerAngles;
        reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
		
		
        reflectionCamera.Render();
        reflectionCamera.transform.position = oldpos;
        GL.SetRevertBackfacing (false);
        Material[] materials = GetComponent<Renderer>().sharedMaterials;
        foreach( Material mat in materials ) {
            if( mat.HasProperty("_ReflectionTex") )
                mat.SetTexture( "_ReflectionTex", m_ReflectionTexture );
        }
		
		custom_scale = transform.lossyScale.x;
 
        // Set matrix on the shader that transforms UVs from object space into screen
        // space. We want to just project reflection texture on screen.
        Matrix4x4 scaleOffset = Matrix4x4.TRS(
            new Vector3(0.5f,0.5f,0.5f), Quaternion.identity, new Vector3(0.5f,0.5f,0.5f) );
        Vector3 scale = transform.lossyScale;
        //Matrix4x4 mtx = transform.localToWorldMatrix * Matrix4x4.Scale( new Vector3(1.0f/scale.x, 1.0f/scale.y, 1.0f/scale.z) );
		Matrix4x4 mtx = transform.localToWorldMatrix * Matrix4x4.Scale( new Vector3(custom_scale/scale.x, custom_scale/scale.y, custom_scale/scale.z) );
        mtx = scaleOffset * cam.projectionMatrix * cam.worldToCameraMatrix * mtx;
        foreach( Material mat in materials ) {
            mat.SetMatrix( "_ProjMatrix", mtx );
        }
 
        // Restore pixel light count
        if( m_DisablePixelLights )
            QualitySettings.pixelLightCount = oldPixelLightCount;
 
        s_InsideRendering = false;
		
		// cam.far = zFarPlaneCurrentCamera; //褔械褉褌 胁褉芯写械 褉邪斜芯褌邪械褌, 薪芯 薪械 写芯 泻芯薪褑邪 泻邪泻 薪邪写芯
    } 
 
 
    // Cleanup all the objects we possibly have created
    void OnDisable()
    {
        if( m_ReflectionTexture ) {
            DestroyImmediate( m_ReflectionTexture );
            m_ReflectionTexture = null;
        }
        foreach( DictionaryEntry kvp in m_ReflectionCameras )
            DestroyImmediate( ((Camera)kvp.Value).gameObject );
        m_ReflectionCameras.Clear();
    }
 
 
    private void UpdateCameraModes( Camera src, Camera dest )
    {
        if( dest == null )
            return;
        // set camera to clear the same way as current camera
        dest.clearFlags = src.clearFlags;
        dest.backgroundColor = src.backgroundColor;        
        if( src.clearFlags == CameraClearFlags.Skybox )
        {
            Skybox sky = src.GetComponent(typeof(Skybox)) as Skybox;
            Skybox mysky = dest.GetComponent(typeof(Skybox)) as Skybox;
            if( !sky || !sky.material )
            {
                mysky.enabled = false;
            }
            else
            {
                mysky.enabled = true;
                mysky.material = sky.material;
            }
        }
        // update other values to match current camera.
        // even if we are supplying custom camera&projection matrices,
        // some of values are used elsewhere (e.g. skybox uses far plane)
        dest.farClipPlane = src.farClipPlane;
		
		// 胁芯蟹屑芯卸薪芯 褌褍褌, 锌褉芯胁械褉懈屑. 袛邪, 褋褌褉邪薪薪芯械 锌芯胁械写械薪懈械, 褋泻芯褉械械 胁褋械谐芯 薪械 褌褍褌
		// 协褌芯 褋邪屑芯械 锌械褉胁芯械 褔褌芯 屑褘 锌芯锌褘褌邪谢懈褋褜 褋写械谢邪褌褜 (((:
		dest.farClipPlane = zFarPlane; // 褝褌芯 懈 械褋褌褜 褑懈褎褉邪
        dest.nearClipPlane = src.nearClipPlane;
        dest.orthographic = src.orthographic;
        dest.fieldOfView = src.fieldOfView;
        dest.aspect = src.aspect;
        dest.orthographicSize = src.orthographicSize;
    }
 
    // On-demand create any objects we need
    private void CreateMirrorObjects( Camera currentCamera, out Camera reflectionCamera )
    {
        reflectionCamera = null;
 
        // Reflection render texture
        if( !m_ReflectionTexture || m_OldReflectionTextureSize != m_TextureSize )
        {
            if( m_ReflectionTexture )
                DestroyImmediate( m_ReflectionTexture );
            m_ReflectionTexture = new RenderTexture( m_TextureSize, m_TextureSize, 16 );
            m_ReflectionTexture.name = "__MirrorReflection" + GetInstanceID();
            m_ReflectionTexture.isPowerOfTwo = true;
            m_ReflectionTexture.hideFlags = HideFlags.DontSave;
            m_OldReflectionTextureSize = m_TextureSize;
        }
 	
		
        // Camera for reflection
        reflectionCamera = m_ReflectionCameras[currentCamera] as Camera;
		
        if( !reflectionCamera ) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
        {
            GameObject go = new GameObject( "Mirror Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox) );
            reflectionCamera = go.GetComponent<Camera>();
            reflectionCamera.enabled = false;
            reflectionCamera.transform.position = transform.position;
            reflectionCamera.transform.rotation = transform.rotation;
            reflectionCamera.gameObject.AddComponent<FlareLayer>();
            go.hideFlags = HideFlags.HideAndDontSave;
            m_ReflectionCameras[currentCamera] = reflectionCamera;
        }        
		
		
    }
 
    // Extended sign: returns -1, 0 or 1 based on sign of a
    private static float sgn(float a)
    {
        if (a > 0.0f) return 1.0f;
        if (a < 0.0f) return -1.0f;
        return 0.0f;
    } //写邪, 薪芯 泻褍写邪?? 薪邪写芯 薪邪泄褌懈 写邪. 胁芯蟹屑芯卸薪芯 械械 锌芯褌芯屑 屑械薪褟褞褌
 
    // Given position/normal of the plane, calculates plane in camera space.
    private Vector4 CameraSpacePlane (Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Vector3 offsetPos = pos + normal * m_ClipPlaneOffset;
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint( offsetPos );
        Vector3 cnormal = m.MultiplyVector( normal ).normalized * sideSign;
        return new Vector4( cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos,cnormal) );
    }
 
    // Adjusts the given projection matrix so that near plane is the given clipPlane
    // clipPlane is given in camera space. See article in Game Programming Gems 5 and
    // http://aras-p.info/texts/obliqueortho.html
    private static void CalculateObliqueMatrix (ref Matrix4x4 projection, Vector4 clipPlane)
    {
        Vector4 q = projection.inverse * new Vector4(
            sgn(clipPlane.x),
            sgn(clipPlane.y),
            1.0f,
            1.0f
        );
		
        Vector4 c = clipPlane * (2.0F / (Vector4.Dot (clipPlane, q)));
        // third row = clip plane - fourth row
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
    }
 
    // Calculates reflection matrix around the given plane
    private static void CalculateReflectionMatrix (ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F*plane[0]*plane[0]);
        reflectionMat.m01 = (   - 2F*plane[0]*plane[1]);
        reflectionMat.m02 = (   - 2F*plane[0]*plane[2]);
        reflectionMat.m03 = (   - 2F*plane[3]*plane[0]);
 
        reflectionMat.m10 = (   - 2F*plane[1]*plane[0]);
        reflectionMat.m11 = (1F - 2F*plane[1]*plane[1]);
        reflectionMat.m12 = (   - 2F*plane[1]*plane[2]);
        reflectionMat.m13 = (   - 2F*plane[3]*plane[1]);
 
        reflectionMat.m20 = (   - 2F*plane[2]*plane[0]);
        reflectionMat.m21 = (   - 2F*plane[2]*plane[1]);
        reflectionMat.m22 = (1F - 2F*plane[2]*plane[2]);
        reflectionMat.m23 = (   - 2F*plane[3]*plane[2]);
 
        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }
}