namespace TheAdventure.Game.Items;

/// <summary> Carried items end up here </summary>
public sealed class Inventory
{
    public const int Capacity = 8;

    private readonly List<Item> _items = new();

    public IReadOnlyList<Item> Items => _items;
    public int Count => _items.Count;
    public bool IsFull => _items.Count >= Capacity;

    public bool TryAdd(Item item)
    {
        if (IsFull)
        {
            return false;
        }

        _items.Add(item);
        return true;
    }

    public int CountOf<T>() where T : Item => _items.Count(i => i is T);

    public int FirstIndexOf<T>() where T : Item
    {
        for (var i = 0; i < _items.Count; i++)
        {
            if (_items[i] is T)
            {
                return i;
            }
        }

        return -1;
    }

    public Item? RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return null;
        }

        var item = _items[index];
        _items.RemoveAt(index);
        return item;
    }
}
