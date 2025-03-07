using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMusicData
{
    public Image musicImage;
    public string musicName;
}

public class SinglePlayUI : BaseTitleUI
{

    public TitleMusicData[] titleMusicDatas;

    private int musicNum;

    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
