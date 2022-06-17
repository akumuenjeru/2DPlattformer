using UnityEngine;

public class MineController : MonoBehaviour
{
    public GameObject explodingMineEffect;
    private GameObject _explodingMineEffectClone;

    public void Explode(GameObject triggeredMine)
    {
        Debug.Log("Triggered Mine " + triggeredMine.name);
        _explodingMineEffectClone = Instantiate(explodingMineEffect, triggeredMine.transform.position, Quaternion.identity);
        Destroy(triggeredMine);
    }

    private void OnDestroy()
    {
        Destroy(_explodingMineEffectClone, 0.5f);
    }
}