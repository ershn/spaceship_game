using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Death))]
[RequireComponent(typeof(TaskExecutor))]
public class Stomach : MonoBehaviour
{
    public TaskScheduler TaskScheduler;

    ItemGridIndex _itemGrid;

    Death _death;
    TaskExecutor _taskExecutor;
    FoodConsumption _foodConsumption;

    public ulong MaxStoredCalories = 7500.KiloCalories();
    public ulong InitialStoredCalories = 5000.KiloCalories();
    public ulong CaloriesConsumedPerCycle = 2500.KiloCalories();
    public ulong FoodConsumptionCaloriesThreshold = 4000.KiloCalories();

    ulong _caloriesAfterLastMeal;
    float _timeOfLastMeal;

    void Awake()
    {
        _itemGrid = transform.root.GetComponent<GridIndexes>().ItemGrid;

        _death = GetComponent<Death>();
        _death.OnDeath.AddListener(OnDeath);
        _taskExecutor = GetComponent<TaskExecutor>();
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
            TryOrderFoodConsumption();
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

    ITask _foodConsumptionTask;
    ulong? _nextOrderTryCaloriesThreshold;

    void TryOrderFoodConsumption()
    {
        if (_foodConsumptionTask != null)
            return;

        var currentCalories = CurrentCalories();
        if (
            _nextOrderTryCaloriesThreshold != null
            && currentCalories >= _nextOrderTryCaloriesThreshold
        )
            return;

        Debug.Log($"Try to consume food (stored: {currentCalories / 1.KiloCalorie()} kcal)");
        _nextOrderTryCaloriesThreshold = null;
        if (OrderFoodConsumption(MaxStoredCalories - currentCalories))
            return;

        _nextOrderTryCaloriesThreshold = NextOrderTryCaloriesThreshold(currentCalories);
    }

    ulong NextOrderTryCaloriesThreshold(ulong currentCalories)
    {
        var multiplier =
            currentCalories < 1000.KiloCalories() ? 100.KiloCalories() : 500.KiloCalories();

        return currentCalories % multiplier == 0
            ? (currentCalories / multiplier - 1) * multiplier
            : currentCalories / multiplier * multiplier;
    }

    bool OrderFoodConsumption(ulong calories)
    {
        var foodItems = _itemGrid.Filter<FoodItemDef>().CumulateCalories(calories);

        if (!foodItems.Any())
            return false;

        var markedCalories = foodItems.Aggregate(0ul, (acc, item) => acc + item.markedCalories);
        Debug.Log($"Consume food: {markedCalories / 1.KiloCalorie()} kcal");

        _foodConsumptionTask = TaskCreator.EatFood(foodItems.CaloriesToMass(), _foodConsumption);
        _foodConsumptionTask.Then(success =>
        {
            Debug.Log($"Food consumption {(success ? "completed" : "canceled")}");
            _foodConsumptionTask = null;
        });
        TaskScheduler.QueueTask(_foodConsumptionTask, _taskExecutor);

        return true;
    }

    #region debug log

    float _timeOfLastCaloriesLog;

    void LogCurrentCalories()
    {
        if (Time.time - _timeOfLastCaloriesLog >= 1f)
        {
            _timeOfLastCaloriesLog = Time.time;
            Debug.Log($"Current calories: {CurrentCalories() / 1.KiloCalorie()} kcal");
        }
    }

    #endregion
}
