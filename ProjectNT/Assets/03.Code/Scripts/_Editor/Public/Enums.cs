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

    public enum Details
    {
        SAVEPATHCHOICE,
        FILESAVEFAIL,
        NONEPROJECTNAME,
        NONEARTIST,
        NONEBPM,
        NONEBGM,
        NONETHUMBNAIL,
        SAVEWARNING,
        FILELOADFAIL,
        PATHSETERROR,
        SAVEFOLDEREXIST,
        LOADIMGFAIL,
        MAKEPROJECTCOMPLETE,
        CHANGEPROJECTINFOCOMPLETE,
        DELETEPROJECTCHECK
    }
}
