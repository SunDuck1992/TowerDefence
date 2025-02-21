using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class TestResetSave : MonoBehaviour
{
    public void ResetSave()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
