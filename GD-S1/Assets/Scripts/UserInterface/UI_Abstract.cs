using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Abstract : MonoBehaviour
{
    public Action<SCENE_TYPE> LoadSceneOnButtonClicked;
    public Action<int> LoadLevelByIndex;
    public Action<UI_SCREENS> LoadUIOnButtonClicked;
    public Action onBackButton;
}
