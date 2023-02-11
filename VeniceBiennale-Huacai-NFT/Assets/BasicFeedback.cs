// Add script to quad and assign material with shader. Play.

using UnityEngine;
using UnityEngine.Video;

public class BasicFeedback : MonoBehaviour 
{
	public int Resolution = 512;
	public Shader BasicFeedbackShader;
	//public VideoClip BasicFeedbackVideoClip;
	Material _Material;
	RenderTexture _Input, _Output;
	public RenderTexture _Video;
	bool swap = true;

	void Blit(RenderTexture source, RenderTexture destination, Material mat, string name)
	{
		RenderTexture.active = destination;
		mat.SetTexture(name, source);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.invertCulling = true;
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.Vertex3(0.0f, 0.0f, 0.0f);
		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.Vertex3(1.0f, 0.0f, 0.0f); 
		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.Vertex3(1.0f, 1.0f, 0.0f); 
		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.Vertex3(0.0f, 1.0f, 0.0f);
		GL.End();
		GL.invertCulling = false;
		GL.PopMatrix();
	}

	void Start () 
	{
		_Input = new RenderTexture(Resolution, Resolution, 0, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
		_Output = new RenderTexture(Resolution, Resolution, 0, RenderTextureFormat.ARGBFloat);  //buffer must be floating point RT
		//_Video = new RenderTexture((int)BasicFeedbackVideoClip.width, (int)BasicFeedbackVideoClip.height, 0, RenderTextureFormat.ARGB32);
		_Material = new Material(BasicFeedbackShader);
		this.gameObject.GetComponent<Renderer>().material = _Material;
		VideoPlayer videoPlayer = this.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
		videoPlayer.renderMode = VideoRenderMode.RenderTexture;
		videoPlayer.targetTexture = _Video;
		//videoPlayer.clip = BasicFeedbackVideoClip;
		videoPlayer.isLooping = true;
		videoPlayer.Play();
	}

	void Update () 
	{
		_Material.SetTexture("_Video", _Video);
		if (swap)
		{
			_Material.SetTexture("_BufferA", _Input);
			Blit(_Input, _Output, _Material, "_BufferA");
			_Material.SetTexture("_BufferA", _Output);
		}
		else
		{
			_Material.SetTexture("_BufferA", _Output);
			Blit(_Output, _Input, _Material,"_BufferA");
			_Material.SetTexture("_BufferA", _Input);
		}
		swap = !swap;
	}

	void OnDestroy ()
	{
		if (_Material != null) Destroy(_Material);
		if (_Video != null) _Video.Release();
		if (_Input != null) _Input.Release();
		if (_Output != null) _Output.Release();
	}
}