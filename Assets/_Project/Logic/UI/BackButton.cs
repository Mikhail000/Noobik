using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void OnBack()
    {
        SceneManager.LoadScene("Menu");
    }
}
