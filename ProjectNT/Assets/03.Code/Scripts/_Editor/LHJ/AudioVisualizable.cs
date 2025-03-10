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
    [SerializeField] private float higthScale = 1f;
    [Header("1초당 샘플링할 횟수(높을수록 해상도 높아짐)")]
    [SerializeField] private float samplesPerSecond = 100;
    [SerializeField] private Color backgroundColor = Color.black;
    [SerializeField] private Color waveColor = Color.green;

    private AudioSourceManager _audioSourceManager;
    private Texture2D _waveformTexture;
    //픽셀을 그릴 배열
    private Color[] _pixels;
    private Material _targetMaterial;

    private void Awake()
    {
        _audioSourceManager = FindObjectOfType<AudioSourceManager>();
    }

    private void Start()
    {
        InitWaveform();
    }

    private void InitWaveform()
    {
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                _targetMaterial = new Material(renderer.material);

                CreateWaveformTexture();
                GenerateWaveform();

                _targetMaterial.mainTexture = _waveformTexture;
                renderer.material = _targetMaterial;

                float duration = _audioSourceManager.AudioDuration;
                float height = duration * higthScale;
                targetObject.transform.localScale = new Vector3(widthScale / 10f, 1, height / 10f);
            }
        }
    }

    //waveform 텍스쳐 생성 후 _pixels에 담아줌
    private void CreateWaveformTexture()
    {
        int duration = _audioSourceManager.AudioDuration;
        print($"오디오 클립의 길이 : {duration}");
        //텍스쳐의 높이 = 오디오의 길이 * 초당 샘플링 수(샘플링 수가 높아질 수록 자세한 파형을 그릴 수 있다)
        int height = duration * (int)samplesPerSecond;

        if (height > MAX_TEXTUREWIDTH)
        {
            float ratio = MAX_TEXTUREWIDTH / duration;
            height = (int)(duration * ratio);
            print($"heightPerSecond의 최대값 : {ratio}");
        }

        //거의 256이면 깨끗한 해상도가 나옴
        int textureWidth = 256;
        //픽셀 초기화
        _waveformTexture = new Texture2D(textureWidth, height, TextureFormat.RGBA32, false);
        //픽셀 배열 크기 초기화
        _pixels = new Color[textureWidth * height];
    }

    private void GenerateWaveform()
    {
        //sample은 0 ~ 1 사이의 값
        float[] samples = new float[_audioSourceManager.AudioSource.clip.samples];
        //0 -> 샘플을 0초부터 가져옴(44100이 1초)
        _audioSourceManager.AudioSource.clip.GetData(samples, 0);

        ClearTexture();

        //1픽셀 당 샘플 수
        int samplesPerPixel = samples.Length / _waveformTexture.height;

        //가로 픽셀에 대해 반복
        for (int y = 0; y < _waveformTexture.height; y++)
        {
            //최대 진폭값 -> 픽셀안에 대표값
            float maxSample = 0f;

            //1픽셀에 있는 샘플 안에서 
            for (int i = 0; i < samplesPerPixel; i++)
            {
                //샘플들을 배열처럼 확인
                int sampleIndex = y * samplesPerPixel + i;
                //끝에 값 예외
                if (sampleIndex < samples.Length)
                {
                    //각 배열들의 값 확인
                    float sample = Mathf.Abs(samples[sampleIndex]);
                    //1픽셀 안에 대표값만 필요
                    maxSample = Mathf.Max(maxSample, sample);
                }
            }

            //텍스쳐의 중앙
            int centerX = _waveformTexture.width / 2;
            //샘플의 넓이 = 최대 진폭값 * 텍스쳐 높이
            int sampleWidth = (int)(maxSample * _waveformTexture.width);

            //위 아래로 왔다갔다 하면서 그림
            for (int x = centerX - sampleWidth / 2; x < centerX + sampleWidth / 2; x++)
            {
                //범위 제한
                if (x >= 0 && x < _waveformTexture.width)
                {
                    //2차원 배열을 1차원 배열로 나타냄
                    _pixels[y * _waveformTexture.width + x] = waveColor;
                }
            }
        }
        //즉 위에 내용
        //1. 1픽셀 안에서의 최대값을 구함
        //2. 텍스쳐 중앙에서 그 값을 위 아래로 뺀 값에 계속해서 pixel을 찍음
        //3. 1, 2번 반복

        _waveformTexture.SetPixels(_pixels);
        _waveformTexture.Apply();
    }

    private void ClearTexture()
    {
        for (int i = 0; i < _pixels.Length; i++)
        {
            _pixels[i] = backgroundColor;
        }
    }
}
