public static class AmountString
{
    public static string Format(AmountType type, ulong amount) =>
        type switch
        {
            AmountType.Mass => MassString.Format(amount),
            AmountType.Count => amount.ToString(),
            _ => throw new System.NotImplementedException(),
        };

    public static bool TryParse(AmountType type, string str, out ulong amount) =>
        type switch
        {
            AmountType.Mass => MassString.TryParse(str, out amount),
            AmountType.Count => ulong.TryParse(str, out amount),
            _ => throw new System.NotImplementedException(),
        };
}
