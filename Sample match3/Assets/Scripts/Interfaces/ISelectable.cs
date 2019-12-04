namespace MatchThree.Interfaces
{
    public interface ISelectable
    {
        int Row { get; }
        int Column { get; }

        void Select();
        void Deselect();
    }
}
