namespace Server.Models;


public interface IIdentifiable<T>
    where T : struct
{
    T Id { get; set; }
}