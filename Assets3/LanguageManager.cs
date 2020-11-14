using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    private void Start()
    {
        print(LocalizationSettings.SelectedLocaleAsync.Result.name);
        print("l. " + LocalizationSettings.AvailableLocales.Locales.Count);
        foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
        {
            print(locale.name);
        }
    }
}
