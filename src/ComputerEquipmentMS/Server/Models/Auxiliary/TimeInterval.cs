using NodaTime;

namespace Server.Models.Auxiliary;

public record TimeInterval(Instant From, Instant To);