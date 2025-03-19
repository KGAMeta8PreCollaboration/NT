using UnityEngine;

public class LongNote : MonoBehaviour
{
    public double startTargetDspTime;
    public double endTargetDspTime;
    public int divideCount = 5;
    public double[] milestones;
    private int currentMilestoneIndex = 0;

    private void Start()
    {
        startTargetDspTime = AudioSettings.dspTime + 3d; // 3초 후 시작
        endTargetDspTime = AudioSettings.dspTime + 7d;   // 7초 후 종료

        double duration = endTargetDspTime - startTargetDspTime;
        print($"롱노트의 지속시간: {duration.ToString("f2")}초");

        CalcMilestones(duration);
    }

    private void CalcMilestones(double noteDuration)
    {
        milestones = new double[divideCount];
        double interval = noteDuration / divideCount;
        for (int i = 0; i < divideCount; i++)
        {
            milestones[i] = startTargetDspTime + (interval * (i + 1));
            print($"판정 시간: {(milestones[i] - startTargetDspTime).ToString("f2")}초");
        }
    }

    public bool CheckMilestone()
    {
        if (currentMilestoneIndex >= milestones.Length)
            return false;

        double currentTime = AudioSettings.dspTime;
        if (currentTime >= milestones[currentMilestoneIndex])
        {
            currentMilestoneIndex++;
            return true;
        }
        return false;
    }
}
