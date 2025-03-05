using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizable : MonoBehaviour
{
    //텍스쳐의 최대 크기
    public static int MAX_TEXTUREWIDTH = 16384;

    [Header("waveform을 표시할 오브젝트")]
    [SerializeField] private GameObject targetObject;
    [Header("waveform의 높이")]
    [SerializeField] private float heightScale = 1f;
    [Header("1초당 표시될 waveform의 너비")]
    [SerializeField] private float widthPerSecond = 1f;
    [Header("1초당 샘플링할 횟수")]
    [SerializeField] private int samplesPerSecond = 100;
    [SerializeField] private Color backgroundColor = Color.black;
    [SerializeField] private Color waveColor = Color.green;

    private AudioSource _audioSource;
    private Texture2D _waveformTexture;
    private Color[] _pixels;
    private Material _targetMaterial;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (targetObject != null)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if(renderer != null)
            {
                _targetMaterial = new Material(renderer.material);

                CreateWaveformTexture();
                GenerateWaveform();

                _targetMaterial.mainTexture = _waveformTexture;
                renderer.material = _targetMaterial;

                float duration = _audioSource.clip.length;
                float width = duration * widthPerSecond;
                targetObject.transform.localScale = new Vector3(width / 10f, 1, heightScale / 10f);
            }
        }
    }

    //waveform 텍스쳐 생성 후 _pixels에 담아줌
    private void CreateWaveformTexture()
    {
        float duration = _audioSource.clip.length;
        print($"오디오 클립의 길이 : {duration}");
        //텍스쳐의 넓이 = 오디오의 길이 * 초당 샘플링 수
        int textureWidth = (int)(duration * samplesPerSecond);

        if (textureWidth > MAX_TEXTUREWIDTH)
        {
            int maxSample = MAX_TEXTUREWIDTH / (int)duration;
            textureWidth = (int)(duration * maxSample);
            print($"WidthPerSecond의 최대값 : {maxSample}");
        }

        print($"텍스쳐의 넓이 : {textureWidth}");

        //거의 256이면 깨끗한 해상도가 나옴
        int textureHeight = 256;
        _waveformTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        _pixels = new Color[textureWidth * textureHeight];
    }

    private void GenerateWaveform()
    {
        //sample은 0 ~ 1 사이의 값
        float[] samples = new float[_audioSource.clip.samples];
        //0 -> 샘플을 0초부터 가져옴(44100이 1초)
        _audioSource.clip.GetData(samples, 0);

        ClearTexture();

        //1픽셀 당 샘플 수
        int samplesPerPixel = samples.Length / _waveformTexture.width;

        //가로 픽셀에 대해 반복
        for (int x = 0; x < _waveformTexture.width; x++)
        {
            //최대 진폭값 -> 픽셀안에 대표값
            float maxSample = 0f;

            //1픽셀에 있는 샘플 안에서 
            for (int i = 0; i < samplesPerPixel; i++)
            {
                //샘플들을 배열처럼 확인
                int sampleIndex = x * samplesPerPixel + i;
                //끝에 값 예외
                if (sampleIndex < samples.Length)
                {
                    //각 배열들의 값 확인
                    float sample = Mathf.Abs(samples[sampleIndex]);
                    if (sample > maxSample)
                    {
                        maxSample = sample;
                    }
                }
            }

            //텍스쳐의 중앙
            int centerY = _waveformTexture.height / 2;
            //샘플의 높이 = 최대 진폭값 * 텍스쳐 높이
            int sampleHeight = (int)(maxSample * _waveformTexture.height);

            //위 아래로 왔다갔다 하면서 그림
            for (int y = centerY - sampleHeight / 2; y < centerY + sampleHeight / 2; y++)
            {
                if (y >= 0 && y < _waveformTexture.height)
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
