using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public Text theAmount;
    public Text TotalWin;

    public Sprite[] winSprites;
    public Image[] slots;

    private int amount;
    private int totalWin;
    private int[] massIndexSlots;

    // Start is called before the first frame update
    void Start()
    {
        amount = 1000;
        totalWin = 0;
        
        UpdateText();
        massIndexSlots = new int[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].sprite = winSprites[Random.Range(0, winSprites.Length)];
        }
    }

    void UpdateText()
    {
        theAmount.text = $"На счету : {amount}$";
        TotalWin.text = $"Общий выигрыш : {totalWin}$";
    }

    public void BanditRun()
    {
        amount -= 100;
        UpdateText();
        
        for (int i = 0; i < slots.Length; i++)
        {
            int index = Random.Range(0, winSprites.Length);
            slots[i].sprite = winSprites[index];
            massIndexSlots[i] = index + 1;
        }

        CheckWin.Joker = winSprites.Length;
        CheckWin.UpdateMatrix(massIndexSlots);
        CheckWinGame();
    }

    void CheckWinGame()
    {
        int prize = CheckWin.GetWinAmount();
        Debug.Log($"You win : {prize}$");

        amount += prize;
        totalWin += prize;
        UpdateText();
    }
}
