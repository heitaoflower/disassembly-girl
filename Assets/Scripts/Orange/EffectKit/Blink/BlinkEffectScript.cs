using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Orange.EffectKit
{
    public class BlinkEffectScript : MonoBehaviour
    {

        [SerializeField]
        private Material _blinkMaterial = null;
        private Dictionary<Renderer, Material> _defaultMaterials = null;
        private Dictionary<Renderer, Material> _blinkMaterials = null;
        private Renderer[] _rendereres = null;

        private static Dictionary<Texture, Material> _blinkMaterialsCache = new Dictionary<Texture, Material>();
        private void Start()
        {
            _rendereres = GetComponentsInChildren<Renderer>();

            _defaultMaterials = _rendereres.ToDictionary(p => p, p => p.sharedMaterial);
            _blinkMaterials = new Dictionary<Renderer, Material>();
            foreach (var sr in _defaultMaterials)
            {
                if (sr.Key is SpriteRenderer)
                {
                    _blinkMaterials[sr.Key] = _blinkMaterial;
                }
                else
                {
                    Material mat;
                    Texture tex = sr.Key.sharedMaterial.mainTexture;

                    if (!_blinkMaterialsCache.TryGetValue(tex, out mat))
                    {
                        mat = Instantiate(_blinkMaterial) as Material;
                        mat.mainTexture = tex;
                        _blinkMaterialsCache[tex] = mat;
                    }
                    _blinkMaterials[sr.Key] = mat;
                }
            }
        }

        public void Blink(float waitSeconds, float blinkSeconds)
        {
            if (_defaultMaterials == null || _defaultMaterials.Count == 0 || _blinkMaterial == null)
            {
                return;
            }

            StopCoroutine("BlinkCoroutine");
            StartCoroutine(BlinkCoroutine(waitSeconds, blinkSeconds));
        }

        private IEnumerator BlinkCoroutine(float waitSeconds, float blinkSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);

            foreach (var sr in _defaultMaterials)
            {
                sr.Key.sharedMaterial = _blinkMaterials[sr.Key];
            }

            yield return new WaitForSeconds(blinkSeconds);

            foreach (var sr in _defaultMaterials)
            {
                sr.Key.sharedMaterial = sr.Value;
            }
        }
    }
}
