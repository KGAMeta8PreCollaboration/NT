using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizable : MonoBehaviour
{
    //텍스쳐의 최대 크기
    public static int MAX_TEXTUREWIDTH = 16384;

    [Header("waveform을 표시할 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("waveform의 너비(클수록 가로로 길어짐)")]
    [SerializeField] private float widthScale = 1f;
    [Header("1초당 표시될 waveform의 높이(클수록 새로로 길어짐)")]
    [SerializeField] private float heightScale = 1f;
    [Header("1초당 샘플링할 횟수(높을수록 해상도 높아짐)")]
    [SerializeField] private float samplesPerSecond = 100;
    [SerializeField] private Color backgroundColor = Color.black;
    [SerializeField] private Color waveColor = Color.green;

    private AudioSourceManager _audioSourceManager;
    private Texture2D _waveformTexture;
    //픽셀을 그릴 배열
    private Color[] _pixels;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
        if (targetObject != null)
        {
            _spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }
    }
    public void GenerateWaveform()
    {
        if (_audioSourceManager == null || _audioSourceManager.AudioSource.clip == null)
        {
            Debug.LogError("No audio clip found!");
            return;
        }

        AudioClip clip = _audioSourceManager.AudioSource.clip;
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        // 텍스처 생성
        int width = 512;  // 고정 너비
        int height = Mathf.CeilToInt(clip.length * 100);  // 1초당 100픽셀
        _waveformTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // 배경색 설정
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = backgroundColor;
        }

        // 샘플당 픽셀 수 계산
        float samplesPerPixel = (float)samples.Length / height;

        // 파형 그리기
        for (int y = 0; y < height; y++)
        {
            // 현재 높이에 해당하는 샘플 범위 계산
            int startSample = Mathf.FloorToInt(y * samplesPerPixel);
            int endSample = Mathf.FloorToInt((y + 1) * samplesPerPixel);

            // 해당 범위의 최대 진폭 찾기
            float maxAmplitude = 0f;
            for (int s = startSample; s < endSample && s < samples.Length; s++)
            {
                float amplitude = Mathf.Abs(samples[s]);
                if (amplitude > maxAmplitude) maxAmplitude = amplitude;
            }

            // 진폭을 너비로 변환
            int centerX = width / 2;
            int waveWidth = Mathf.RoundToInt(maxAmplitude * width * 0.5f);

            // 파형 그리기 (중앙에서 좌우로)
            for (int x = centerX - waveWidth; x < centerX + waveWidth; x++)
            {
                if (x >= 0 && x < width)
                {
                    pixels[y * width + x] = waveColor;
                }
            }
        }

        // 텍스처에 픽셀 적용
        _waveformTexture.SetPixels(pixels);
        _waveformTexture.Apply();

        // 스프라이트 생성 및 적용
        _waveformTexture.filterMode = FilterMode.Bilinear;
        Sprite waveformSprite = Sprite.Create(
            _waveformTexture,
            new Rect(0, 0, _waveformTexture.width, _waveformTexture.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        // 스프라이트 렌더러에 적용
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = waveformSprite;
            targetObject.transform.localScale = new Vector3(widthScale, heightScale, 1);
        }
    }
}
//    public void InitWaveform()
//    {
//        if (targetObject != null)
//        {
//            _targetSpriteRenderer = targetObject.GetComponent<SpriteRenderer>();
//            if (_targetSpriteRenderer != null)
//            {
//                CreateWaveformTexture();
//                GenerateWaveform();
//                CreateAndApplySprite();

//                float duration = _audioSourceManager.AudioDuration;
//                float height = duration * heightScale;
//                targetObject.transform.localScale = new Vector3(widthScale, height, 1);
//            }
//        }
//    }

//    //waveform 텍스쳐 생성 후 _pixels에 담아줌
//    private void CreateWaveformTexture()
//    {
//        int duration = _audioSourceManager.AudioDuration;
//        print($"오디오 클립의 길이 : {duration}");
//        int height = duration * (int)samplesPerSecond;

//        if (height > MAX_TEXTUREWIDTH)
//        {
//            float ratio = MAX_TEXTUREWIDTH / duration;
//            height = (int)(duration * ratio);
//            print($"heightPerSecond의 최대값 : {ratio}");
//        }

//        int textureWidth = 512; // 해상도 증가
//        _waveformTexture = new Texture2D(textureWidth, height, TextureFormat.RGBA32, false);
//        _pixels = new Color[textureWidth * height];
//    }

//    private void GenerateWaveform()
//    {
//        float[] samples = new float[_audioSourceManager.AudioSource.clip.samples];
//        _audioSourceManager.AudioSource.clip.GetData(samples, 0);

//        ClearTexture();

//        int samplesPerPixel = samples.Length / _waveformTexture.height;

//        for (int y = 0; y < _waveformTexture.height; y++)
//        {
//            float maxSample = 0f;

//            for (int i = 0; i < samplesPerPixel; i++)
//            {
//                int sampleIndex = y * samplesPerPixel + i;
//                if (sampleIndex < samples.Length)
//                {
//                    float sample = Mathf.Abs(samples[sampleIndex]);
//                    maxSample = Mathf.Max(maxSample, sample);
//                }
//            }

//            int centerX = _waveformTexture.width / 2;
//            float sampleWidth = maxSample * _waveformTexture.width;

//            // 안티앨리어싱을 위한 부드러운 그리기
//            float leftX = centerX - sampleWidth / 2;
//            float rightX = centerX + sampleWidth / 2;

//            for (float x = leftX; x < rightX; x += 0.5f)
//            {
//                int pixelX = Mathf.RoundToInt(x);
//                if (pixelX >= 0 && pixelX < _waveformTexture.width)
//                {
//                    _pixels[y * _waveformTexture.width + pixelX] = waveColor;
//                    // 주변 픽셀에 알파값이 있는 색상 적용
//                    if (pixelX > 0)
//                        _pixels[y * _waveformTexture.width + (pixelX - 1)] = new Color(waveColor.r, waveColor.g, waveColor.b, 0.5f);
//                    if (pixelX < _waveformTexture.width - 1)
//                        _pixels[y * _waveformTexture.width + (pixelX + 1)] = new Color(waveColor.r, waveColor.g, waveColor.b, 0.5f);
//                }
//            }
//        }

//        _waveformTexture.SetPixels(_pixels);
//        _waveformTexture.Apply();
//    }

//    private void CreateAndApplySprite()
//    {
//        _waveformTexture.filterMode = FilterMode.Bilinear;
//        _waveformTexture.wrapMode = TextureWrapMode.Clamp;

//        Sprite waveformSprite = Sprite.Create(
//            _waveformTexture,
//            new Rect(0, 0, _waveformTexture.width, _waveformTexture.height),
//            new Vector2(0.5f, 0.5f),
//            100f
//        );

//        _targetSpriteRenderer.sprite = waveformSprite;
//    }

//    private void ClearTexture()
//    {
//        for (int i = 0; i < _pixels.Length; i++)
//        {
//            _pixels[i] = backgroundColor;
//        }
//    }
//}
