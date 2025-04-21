using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public const int MODEDIFF_COUNT = 12;
    public enum ModeDiff
    {
        SOLO_EASY,
        SOLO_NORMAL,
        SOLO_HARD,
        SOLO_EXTREAM,
        DUO1_EASY,
        DUO1_NORMAL,
        DUO1_HARD,
        DUO1_EXTREAM,
        DUO2_EASY,
        DUO2_NORMAL,
        DUO2_HARD,
        DUO2_EXTREAM
    }
    public enum GameMode
    {
        Solo,
        Duo1,
        Duo2
    }
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Extreme
    }

    public enum Details
    {
        SavePathChoice,
        FileSaveFail,
        NoneProjectName,
        NoneArtist,
        NoneBpm,
        NoneBgm,
        NoneBeatNum,
        NoneThumbnail,
        NoneKeySoundFolder,
        SaveWarning,
        FileLoadFail,
        PathSetError,
        SaveFolderExist,
        ThemeAlreadyExist,
        LoadImageFail,
        MakeProjectComplete,
        ChangeProjectInfoComplete,
        DeleteProjectCheck,
        FileDetectFail,
        EditorQuit
    }

    public enum PlayMode
    {
        Player1,
        Player2,
        Single,
        None
    }
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3
    }
}
