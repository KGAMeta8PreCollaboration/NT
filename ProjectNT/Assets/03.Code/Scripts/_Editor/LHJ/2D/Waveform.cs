using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Waveform : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height = 64;
    [SerializeField] private Color backgroundColor = Color.black;
    [SerializeField] private Color waveformColor = Color.yellow;
    //[SerializeField] private GameObject playBarPrefab;

    private AudioSource _audioSource = null;
    public SpriteRenderer _spriteRenderer = null;
    private int sampleSize;
    private float[] samples = null;
    private float[] waveform = null;

    public bool isLoaded = false;

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
    }

    public int maxNum;

    const int MAX_FIXEL = 8192;
    private Texture2D GetWaveform()
    {
        //노래는 잘리면 안되므로 올림한다
        width = Mathf.CeilToInt(_audioSource.clip.length);
        maxNum = (width * 100 >= MAX_FIXEL) ? MAX_FIXEL / width : 100;
        width = maxNum * width;

        //텍스쳐 크기 설정
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        waveform = new float[width];

        //샘플 갯수
        sampleSize = _audioSource.clip.samples * _audioSource.clip.channels;
        samples = new float[sampleSize];
        _audioSource.clip.GetData(samples, 0);

        int packSize = Mathf.Max(1, sampleSize / width);

        //픽셀만큼 waveform넓히기
        for (int w = 0; w < width; w++)
        {
            waveform[w] = Mathf.Abs(samples[w * packSize]);
        }

        //배경색으로 texture 채우기
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, backgroundColor);
            }
        }

        //sample 그리기
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < waveform[x] * (float)height * 0.75f; y++)
            {
                texture.SetPixel(x, height / 2 + y, waveformColor);
                texture.SetPixel(x, height / 2 - y, waveformColor);
            }
        }

        texture.Apply();
        isLoaded = true;
        return texture;
    }
}

//waveform을 더 자세하게 그리고 싶으면 이걸 넣으면 된다.
//for (int w = 0; w < width; w++)
//{
//    float sum = 0f;
//    for (int i = 0; i < packSize; i++)
//    {
//        int index = w * packSize + i;
//        if (index < samples.Length)
//        {
//            sum += Mathf.Abs(samples[index]);
//        }
//    }
//    waveform[w] = sum / packSize;
//}
