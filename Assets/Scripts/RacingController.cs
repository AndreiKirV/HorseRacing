using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RacingController : MonoBehaviour
{
    [SerializeField] private GameObject _targetHorse;
    [SerializeField] private int _valueHorses;
    [SerializeField] private GameObject _ribbon;
    [SerializeField] private TextMeshProUGUI _informationPanel;

    private int _finishingHorses = 0;
    private int _firstHorse = 0;
    private float _step = 0.35f;
    private float _minSpeed = 3.5f;
    private float _maxSpeed = 10f;
    private bool _isRunning = false;
    private Vector3 _startPosition;
    private Dictionary <GameObject, Horse> _horses = new Dictionary<GameObject, Horse>();

    public UnityEvent <int> ReachedHorse = new UnityEvent<int>();
    public static UnityEvent <int> ReachedFirstHorse = new UnityEvent<int>();
    public delegate int GiveInt();
    public static GiveInt MaxHorses;

    private void Awake() 
    {
        MaxHorses += GiveMaxHorses;
        PanelBet.HorseIsChosen.AddListener(SetSpeedHorses);
    }

    private void Start() 
    {
        Init();
    }
    
    private void Update() 
    {
        if (_isRunning)
        Running();
    }

    private void Init()
    {
        _horses.Add(_targetHorse.gameObject, new Horse(_targetHorse.gameObject, 1, Random.Range(_minSpeed, _maxSpeed)));
        _startPosition = _targetHorse.transform.position;

        for (int i = 1; i < _valueHorses; i++)
        {
            GameObject tempObject = Instantiate<GameObject>(_targetHorse, _targetHorse.transform.parent.transform);
            tempObject.transform.position = new Vector3(tempObject.transform.position.x, tempObject.transform.position.y - i*_step, tempObject.transform.position.z);
            tempObject.name = _targetHorse.name + (i+1);
            tempObject.GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();

            tempObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
            
            _horses.Add(tempObject, new Horse(_targetHorse.gameObject, i+1, Random.Range(_minSpeed, _maxSpeed)));
        }

        ReachedHorse.AddListener(SetTextInformationPanel);
    }

    private void Running()
    {
        GameObject tempObject = null;

        foreach (var item in _horses)
        {
            if (item.Key.transform.position.x <= _ribbon.transform.position.x)
            {
                item.Key.transform.position = new Vector3(item.Key.transform.position.x + item.Value.Speed * Time.deltaTime, item.Key.transform.position.y, item.Key.transform.position.z);
            }
            else if (item.Key.transform.position.x >= _ribbon.transform.position.x && item.Value.Speed > 0)
            {
                ChangeRunningAnimator(item.Key.GetComponent<Animator>());
                tempObject = item.Key;

                if (ReachedHorse != null)
                {
                    ReachedHorse.Invoke(item.Value.Number);
                }
            }
        }

        if (tempObject != null)
        {
            if (_firstHorse == 0)
            {
                _firstHorse = _horses[tempObject].Number;
                ReachedFirstHorse.Invoke(_firstHorse);
            }

            _horses[tempObject].SlowDown();
            tempObject = null;
        }

        if (_finishingHorses >= _valueHorses)
        {
            _informationPanel.text = $"horse number {_firstHorse}, came 1.";
            _finishingHorses = 0;
            _isRunning = !_isRunning;
            _firstHorse = 0;

            foreach (var item in _horses)
            {
                item.Key.transform.position = new Vector3(_startPosition.x, item.Key.transform.position.y, item.Key.transform.position.z);
            }
        }
    }

    private void ChangeRunningAnimator(Animator animator)
    {
        animator.enabled = !_isRunning;
    }

    private void SetTextInformationPanel(int number)
    {
        _informationPanel.text = $"horse number {number}, came {_finishingHorses + 1}.";
        _finishingHorses ++;
    }

    public void StartRunning()
    {
        foreach (var item in _horses)
        {
            ChangeRunningAnimator(item.Key.GetComponent<Animator>());
        }

        _isRunning = !_isRunning;
    }

    public int GiveMaxHorses()
    {
        return _valueHorses;
    }

    public void SetSpeedHorses()
    {
        foreach (var item in _horses)
        {
            item.Value.SetSpeed(Random.Range(_minSpeed, _maxSpeed));
        }
    }
}