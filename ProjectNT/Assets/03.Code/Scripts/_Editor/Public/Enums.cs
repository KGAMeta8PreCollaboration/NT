using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public const int MODEDIFF_COUNT = 12;
    [Flags]
    public enum ModeDiff
    {
        None = 0,
        SOLO_EASY = 1,
        SOLO_NORMAL = 2,
        SOLO_HARD = 4,
        SOLO_EXTREAM = 8,
        DUO1_EASY = 16,
        DUO1_NORMAL = 32,
        DUO1_HARD = 64,
        DUO1_EXTREAM = 128,
        DUO2_EASY = 256,
        DUO2_NORMAL = 512,
        DUO2_HARD = 1024,
        DUO2_EXTREAM = 2048,
    }
    public static ModeDiff SOLO_DIFF_MODES = Enums.ModeDiff.SOLO_EASY | 
                                    Enums.ModeDiff.SOLO_NORMAL | 
                                    Enums.ModeDiff.SOLO_HARD | 
                                    Enums.ModeDiff.SOLO_EXTREAM;
    public static ModeDiff MULTI_DIFF_MODES = Enums.ModeDiff.DUO1_EASY | Enums.ModeDiff.DUO1_NORMAL | 
                                    Enums.ModeDiff.DUO1_HARD | Enums.ModeDiff.DUO1_EXTREAM |
                                    Enums.ModeDiff.DUO2_EASY | Enums.ModeDiff.DUO2_NORMAL |
                                    Enums.ModeDiff.DUO2_HARD | Enums.ModeDiff.DUO2_EXTREAM;

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
