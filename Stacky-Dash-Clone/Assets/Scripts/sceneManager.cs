using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class sceneManager : MonoBehaviour
{
    [SerializeField] private Text levelEndText;
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private Text scoreBoard;

    public static sceneManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }
    public void handleLevelEnd(float coefficient, float totalCollected)
    {

        levelEndText.text = "Congrats for having X" + coefficient.ToString();


        scoreBoard.text = (totalCollected * coefficient).ToString();


        levelEndPanel.SetActive(true);

        Invoke("load_next_scene", 1f);
    }

    private void load_next_scene() //calling with invoke. That's why It says 0 
    {
        Destroy(this);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    public void updateScore(int totalCollected)
    {
        
            scoreBoard.text = totalCollected.ToString();
        
    }

    


}
