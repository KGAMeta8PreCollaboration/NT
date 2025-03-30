using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Waveform : MonoBehaviour
{
    [SerializeField] private int width = 1024;
    [SerializeField] private int height = 64;
    [SerializeField] private Color background = Color.black;
    [SerializeField] private Color waveformColor = Color.yellow;
    [SerializeField] private GameObject playBarPrefab;
    [SerializeField] private Camera cam = null;

    private AudioSource _audioSource = null;
    public SpriteRenderer _spriteRenderer = null;
    private int sampleSize;
    private float[] samples = null;
    private float[] waveform = null;
    private float playBarOffsetX;
    private bool isDragging = false;
    private bool isLoaded = false;

    private void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        isLoaded = false;
    }

    public void CreateWaveform(AudioSource audioSource)
    {
        _audioSource = audioSource;

        Texture2D texture = GetWaveform();

        Rect rect = new Rect(Vector2.zero, new Vector2(width, height));
        _spriteRenderer.sprite = Sprite.Create(texture, rect, Vector2.zero);
        //arrow.transform.position = new Vector3(0f, 0f);
        //arrowOffsetX = -(arrow.GetComponent<SpriteRenderer>().size.x / 2f);

        //cam.transform.position = new Vector3(0f, 0f, -1f);
        //cam.transform.Translate(Vector3.right * (_spriteRenderer.size.x / 2f));
    }

    private void Update()
    {
        //if (_audioSource == null)
        //{
        //    return;
        //}

        if (isLoaded != true)
        {
            return;
        }

        SetPlayBarPos();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    print("마우스 클릭 감지");
        //    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //    if (hit.collider != null && hit.collider.gameObject == arrow)
        //    {
        //        isDragging = true;
        //    }
        //}

        //if (isDragging && Input.GetMouseButton(0))
        //{
        //    print("마우스 드래그 시작");
        //    mousePos.z = 0;
        //    arrow.transform.position = new Vector3(0, mousePos.y);

        //    float progress = Mathf.Clamp01((arrow.transform.position.x - arrowOffsetX) / _spriteRenderer.size.x);
        //    _audioSource.time = progress * _audioSource.clip.length;
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    print("마우스 드래그 끝남");
        //    isDragging = false;
        //}

        //if (!isDragging)
        //{
        //    float xoffset = (_audioSource.time / _audioSource.clip.length) * _spriteRenderer.size.x;
        //    playBarPrefab.transform.position = new Vector3(0, xoffset + arrowOffsetX);
        //}
    }

    private Texture2D GetWaveform()
    {
        int halfHeight = height / 2;
        float heightScale = (float)height * 0.75f;
        width = Mathf.CeilToInt(_audioSource.clip.length) * 100;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        waveform = new float[width];

        sampleSize = _audioSource.clip.samples * _audioSource.clip.channels;
        samples = new float[sampleSize];
        _audioSource.clip.GetData(samples, 0);

        int packSize = Mathf.Max(1, sampleSize / width);
        for (int w = 0; w < width; w++)
        {
            waveform[w] = Mathf.Abs(samples[w * packSize]);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, background);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < waveform[x] * heightScale; y++)
            {
                texture.SetPixel(x, halfHeight + y, waveformColor);
                texture.SetPixel(x, halfHeight - y, waveformColor);
            }
        }

        texture.Apply();
        isLoaded = true;
        //MakePlayBar();
        return texture;
    }

    //private void MakePlayBar()
    //{
    //    if (isLoaded == false)
    //    {
    //        print("아직 로딩이 끝나지 않음");
    //        return;
    //    }

    //    GameObject playBarObj = Instantiate(playBarPrefab);
    //    PlayBar playBar = playBarObj.GetComponent<PlayBar>();
    //    //playBar.transform.SetParent(transform);
    //    playBar.transform.position = new Vector3(0, playBarOffsetX);
    //}

    private void SetPlayBarPos()
    {
        //progress = 현재 노래 시간 / 전체 노래 시간
        float progress = _audioSource.time / _audioSource.clip.length;
        //print($"_audioSource.time : {_audioSource.time}, _audioSource.clip.length : {_audioSource.clip.length}");
        float yOffset = progress * _spriteRenderer.size.x;
        playBarPrefab.transform.position = new Vector3(0, playBarOffsetX + yOffset);
    }
}
