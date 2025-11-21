using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ScreenShot : MonoBehaviour
{
    public Camera renderCamera;
    public GameObject prefabToCapture; // ÂïÀ» ÇÁ¸®ÆÕ
    public int width = 256;
    public int height = 256;
    public string savePath = "Assets/ItemIcon.png";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CapturePrefab(prefabToCapture);
        }
    }

    public void CapturePrefab(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        RenderTexture rt = new RenderTexture(width, height, 24);
        renderCamera.targetTexture = rt;
        renderCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);
        Debug.Log("PNG saved at: " + savePath);

        RenderTexture.active = null;
        renderCamera.targetTexture = null;
        Destroy(rt);
        Destroy(tex);
        Destroy(instance);
    }
}
