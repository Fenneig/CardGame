using CardGame.Visual;
using UnityEngine;

public class HeroPowerButton : MonoBehaviour
{
    [SerializeField] private AreaPosition _owner;
    [SerializeField] private GameObject _front;
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _glow;

    private bool _highlighted = false;
    public bool Highlighted
    {
        get => _highlighted;
        set
        {
            _highlighted = value;
            _glow.SetActive(_highlighted);
        }
    }

    private bool _wasUsed = false;
    public bool WasUsed
    {
        get => _wasUsed;
        set
        {
            _wasUsed = value;
            if (!_wasUsed)
            {
                _front.SetActive(true);
                _back.SetActive(false);
            }
            else
            {
                _front.SetActive(false);
                _back.SetActive(true);
                Highlighted = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!WasUsed && Highlighted)
        {
            GlobalSettings.Instance.Players[_owner].UseHeroPower();
            WasUsed = !WasUsed;
        }
    }
}