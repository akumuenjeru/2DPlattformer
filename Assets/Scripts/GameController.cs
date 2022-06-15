using UnityEngine;

public class GameController : MonoBehaviour
{
    private CollectingPerls _collectingPerls;
    private GameObject[] _perlTriggers;

    private void Start()
    {
        _perlTriggers = GameObject.FindGameObjectsWithTag("Perl");
        //handles Perl Count
        _collectingPerls = gameObject.GetComponent<CollectingPerls>();
        
        foreach (var perl in _perlTriggers)
        {
            if (perl == null) continue;
            PerlTriggerController perlScript = perl.GetComponent<PerlTriggerController>();
            if (perlScript == null) continue;
            perlScript.Setup(_collectingPerls);
        }
        _collectingPerls.UpdateText();
    }


}
