using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public enum UI_STATE
    {
        NONE,
        LEVEL_SELECT,
        HOWTOPLAY,
        GLOSSARY,
        GLOSSARY_TUTORIAL,
        SETTINGS,
        PAUSED,
        WIN,
        LOSE,
        BACK
    }

    public enum VOLUME_SLIDER
    {
        MASTERVOLUME,
        MUSICVOLUME,
        SFXVOLUME
    }

    public enum COLLECTABLE_TYPE
    { 
        COIN,
        KEY,
        BOSS_KEY,
        HEALTH,
        MAGIC
    }

    public enum SEGMENT_TYPE
    {
        HEALTH,
        MAGIC,
        ABILITY_TIMER
    }

    public enum AFFINITY_TYPE
    {
        STANDARD,
        FIRE,
        ICE,
        WIND
    }

}

