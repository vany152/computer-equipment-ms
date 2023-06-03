using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities.Auxiliary;

public record TimeIntervalEntity(Instant From, Instant To);