using System;

using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static AudioClip WavToAudioClip(byte[] wavFile, string clipName)
    {
        using MemoryStream stream = new MemoryStream(wavFile);
        using BinaryReader reader = new BinaryReader(stream);

        // "RIFF" 체크
        if (new string(reader.ReadChars(4)) != "RIFF")
        {
            Debug.LogError("WAV 파일이 아닙니다.");
            return null;
        }

        reader.ReadInt32(); // 파일 크기
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

        int fmtChunkSize = reader.ReadInt32();
        int audioFormat = reader.ReadInt16();       // 1 = PCM, 3 = IEEE float
        int numChannels = reader.ReadInt16();
        int sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // byte rate
        reader.ReadInt16(); // block align
        int bitsPerSample = reader.ReadInt16();

        // 확장된 fmt 청크라면 스킵
        if (fmtChunkSize > 16)
        {
            reader.BaseStream.Position += (fmtChunkSize - 16);
        }

        // "data" 청크 찾기
        string chunkID = "";
        while ((chunkID = new string(reader.ReadChars(4))) != "data")
        {
            int chunkSize = reader.ReadInt32();
            reader.BaseStream.Position += chunkSize;
        }

        int dataSize = reader.ReadInt32();
        byte[] data = reader.ReadBytes(dataSize);

        // PCM 또는 Float 데이터 변환
        float[] samples = ConvertWavToFloat(data, bitsPerSample, audioFormat);
        if (samples == null)
        {
            Debug.LogError("지원하지 않는 WAV 포맷입니다.");
            return null;
        }

        AudioClip audioClip = AudioClip.Create(clipName, samples.Length / numChannels, numChannels, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    private static float[] ConvertWavToFloat(byte[] wavData, int bitsPerSample, int audioFormat)
    {
        int bytesPerSample = bitsPerSample / 8;
        int sampleCount = wavData.Length / bytesPerSample;
        float[] ret = new float[sampleCount];

        if (audioFormat == 1) // PCM
        {
            if (bitsPerSample == 16)
            {
                for (int i = 0; i < sampleCount; i++)
                {
                    short sample = BitConverter.ToInt16(wavData, i * 2);
                    ret[i] = sample / 32768f;
                }
            }
            else if (bitsPerSample == 8)
            {
                for (int i = 0; i < sampleCount; i++)
                {
                    ret[i] = (wavData[i] - 128) / 128f;
                }
            }
            else
            {
                Debug.LogError("지원하지 않는 PCM 비트 수: " + bitsPerSample);
                return null;
            }
        }
        else if (audioFormat == 3 && bitsPerSample == 32) // IEEE Float
        {
            for (int i = 0; i < sampleCount; i++)
            {
                ret[i] = BitConverter.ToSingle(wavData, i * 4);
            }
        }
        else
        {
            Debug.LogError($"지원하지 않는 오디오 포맷: format={audioFormat}, bits={bitsPerSample}");
            return null;
        }

        return ret;
    }
}