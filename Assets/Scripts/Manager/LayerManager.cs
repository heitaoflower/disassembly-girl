using Utils;
using View;

using UnityEngine;

using System;
using System.Collections.Generic;

namespace Manager
{
    public class LayerManager : Singleton<LayerManager>
    {
        private static readonly string UI_ROOT = "UI_Root";

        private static readonly string SCENE_ROOT = "Scene_Root";

        private Transform uiRoot = null;

        private Transform sceneRoot = null;

        private GameObject sceneLayer = null;

        private GameObject alertLayer = null;

        private GameObject mainLayer = null;

        private GameObject windowLayer = null;

        private GameObject guideLayer = null;

        private readonly IList<BaseView> viewCache = new List<BaseView>();

        private readonly IDictionary<ViewLayerTypes, Transform> viewLayers = new Dictionary<ViewLayerTypes, Transform>();

        private readonly KeyValuePair<ViewLayerTypes, string> VIEW_LAYER_WINDOW = new KeyValuePair<ViewLayerTypes, string>(ViewLayerTypes.Window, "WindowLayer");

        private readonly KeyValuePair<ViewLayerTypes, string> VIEW_LAYER_MAIN = new KeyValuePair<ViewLayerTypes, string>(ViewLayerTypes.Main, "MainLayer");

        private readonly KeyValuePair<ViewLayerTypes, string> VIEW_LAYER_SCENE = new KeyValuePair<ViewLayerTypes, string>(ViewLayerTypes.Scene, "SceneLayer");

        private readonly KeyValuePair<ViewLayerTypes, string> VIEW_LAYER_ALERT = new KeyValuePair<ViewLayerTypes, string>(ViewLayerTypes.Alert, "AlertLayer");

        private readonly KeyValuePair<ViewLayerTypes, string> VIEW_LAYER_GUIDE = new KeyValuePair<ViewLayerTypes, string>(ViewLayerTypes.Guide, "GuideLayer");

        public override void Initialize()
        {
            base.Initialize();

            RegisterUIRoot();
        }

        public void RegisterUIRoot()
        {
            uiRoot = GameObject.Find(UI_ROOT).transform;
            GameObject.DontDestroyOnLoad(uiRoot);

            this.sceneLayer = GameObject.Find(VIEW_LAYER_SCENE.Value);
            this.mainLayer = GameObject.Find(VIEW_LAYER_MAIN.Value);
            this.windowLayer = GameObject.Find(VIEW_LAYER_WINDOW.Value);
            this.guideLayer = GameObject.Find(VIEW_LAYER_GUIDE.Value);
            this.alertLayer = GameObject.Find(VIEW_LAYER_ALERT.Value);

            viewLayers.Add(VIEW_LAYER_SCENE.Key, this.sceneLayer.transform);
            viewLayers.Add(VIEW_LAYER_MAIN.Key, this.mainLayer.transform);
            viewLayers.Add(VIEW_LAYER_WINDOW.Key, this.windowLayer.transform);
            viewLayers.Add(VIEW_LAYER_GUIDE.Key, this.guideLayer.transform);
            viewLayers.Add(VIEW_LAYER_ALERT.Key, this.alertLayer.transform);
        }

        public void RegisterSceneRoot()
        {
            sceneRoot = GameObject.Find(SCENE_ROOT).transform;
        }

        public override void Release()
        {
            base.Release();
        }

        public void AddObject(Transform child)
        {
            child.SetParent(sceneRoot.transform);
        }

        public T GetPopUpView<T>() where T : BaseView
        {
            Type type = typeof(T);

            ViewAttribute viewAttribute = (ViewAttribute)Attribute.GetCustomAttribute(type, typeof(ViewAttribute));

            foreach (BaseView view in viewCache)
            {
                if (view is T)
                {
                    return (T)view;
                }
            }

            return null;
        }

        public T AddPopUpView<T>(object data = null) where T : BaseView
        {
            Type type = typeof(T);

            ViewAttribute viewAttribute = (ViewAttribute)Attribute.GetCustomAttribute(type, typeof(ViewAttribute));

            Transform layer = null;

            if (!viewLayers.TryGetValue(viewAttribute.layer, out layer))
            {
                return null;
            }

            if (viewAttribute.isSingleton)
            {
                foreach (BaseView _view in viewCache)
                {
                    if (_view is T)
                    {
                        return (T)_view;
                    }
                }
            }

            BaseView view = ResourceUtils.GetComponent<BaseView>(viewAttribute.prefabPath);

            view.transform.parent = layer.transform;
            view.transform.localScale = Vector3.one;
            view.transform.localRotation = Quaternion.identity;
            view.transform.localPosition = layer.transform.localPosition;
            viewCache.Add(view);

            view.Show(data);

            EventBox.Send(CustomEvent.WINDOW_OPEN, view.GetType());

            return (T)view;
        }

        public void RemovePopUpView(BaseView view)
        {
            viewCache.Remove(view);

            view.Hide();

            Destroy(view.gameObject);

            EventBox.Send(CustomEvent.WINDOW_CLOSE, view.GetType());
        }

        public void RemovePopUpView<T>()
        {
            IList<BaseView> views = new List<BaseView>();

            foreach (BaseView view in viewCache)
            {
                if (view is T)
                {
                    views.Add(view);
                }
            }
            foreach (BaseView view in views)
            {
                view.Hide();

                viewCache.Remove(view);

                Destroy(view.gameObject);

                EventBox.Send(CustomEvent.WINDOW_CLOSE, view.GetType());
            }
        }

        public void CloseAllViews()
        {
            foreach (BaseView view in viewCache)
            {
                view.Hide();

                Destroy(view.gameObject);

                EventBox.Send(CustomEvent.WINDOW_CLOSE, view.GetType());
            }

            viewCache.Clear();
        }
    }
}