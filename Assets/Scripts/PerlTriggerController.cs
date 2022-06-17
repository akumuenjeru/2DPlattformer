using UnityEngine;

public class PerlTriggerController : MonoBehaviour
{
    private CollectingPerls _collectingPerls;
    
    public void Setup(CollectingPerls collectingPerls)
    {
        _collectingPerls = collectingPerls;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name != "Player") return;
        if (_collectingPerls == null) return;
        _collectingPerls.SetPerlCount(_collectingPerls.GetPerlCount()+1);
        _collectingPerls.UpdateText();
        gameObject.SetActive(false);
    }
}