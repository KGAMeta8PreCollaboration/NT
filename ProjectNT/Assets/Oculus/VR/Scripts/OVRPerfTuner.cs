using UnityEngine;
using Oculus; // Oculus Integration 설치 후 필요
using static OVRManager; // OVRManager 안의 enum에 접근하기 위해

public class OVRPerfTuner : MonoBehaviour
{
    void Start()
    {
        // 권장 주사율 설정 (지원하는 최대 주사율 중 선택 가능: 72, 80, 90Hz 등)
        OVRPlugin.systemDisplayFrequency = 72f;
        Application.targetFrameRate = 72;

        // 성능 레벨 설정 (SustainedHigh는 안정적 고성능 모드)
        suggestedCpuPerfLevel = ProcessorPerformanceLevel.SustainedHigh;
        suggestedGpuPerfLevel = ProcessorPerformanceLevel.SustainedHigh;

        Debug.Log("✅ OVR 성능 설정 완료");
    }
}
