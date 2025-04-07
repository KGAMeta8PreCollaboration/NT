using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSensitivityTest : MonoBehaviour
{
    public Button m_minPosYBtn;
    public Button p_minPosYBtn;
    public Button m_maxPosYBtn;
    public Button p_maxPosYBtn;
    public Button m_minRotXBtn;
    public Button p_minRotXBtn;
    public Button m_maxRotXBtn;
    public Button p_maxRotXBtn;
    public Button m_minRotYBtn;
    public Button p_minRotYBtn;
    public Button m_maxRotYBtn;
    public Button p_maxRotYBtn;
    public Button m_speedBtn;
    public Button p_speedBtn;

    public TextMeshProUGUI minPosYText;
    public TextMeshProUGUI maxPosYText;
    public TextMeshProUGUI minRotXText;
    public TextMeshProUGUI maxRotXText;
    public TextMeshProUGUI minRotYText;
    public TextMeshProUGUI maxRotYText;
    public TextMeshProUGUI speedText;

    public ClampedActionBasedController leftcontroller;
    public ClampedActionBasedController rightcontroller;

    private void Start()
    {
        minPosYText.text = leftcontroller.minPosY.ToString();
        maxPosYText.text = leftcontroller.maxPosY.ToString();

        minRotXText.text = leftcontroller.minRotX.ToString();
        maxRotXText.text = leftcontroller.maxRotX.ToString();

        minRotYText.text = leftcontroller.minRotY.ToString();
        maxRotYText.text = leftcontroller.maxRotY.ToString();

        speedText.text = leftcontroller.positionSmoothSpeed.ToString();

        m_minPosYBtn.onClick.AddListener(() => OnMinPosYButtonClick(0.5f, false));
        p_minPosYBtn.onClick.AddListener(() => OnMinPosYButtonClick(0.5f, true));
        m_maxPosYBtn.onClick.AddListener(() => OnMaxPosYButtonClick(0.5f, false));
        p_maxPosYBtn.onClick.AddListener(() => OnMaxPosYButtonClick(0.5f, true));

        m_minRotXBtn.onClick.AddListener(() => OnMinRotXButtonClick(5, false));
        p_minRotXBtn.onClick.AddListener(() => OnMinRotXButtonClick(5, true));
        m_maxRotXBtn.onClick.AddListener(() => OnMaxRotXButtonClick(5, false));
        p_maxRotXBtn.onClick.AddListener(() => OnMaxRotXButtonClick(5, true));

        m_minRotYBtn.onClick.AddListener(() => OnMinRotYButtonClick(5, false));
        p_minRotYBtn.onClick.AddListener(() => OnMinRotYButtonClick(5, true));
        m_maxRotYBtn.onClick.AddListener(() => OnMaxRotYButtonClick(5, false));
        p_maxRotYBtn.onClick.AddListener(() => OnMaxRotYButtonClick(5, true));

        m_speedBtn.onClick.AddListener(() => OnSpeedButtonClick(1, false));
        p_speedBtn.onClick.AddListener(() => OnSpeedButtonClick(1, true));
    }

    public void OnMinPosYButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.minPosY += value;
            rightcontroller.minPosY += value;
        }
        else
        {
            leftcontroller.minPosY -= value;
            rightcontroller.minPosY -= value;
        }

        minPosYText.text = leftcontroller.minPosY.ToString();
    }
    public void OnMaxPosYButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.maxPosY += value;
            rightcontroller.maxPosY += value;
        }
        else
        {
            leftcontroller.maxPosY -= value;
            rightcontroller.maxPosY -= value;
        }

        maxPosYText.text = leftcontroller.maxPosY.ToString();
    }
    public void OnMinRotXButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.minRotX += value;
            rightcontroller.minRotX += value;
        }
        else
        {
            leftcontroller.minRotX -= value;
            rightcontroller.minRotX -= value;
        }

        minRotXText.text = leftcontroller.minRotX.ToString();
    }
    public void OnMaxRotXButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.maxRotX += value;
            rightcontroller.maxRotX += value;
        }
        else
        {
            leftcontroller.maxRotX -= value;
            rightcontroller.maxRotX -= value;
        }

        maxRotXText.text = leftcontroller.maxRotX.ToString();
    }
    public void OnMinRotYButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.minRotY += value;
            rightcontroller.minRotY += value;
        }
        else
        {
            leftcontroller.minRotY -= value;
            rightcontroller.minRotY -= value;
        }

        minRotYText.text = leftcontroller.minRotY.ToString();
    }
    public void OnMaxRotYButtonClick(float value, bool plus)
    {
        if (plus)
        {
            leftcontroller.maxRotY += value;
            rightcontroller.maxRotY += value;
        }
        else
        {
            leftcontroller.maxRotY -= value;
            rightcontroller.maxRotY -= value;
        }

        maxRotYText.text = leftcontroller.maxRotY.ToString();
    }
    public void OnSpeedButtonClick(int value, bool plus)
    {
        if (plus)
        {
            leftcontroller.positionSmoothSpeed += value;
            leftcontroller.rotationSmoothSpeed += value;
            rightcontroller.positionSmoothSpeed += value;
            rightcontroller.rotationSmoothSpeed += value;
        }
        else
        {
            leftcontroller.positionSmoothSpeed -= value;
            leftcontroller.rotationSmoothSpeed -= value;
            rightcontroller.positionSmoothSpeed -= value;
            rightcontroller.rotationSmoothSpeed -= value;
        }

        speedText.text = leftcontroller.positionSmoothSpeed.ToString();
    }

    private void OnDestroy()
    {
        m_minPosYBtn.onClick.RemoveAllListeners();
        p_minPosYBtn.onClick.RemoveAllListeners();
        m_maxPosYBtn.onClick.RemoveAllListeners();
        p_maxPosYBtn.onClick.RemoveAllListeners();
        m_minRotXBtn.onClick.RemoveAllListeners();
        p_minRotXBtn.onClick.RemoveAllListeners();
        m_maxRotXBtn.onClick.RemoveAllListeners();
        p_maxRotXBtn.onClick.RemoveAllListeners();
        m_minRotYBtn.onClick.RemoveAllListeners();
        p_minRotYBtn.onClick.RemoveAllListeners();
        m_maxRotYBtn.onClick.RemoveAllListeners();
        p_maxRotYBtn.onClick.RemoveAllListeners();
        m_speedBtn.onClick.RemoveAllListeners();
        p_speedBtn.onClick.RemoveAllListeners();
    }
}
