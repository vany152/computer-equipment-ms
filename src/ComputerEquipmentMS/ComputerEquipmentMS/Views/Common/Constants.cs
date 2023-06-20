namespace ComputerEquipmentMS.Views.Common;

public static class Constants
{
    /// <summary>
    /// Regex denies strings consisting only of spaces and space-framed strings
    /// </summary>
    public const string DenyOnlySpacesOrSpaceFramesStringsRegex = @"^(?!\s+$)\S+((.*)\S+)?$";
}