using TMPro;
using UnityEngine;

public class CollectingPerls : MonoBehaviour
{
    private int _perlCount;
    public TextMeshProUGUI perlCounterText;

    public void SetPerlCount(int newAmount)
    {
        _perlCount = newAmount;
    }

    public int GetPerlCount()
    {
        return _perlCount;
    }

    public void UpdateText()
    {
        perlCounterText.text = "Collected perls: " + GetPerlCount();
    }
}