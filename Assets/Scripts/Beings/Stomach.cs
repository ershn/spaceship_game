using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Stomach : MonoBehaviour
{
    ItemGridIndex _itemGrid;
    JobScheduler _jobScheduler;

    Death _death;
    JobExecutor _jobExecutor;
    FoodConsumption _foodConsumption;

    public ulong MaxStoredCalories = 7500.KiloCalories();
    public ulong InitialStoredCalories = 5000.KiloCalories();
    public ulong CaloriesConsumedPerCycle = 2500.KiloCalories();
    public ulong FoodConsumptionCaloriesThreshold = 4000.KiloCalories();

    ulong _caloriesAfterLastMeal;
    float _timeOfLastMeal;

    void Awake()
    {
        _itemGrid = GetComponentInParent<GridIndexes>().ItemGrid;
        _jobScheduler = GetComponentInParent<WorldInternalIO>().JobScheduler;

        _death = GetComponent<Death>();
        _death.OnDeath.AddListener(OnDeath);
        _jobExecutor = GetComponent<JobExecutor>();
        _foodConsumption = GetComponent<FoodConsumption>();
    }

    void Start()
    {
        _caloriesAfterLastMeal = InitialStoredCalories;
        _timeOfLastMeal = Time.time;
    }

    void OnDeath()
    {
        enabled = false;
    }

    void Update()
    {
        var currentCalories = CurrentCalories();

        if (currentCalories == 0)
            _death.Die();
        else if (currentCalories < FoodConsumptionCaloriesThreshold)
            TryConsumeFood();
    }

    float CyclesSinceLastMeal() => (Time.time - _timeOfLastMeal) / Clock.CycleLength;

    ulong CaloriesConsumedSinceLastMeal() =>
        (ulong)(CaloriesConsumedPerCycle * CyclesSinceLastMeal());

    ulong CurrentCalories()
    {
        var caloriesConsumed = CaloriesConsumedSinceLastMeal();
        return _caloriesAfterLastMeal > caloriesConsumed
            ? _caloriesAfterLastMeal - caloriesConsumed
            : 0;
    }

    public void StoreCalories(ulong calories)
    {
        _caloriesAfterLastMeal = CurrentCalories() + calories;
        _timeOfLastMeal = Time.time;
    }

    bool _isConsumingFood;
    ulong? _nextCaloriesThreshold;

    async void TryConsumeFood()
    {
        if (_isConsumingFood)
            return;

        var currentCalories = CurrentCalories();
        if (_nextCaloriesThreshold != null && currentCalories > _nextCaloriesThreshold)
            return;

        Debug.Log(
            $"Try to consume food (stored: {currentCalories / 1.KiloCalorie()} kcal)",
            gameObject
        );

        _isConsumingFood = true;
        if (await ConsumeFood(MaxStoredCalories - currentCalories))
            _nextCaloriesThreshold = null;
        else
            _nextCaloriesThreshold = NextCaloriesThreshold(currentCalories);
        _isConsumingFood = false;
    }

    ulong NextCaloriesThreshold(ulong currentCalories)
    {
        var multiplier =
            currentCalories < 1000.KiloCalories() ? 100.KiloCalories() : 500.KiloCalories();
        return currentCalories / multiplier * multiplier;
    }

    async Task<bool> ConsumeFood(ulong calories)
    {
        var foodItems = _itemGrid.Filter<FoodItemDef>().CumulateCalories(calories);
        if (!foodItems.Any())
            return false;

        try
        {
            var markedCalories = foodItems.Sum(item => item.markedCalories);
            Debug.Log($"Consume food: {markedCalories / 1.KiloCalorie()} kcal", gameObject);

            var job = new EatFoodJob(foodItems.CaloriesToMass(), _foodConsumption);
            await _jobScheduler.Execute(job, _jobExecutor, CancellationToken.None);

            Debug.Log($"Food consumption completed", gameObject);
            return true;
        }
        catch (TaskCanceledException)
        {
            Debug.Log($"Food consumption canceled", gameObject);
            return false;
        }
    }
}
