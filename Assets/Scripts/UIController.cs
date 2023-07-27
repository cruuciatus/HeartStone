using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TextMeshProUGUI PlayerMana, EnemyMana;
    public TextMeshProUGUI PlayerHP, EnemyHP;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public TextMeshProUGUI TurnTime;
    public Button EndTurnBtn;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(this);

    }
    public void StartGame()
    {
        EndTurnBtn.interactable = true;
        ResultGO.SetActive(false);
        UpdateHPAndMana(); 
    }
    public void UpdateHPAndMana()
    {
        PlayerMana.text = GameManager.Instance.CurrentGame.Player.Mana.ToString();
        EnemyMana.text = GameManager.Instance.CurrentGame.Enemy.Mana.ToString();
        PlayerHP.text = GameManager.Instance.CurrentGame.Player.HP.ToString();
        EnemyHP.text = GameManager.Instance.CurrentGame.Enemy.HP.ToString();
    }
    public void ShowResult()
    {
        ResultGO.SetActive(true);
        if (GameManager.Instance.CurrentGame.Enemy.HP == 0)
        {
            ResultTxt.text = "YOU WIN";
        }else
        {
            ResultTxt.text = "YOU LOST";
        }
    }
    public void UpdateTurnTime(int time)
    {
        TurnTime.text = time.ToString();
    }
    public void DisableTurnBtn()
    {
        EndTurnBtn.interactable = GameManager.Instance.IsPlayerTurn;
    }
}
