using System;

using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static AudioClip WavToAudioClip(byte[] wavFile, string clipName)
    {
        using MemoryStream stream = new MemoryStream(wavFile);
        using BinaryReader reader = new BinaryReader(stream);
        if (new string(reader.ReadChars(4)) != "RIFF")
        {
            Debug.LogError("WAV 파일이 아닙니다.");
            return null;
        }
        reader.ReadInt32();
        if (new string(reader.ReadChars(4)) != "WAVE")
        {
            Debug.LogError("WAV 포맷이 아닙니다.");
            return null;
        }

        // "fmt " 청크 찾기
        while (new string(reader.ReadChars(4)) != "fmt ")
        {
            int chunkSize = reader.ReadInt32();
            reader.BaseStream.Position += chunkSize;
        }

        reader.ReadInt32(); // 청크 크기
        int audioFormat = reader.ReadInt16();
        int numChannels = reader.ReadInt16();
        int sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // 바이트 속도
        reader.ReadInt16(); // 블록 정렬
        int bitsPerSample = reader.ReadInt16();

        // "data" 청크 찾기
        while (new string(reader.ReadChars(4)) != "data")
        {
            int chunkSize = reader.ReadInt32();
            reader.BaseStream.Position += chunkSize;
        }

        int dataSize = reader.ReadInt32();
        byte[] data = reader.ReadBytes(dataSize);

        // PCM 데이터 변환
        float[] samples = ConvertWavToFloat(data, bitsPerSample);
        AudioClip audioClip = AudioClip.Create(clipName, samples.Length, numChannels, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    private static float[] ConvertWavToFloat(byte[] wavData, int bitsPerSample)
    {
        int bytesPerSample = bitsPerSample / 8;
        int sampleCount = wavData.Length / bytesPerSample;
        float[] ret = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            if (bitsPerSample == 16)
            {
                short sample = BitConverter.ToInt16(wavData, i * 2);
                ret[i] = sample / 32768f;
            }
            else if (bitsPerSample == 8)
            {
                ret[i] = (wavData[i] - 128) / 128f;
            }
        }

        return ret;
    }
}