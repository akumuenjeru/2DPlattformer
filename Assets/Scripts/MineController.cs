
using System.Collections;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public GameObject explodingMineEffect;
    private GameObject _explodingMineEffectClone;
    private GameController _gameController;

    public void Explode()
    {
        _explodingMineEffectClone = Instantiate(explodingMineEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(_explodingMineEffectClone, 0.5f);
    }


}