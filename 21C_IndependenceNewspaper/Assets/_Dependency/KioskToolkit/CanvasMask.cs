using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit
{
    [RequireComponent(typeof(Image))]
    public class CanvasMask : MonoBehaviour
    {
        // Start is called before the first frame update
        public enum PathType
        {
            AbsolutePath,
            RelativeToProjectFolder,
            RelativeToStreamingAssetsFolder,
        }

        public PathType _pathType;
        public string _MaskImageFilePath;
        public Sprite _Sprite;
        private Image mMaskImage;

        private string mMaskImageFullPath;

        private void Awake()
        {
            mMaskImage = GetComponent<Image>();

            Debug.Assert(mMaskImage != null);

            mMaskImage.color = Color.clear;
        }
        void Start()
        {
            if (!string.IsNullOrEmpty(_MaskImageFilePath))
            {
                switch (_pathType)
                {
                    case PathType.AbsolutePath:
                        {
                            mMaskImageFullPath = _MaskImageFilePath;
                        }
                        break;
                    case PathType.RelativeToProjectFolder:
                        {
                            string projectRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
                            projectRoot = projectRoot.Replace('\\', '/');

                            mMaskImageFullPath = projectRoot + '/' + _MaskImageFilePath;
                        }
                        break;
                    case PathType.RelativeToStreamingAssetsFolder:
                        {

                            mMaskImageFullPath = Application.streamingAssetsPath + '/' + _MaskImageFilePath;
                        }
                        break;
                }

                mMaskImageFullPath = mMaskImageFullPath.Replace('\\', '/');

                mMaskImage.color = Color.clear;

                if (System.IO.File.Exists(mMaskImageFullPath))
                {
                    try
                    {
                        
                        byte[] bytes = System.IO.File.ReadAllBytes(mMaskImageFullPath);

                        Texture2D texture = new Texture2D(0, 0);
                        texture.LoadImage(bytes);

                        int textureSize = Mathf.NextPowerOfTwo(texture.width);
                        textureSize = Mathf.Max(Mathf.NextPowerOfTwo(texture.height), textureSize);

                        //texture.Reinitialize(textureSize, textureSize, TextureFormat.RGBA32, false);
                        //texture.LoadImage(bytes);

                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                        _Sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                        //_Sprite.pixelsPerUnit = 100;


                        mMaskImage.type = Image.Type.Simple;
                        mMaskImage.sprite = _Sprite;

                        mMaskImage.color = Color.white;
                    }
                    catch
                    {

                    }



                }

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}