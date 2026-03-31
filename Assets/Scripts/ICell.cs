public interface ICell
{
    enum State { Revealed, Hidden, Triggered }

    State CellState { get; }
    //uint floor { get; }

    void Activate();
}