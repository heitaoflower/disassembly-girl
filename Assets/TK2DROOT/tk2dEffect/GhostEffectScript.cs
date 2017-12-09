using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostSprite : MonoBehaviour
{
    private float _duration = default(float);
    private float _alpha = default(float);
    private tk2dSprite _sprite = null;
    private Vector3 _originalScale = Vector3.one;
    private Vector3 _originalPosition = Vector3.zero;
    private Quaternion _originalRotation = Quaternion.identity;

    private bool _initialized = false;
    private bool _canBeReused = false;

    private tk2dSprite Sprite
    {
        get
        {
            return _sprite;
        }
    }

    public void Initialize(tk2dSprite sprite, float duration, float alpha)
    {
        if (this.Sprite == null)
        {
            _sprite = tk2dSprite.AddComponent(this.gameObject, sprite.Collection, sprite.spriteId);
            _sprite.GetComponent<MeshRenderer>().sortingOrder = sprite.GetComponent<MeshRenderer>().sortingOrder - 1;
            _sprite.GetComponent<MeshRenderer>().sortingLayerID = sprite.GetComponent<MeshRenderer>().sortingLayerID;

        }
        else
        {
            _sprite.SetSprite(sprite.spriteId);
        }

        _alpha = alpha;
        Color color = sprite.color;
        color.a = _alpha;

        _sprite.color = color;
        _duration = duration;

        _originalPosition = sprite.transform.position;
        transform.position = _originalPosition;

        _originalScale = sprite.transform.localScale;
        transform.localScale = _originalScale;

        _originalRotation = sprite.transform.localRotation;
        transform.localRotation = _originalRotation;

        _initialized = true;

        gameObject.SetActive(true);

        StartCoroutine(RunProcessor());
    }

    public bool CanBeReused()
    {
        return _canBeReused;
    }

    void Update()
    {
        if (_initialized)
        {
            transform.position = _originalPosition;
            transform.localScale = _originalScale;
            transform.localRotation = _originalRotation;
        }
    }

    private IEnumerator RunProcessor()
    {
        bool finished = false;

        float startTime = Time.time;

        Color color = _sprite.color;

        while (!finished)
        {
            float timeSinceStart = Time.time - startTime;
            float percentComplete = timeSinceStart / _duration;

            if (percentComplete >= 1)
            {
                finished = true;

                color.a = 0;
                Sprite.color = color;
            }
            else
            {
                color.a = Mathf.Lerp(_alpha, 0, percentComplete);
                Sprite.color = color;
            }

            yield return null;
        }

        _canBeReused = true;
        gameObject.SetActive(false);
        _initialized = false;
    }

}

public class GhostEffectScript : MonoBehaviour
{

    [SerializeField]
    public float _spwanRate = default(float);
    [SerializeField]
    public int _startSize = 1;
    [SerializeField, Range(0, 1)]
    public float _alpha = 0.5f;
    [SerializeField]
    public float _duration = 1f;

    private List<GhostSprite> _inactiveGhostSpritesPool = null;
    private float _nextSpwanTime = default(float);
    private Queue<GhostSprite> _ghostSpritesQueue = null;
    private tk2dSprite _sprite = null;
    private GameObject _ghostSpritesParent = null;
    private bool _hasStarted = false;

    private static readonly int DEFAULT_POOL_SIZE = 5;

    private static readonly string DEFAULT_CONTAINER_NAME = "GHOST_SPRITES_CONTAINER";

    void Awake()
    {
        _sprite = GetComponent<tk2dSprite>();
    }

    public List<GhostSprite> InactiveGhostSpritePool
    {
        get
        {
            if (_inactiveGhostSpritesPool == null)
            {
                _inactiveGhostSpritesPool = new List<GhostSprite>(DEFAULT_POOL_SIZE);
            }

            return _inactiveGhostSpritesPool;
        }

        set { _inactiveGhostSpritesPool = value; }
    }

    public Queue<GhostSprite> GhostSpritesQueue
    {
        get
        {
            if (_ghostSpritesQueue == null)
            {
                _ghostSpritesQueue = new Queue<GhostSprite>(_startSize);
            }

            return _ghostSpritesQueue;
        }
        set { _ghostSpritesQueue = value; }
    }

    public GameObject GhostSpriteParent
    {
        get
        {
            if (_ghostSpritesParent == null)
            {
                _ghostSpritesParent = new GameObject();
                _ghostSpritesParent.transform.position = Vector3.zero;
                _ghostSpritesParent.transform.localScale = Vector3.one;                
                _ghostSpritesParent.name = DEFAULT_CONTAINER_NAME;
            }

            return _ghostSpritesParent;
        }

        set { _ghostSpritesParent = value; }
    }

    public void Initialize(int startSize, float spwanRate, float duration, float alpha)
    {
        _startSize = startSize;
        _spwanRate = spwanRate;
        _duration = duration;
        _alpha = alpha;
    }

    public void StartEffect()
    {
        _nextSpwanTime = Time.time;

        _hasStarted = true;
    }

    public void StopEffect()
    {
        _hasStarted = false;
    }

    void OnDestroy()
    {
        Destroy(_ghostSpritesParent);
    }

    void Update()
    {
        if (_hasStarted)
        {
            if (Time.time >= _nextSpwanTime)
            {
                if (GhostSpritesQueue.Count == _startSize)
                {
                    GhostSprite peekedGhostSprite = GhostSpritesQueue.Peek();

                    bool canBeReused = peekedGhostSprite.CanBeReused();

                    if (canBeReused)
                    {
                        GhostSpritesQueue.Dequeue();
                        GhostSpritesQueue.Enqueue(peekedGhostSprite);

                        peekedGhostSprite.Initialize(_sprite, _duration, _alpha);

                        _nextSpwanTime += _spwanRate;
                    }
                    else
                    {
                        return;
                    }
                }

                if (GhostSpritesQueue.Count < _startSize)
                {
                    GhostSprite newGhostSprite = Get();
                    GhostSpritesQueue.Enqueue(newGhostSprite);

                    newGhostSprite.Initialize(_sprite, _duration, _alpha);

                    _nextSpwanTime += _spwanRate;
                }

                if (_ghostSpritesQueue.Count > _startSize)
                {
                    int size = GhostSpritesQueue.Count - _startSize;

                    for (int index = 0; index < size; index++)
                    {
                        GhostSprite ghostSprite = GhostSpritesQueue.Dequeue();
                        InactiveGhostSpritePool.Add(ghostSprite);
                    }

                    return;
                }
            }
        }
    }

    private GhostSprite Get()
    {
        for (int index = 0; index < InactiveGhostSpritePool.Count; index++)
        {
            if (InactiveGhostSpritePool[index].CanBeReused())
            {
                return InactiveGhostSpritePool[index];
            }
        }

        return CreateNewGhostSprite();
    }

    private GhostSprite CreateNewGhostSprite()
    {
        GameObject go = new GameObject();
        go.transform.position = transform.position;
        go.transform.parent = GhostSpriteParent.transform;

        GhostSprite ghostSprite = go.AddComponent<GhostSprite>();

        return ghostSprite;
    }
}
