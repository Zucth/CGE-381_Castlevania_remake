using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_controller : MonoBehaviour
{
    [SerializeField] replay_controller replay;
    private void Update()
    {
        if(replay == null)
        {
            replay = (replay_controller)FindObjectOfType(typeof(replay_controller));
        }
    }
    public void LS_Menu()
    {
        replay.IsIngame = false;
        SceneManager.LoadScene("Menu_scene");
    }
    public void LS_InGame()
    {
        replay.IsIngame = true;
        replay.Called_for_Wholerestart();
        SceneManager.LoadScene("SampleScene");
    }
    public void LS_same()
    {
        replay.Called_for_restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
