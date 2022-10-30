using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PanelBet : MonoBehaviour
{
    [SerializeField] private TMP_InputField _horseNumber;
    [SerializeField] private TMP_InputField _betValue;
    [SerializeField] private TextMeshProUGUI _walet;
    [SerializeField] private Button _betButton;
    [SerializeField] private UnityEvent  _betIsPlaced = new UnityEvent();
    private int _maxHorses;
    private int _targetHorse;
    private int _bet;

    public static UnityEvent HorseIsChosen = new UnityEvent();

    private void Awake() 
    {
        RacingController.ReachedFirstHorse.AddListener(TryGivePrize);
    }
    
    private void Start() 
    {
        _betButton.onClick.AddListener(delegate 
        {
            CheckRate();
        });

        _maxHorses = RacingController.MaxHorses();
    }

    private void CheckRate()
    {
        int walet = -1;
        int horse = -1;
        int betValue = -1;

        if (int.TryParse(_walet.text, out walet) && int.TryParse(_horseNumber.text, out horse) && int.TryParse(_betValue.text, out betValue))
        {
            walet = int.Parse(_walet.text);
            horse = int.Parse(_horseNumber.text);
            betValue = int.Parse(_betValue.text);
            _horseNumber.text = null;
            _betValue.text = null;
        }

        if (betValue > 0 && betValue <= walet && horse != -1 && horse <= _maxHorses)
        {
            walet -= betValue;
            _walet.text = walet.ToString();
            HorseIsChosen.Invoke();
            _betIsPlaced.Invoke();
            _bet = betValue;
            _targetHorse = horse;
        }
    }

    private void TryGivePrize(int tagetNumber)
    {
        if (tagetNumber == _targetHorse)
        {
            _walet.text = (int.Parse(_walet.text) + (_bet*9)).ToString();
        }
    }
}