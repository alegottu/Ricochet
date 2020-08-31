using UnityEngine;

public class Survival : MonoBehaviour, IHasInfo
{
    [SerializeField] InfoText text = null;

    public void getInfo()
    {
        UIManager.Instance.displayInfo(text.info);
    }

    public void getMode()
    {
        GameManager.Instance.setMode(GameManager.GameMode.SURVIVAL);
    }
}
