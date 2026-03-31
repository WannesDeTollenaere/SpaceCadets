public interface ICell
{
    bool IsActivated { get; }

    //uint floor { get; }

    void Activate();
}