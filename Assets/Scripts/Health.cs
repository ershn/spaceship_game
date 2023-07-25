using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent OnZeroHealth;
    public UnityEvent OnMaxHealth;

    IHealthHolderConf _conf;

    public int MaxHealthPoints => _conf.MaxHealthPoints;
    public int HealthPoints { get; private set; } = 1;

    void Awake()
    {
        _conf = GetComponent<IHealthHolderConf>();
    }

    public void AddHealth(int points)
    {
        HealthPoints = Mathf.Clamp(HealthPoints + points, 0, MaxHealthPoints);

        NotifyHealthChange();
    }

    public void SetTotalHealth(float total)
    {
        var newHealthPoints = Mathf.FloorToInt(MaxHealthPoints * total);

        if (newHealthPoints != HealthPoints)
        {
            HealthPoints = newHealthPoints;

            NotifyHealthChange();
        }
    }

    void NotifyHealthChange()
    {
        if (HealthPoints == 0)
            OnZeroHealth.Invoke();
        else if (HealthPoints == MaxHealthPoints)
            OnMaxHealth.Invoke();
    }
}
