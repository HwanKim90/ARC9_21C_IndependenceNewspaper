using UnityEngine;
using UnityEngine.UI;

namespace Arc9.Unity.KioskToolkit
{
    [RequireComponent(typeof(Material))]
    public class CanvasUiBlur : MonoBehaviour
    {
        const int _BlurOffRadius_ = 1;
        public int DefaultBlurOnRadius = 30;
        public float Duration = 1;

        private Image mImage;
        private Material mMaterial;
        private int mNewBlurRadius;
        private float mTargetValue;

        private void Awake()
        {
            mImage = GetComponent<Image>();

            Debug.Assert(mImage != null);

            mMaterial = mImage.material;
        }
        void Start()
        {

        }

        public void BlurOn()
        {
            mNewBlurRadius = DefaultBlurOnRadius;

            mNewBlurRadius = Mathf.Max(Mathf.Min(mNewBlurRadius, 255), 1);

            mTargetValue = mMaterial.GetInt("_Radius");

            gameObject.SetActive(true);
        }

        public void BlurOff()
        {
            mNewBlurRadius = _BlurOffRadius_;

            mNewBlurRadius = Mathf.Max(Mathf.Min(mNewBlurRadius, 255), 1);

            mTargetValue = mMaterial.GetInt("_Radius");
        }

        // Update is called once per frame
        void Update()
        {
            if (mMaterial != null)
            {

                int currentRadius = mMaterial.GetInt("_Radius");

                if (currentRadius != mNewBlurRadius)
                {
                    if (currentRadius > mNewBlurRadius)
                    {
                        mTargetValue = ((float)(mTargetValue) - Time.deltaTime * ((float)DefaultBlurOnRadius / Duration));

                        mTargetValue = Mathf.Max(mTargetValue, mNewBlurRadius);
                    }
                    else
                    {
                        mTargetValue = ((float)(mTargetValue) + Time.deltaTime * ((float)DefaultBlurOnRadius / Duration));

                        mTargetValue = Mathf.Min(mTargetValue, mNewBlurRadius);
                    }

                    mMaterial.SetInt("_Radius", (int)mTargetValue);
                }

                if (mNewBlurRadius == _BlurOffRadius_ && currentRadius == _BlurOffRadius_)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}