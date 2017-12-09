using UnityEngine;
using Utils;
namespace Manager
{
    public enum CameraType : int
    {
        Main = 0,
        UI = 1,
        Guide = 2,
        Skill = 3,
    }

    public class CameraManager : Singleton<CameraManager>
    {
 
        private Camera skillCamera = null;

        private Camera guideCamera = null;

        private Camera mainCamera = null;

        private Camera uiCamera = null;

        public override void Initialize()
        {
            base.Initialize();

            RegisterCameras();
        }

        public override void Release()
        {
            base.Release();
        }

        private void RegisterCameras()
        {
            mainCamera = Camera.main;
            uiCamera = GameObject.FindGameObjectWithTag(TagDefines.TAG_UI_CAMERA).GetComponent<Camera>();
            skillCamera = GameObject.FindGameObjectWithTag(TagDefines.TAG_SKILL_CAMERA).GetComponent<Camera>();
            guideCamera = GameObject.FindGameObjectWithTag(TagDefines.TAG_GUIDE_CAMERA).GetComponent<Camera>();
        }

        public void openCamera(CameraType type)
        {
            if (CameraType.Skill == type)
            {
                skillCamera.enabled = true;
            }
            else if (CameraType.Guide == type)
            {
                guideCamera.enabled = true;
            }
        }

        public void closeCamera(CameraType type)
        {
            if (CameraType.Skill == type)
            {
                skillCamera.enabled = false;
            }
            else if (CameraType.Guide == type)
            {
                guideCamera.enabled = false;
            }
        }
    }
}
