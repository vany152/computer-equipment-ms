namespace ComputerEquipmentMS.Test.RepositoriesTests;

public static class Util
{
    public static bool DictionariesEqual<TKey, TValue>(IDictionary<TKey, TValue>? first, IDictionary<TKey, TValue>? second)
    {
        if (first is null && second is null) 
            return true;
        if (first is null && second is not null || first is not null && second is null)
            return false;

        var countsEqual = first!.Count == second!.Count;
        var differenceEmpty = !first.Except(second).Any();
        return countsEqual && differenceEmpty; 
    }
}