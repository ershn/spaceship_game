// Simplify StateNode manipulation by defining the next alias in every file that uses them
using StateNode = Vertex<SuccessState, IState>;

public static class StateNodeExtensions
{
    public static IState State(this StateNode node) => node.Value;
}
