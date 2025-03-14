using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public bool isUIOpen = false; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenUI()
    {
        isUIOpen = true;
    }

    public void CloseUI()
    {
        isUIOpen = false;
    }
}
