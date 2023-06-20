using NodaTime;

namespace ComputerEquipmentMS.Models.Auxiliary;

public record TimeInterval(Instant From, Instant To);