using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int _level = 0;
    public int Level => _level;

    [SerializeField] private Stat _blowerForce;
    public Stat BlowerForce => _blowerForce;

    public void AddLevel()
    {
        _level++;
        GameEventManager.Instance.playerEvents.PlayerLevelChange(_level);
    }
    public void SetLevel(int lvl) => _level = lvl;
}
