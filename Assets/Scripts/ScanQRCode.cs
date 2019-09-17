using UnityEngine;
using System.Collections;
using ZXing;
using UnityEngine.UI;

public class ScanQRCode : MonoBehaviour 
{
	private WebCamTexture webCamTexture;
	private string resultText;
	private Material quadMat;

	// Use this for initialization
	void Start () 
	{
		webCamTexture = new WebCamTexture (4096, 4096);
		WebCamDevice[] devices = WebCamTexture.devices;
		webCamTexture.deviceName = devices[0].name;
		quadMat = GetComponent<Renderer>().material;
		quadMat.mainTexture = webCamTexture;
		webCamTexture.Play ();

		InvokeRepeating ("Scan", 1f, 0.5f);
	}

	void Update()
	{
		transform.rotation = Quaternion.AngleAxis(webCamTexture.videoRotationAngle, -Vector3.forward);

		var screenAspect = (float)Screen.width / Screen.height;
		var webCamAspect = (float)webCamTexture.width / webCamTexture.height;

		var rot90 = (webCamTexture.videoRotationAngle / 90) % 2 != 0;
		if (rot90) 
		{
			webCamAspect = 1.0f / webCamAspect;
		}

		float sx, sy;
		if (webCamAspect < screenAspect) //0.5625 > 0.5622189
		{
			sx = webCamAspect;
			sy = 1.0f;
		}
		else
		{
			sx = screenAspect;
			sy = screenAspect / webCamAspect;
		}

		if (rot90) 
		{
			transform.localScale = new Vector3 (sy, sx, 1);
		} 
		else 
		{
			transform.localScale = new Vector3 (sx, sy, 1);
		}
			
		var mirror = webCamTexture.videoVerticallyMirrored;
		quadMat.mainTextureOffset = new Vector2(0, mirror ? 1 : 0);
		quadMat.mainTextureScale = new Vector2(1, mirror ? -1 : 1);
	}

	private void Scan()
	{
		if (webCamTexture != null && webCamTexture.width > 100) 
		{
			resultText = Decode(webCamTexture.GetPixels32 (), webCamTexture.width, webCamTexture.height);
            if(resultText != null)
            {
                GameManager.instance.readJson = resultText;
                GameManager.instance.LoadScene("SnitchWorld");
            }
        }
	}

	public string Decode(Color32[] colors, int width, int height)
	{
		BarcodeReader reader = new BarcodeReader ();
		var result = reader.Decode (colors, width, height);
		if (result != null) 
		{
			return result.Text;
		}
		return null;
	}
}
