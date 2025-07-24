using UnityEngine;

public class OpenURLButton : MonoBehaviour
{
    public string url = "https://store.steampowered.com/app/3297800/Plushes/";

    public void OpenLink()
    {
        Application.OpenURL(url);
    }
}