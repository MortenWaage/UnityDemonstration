using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IRestart
{  
    void Start()
    {
        GameController.Instance.ActiveGameObjects.Add(this.gameObject);
    }
    public void Respawn()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
    }
}
