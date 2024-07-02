using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;

public class LvlButtonWidget : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private Button button;
    [SerializeField] private Image lockK;

    private PreloaderLevelService p;

    [Inject]
    private void Construct(PreloaderLevelService preloader)
    {
        p = preloader;
    }
    

    public void OnButtonClick()
    {
        p.SetCurrentLevel(level);
        SceneManager.LoadScene("Gameplay");
    }

    public void UpdateWidgetState(bool state)
    {
        if (state)
        {
            button.interactable = true;
            lockK.enabled = false;
        }
        
        if (!state)
        {
            button.interactable = false;
            lockK.enabled = true;
        }
    }
}