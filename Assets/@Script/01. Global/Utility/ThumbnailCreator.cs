using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MyTool
{
    public class ThumbnailCreator : MonoBehaviour
    {
        public Camera captureCamera;
        public RenderTexture renderTexture;
        public Vector2 targetSize;
        public GameObject targetObject;

        void Start()
        {
            captureCamera = Camera.main;
            captureCamera.clearFlags = CameraClearFlags.Depth;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(CaptureThumbnail());
        }

        public IEnumerator CaptureThumbnail()
        {
            renderTexture = new RenderTexture((int)targetSize.x, (int)targetSize.y, 24);
            if (captureCamera.targetTexture != null)
            {
                captureCamera.targetTexture.Release();
            }

            captureCamera.targetTexture = renderTexture;

            yield return null;

            RenderTexture.active = renderTexture;

            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, true);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            yield return null;
            var data = texture.EncodeToPNG();
            string name = "Thumbnail_" + targetObject.name;
            string extension = ".png";
            string path = Application.persistentDataPath + "/Thumbnail/";

            Debug.Log(path);

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extension, data);

        }
    }
}
