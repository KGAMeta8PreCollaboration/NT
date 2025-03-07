using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    public Image musicImage;
    public TextMeshProUGUI musicName;

    private TitleMusicData data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MusicDataSetting()
    {
        musicImage = data.musicAlbumArtImage;
        musicName.text = data.musicName;
    }
}
