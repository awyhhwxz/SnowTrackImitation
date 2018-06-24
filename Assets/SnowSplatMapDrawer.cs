using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSplatMapDrawer : MonoBehaviour {

    public Shader DrawShader;
    public List<GameObject> DrawPainterList;
    public RenderTexture RT;
    private Material _drawMaterial;
    private RaycastHit _raycastHit;

    [Range(0, 500)]
    public float Size = 4;
    [Range(0, 1)]
    public float Strength = 1;
    [Range(0, 200)]
    public float Radius = 80;

	// Use this for initialization
	void Start ()
    {
        _drawMaterial = new Material(DrawShader);
        _drawMaterial.SetColor("_DrawColor", Color.red);

        var tempRT = RenderTexture.GetTemporary(RT.width, RT.height);
        var initializeMaterial = new Material(Resources.Load<Shader>("Shader/PureColorDrawShader"));
        Graphics.Blit(tempRT, RT, initializeMaterial);
        RenderTexture.ReleaseTemporary(tempRT);
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (var drawPainter in DrawPainterList)
        {
            if (Physics.Raycast(drawPainter.transform.position, -Vector3.up, out _raycastHit, 1, LayerMask.GetMask("Ground")))
            {
                _drawMaterial.SetVector("_Coordination", new Vector4(_raycastHit.textureCoord.x, _raycastHit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_ShaderSize", Size);
                _drawMaterial.SetFloat("_ShaderStrength", Strength);
                _drawMaterial.SetFloat("_ShaderRadius", Radius);
                var tempRT = RenderTexture.GetTemporary(RT.width, RT.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(RT, tempRT);
                _drawMaterial.SetTexture("_SplatMap", tempRT);
                Graphics.Blit(tempRT, RT, _drawMaterial);
                RenderTexture.ReleaseTemporary(tempRT);
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 200, 200), RT, ScaleMode.ScaleToFit, false, 1);
    }
}
