using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void Click()
    {
        UIManager.Instance.playMode(this.gameObject);
    }
}
