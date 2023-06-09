﻿using NodaTime;

namespace ComputerEquipmentMS.Views.Common;

public static class Converter
{
    public static LocalDate LocalDateFromInstant(Instant i)
    {
        var dateTime = i.ToDateTimeUtc();
        var localDate = LocalDate.FromDateTime(dateTime);

        return localDate;
    }
}