using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public static void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
