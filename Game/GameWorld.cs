using TheAdventure.Core;
using TheAdventure.Game.Entities;
using TheAdventure.Game.Items;
using TheAdventure.Game.Map;

namespace TheAdventure.Game;

/// <summary> The live state of one run: the current floor, the hero, the monsters, and the turn engine </summary>
public sealed class GameWorld
{
    public const int MaxDepth = 5;

    private readonly Random _rng;
    private readonly int _width;
    private readonly int _height;

    public GameWorld(int width, int height, int? seed = null)
    {
        _rng = seed is null ? new Random() : new Random(seed.Value);
        _width = width;
        _height = height;
        Depth = 1;

        var level = new DungeonGenerator(_rng).Generate(width, height);
        Map = level.Map;
        Player = new Player(level.PlayerStart);
        SpawnMonsters(level);
        SpawnItems(level);
        RecomputeFieldOfView();
        Log.Add("YOU ENTER THE UNDERWORLD.");
    }

    public DungeonMap Map { get; private set; }
    public Player Player { get; }
    public List<Monster> Monsters { get; } = new();

    // Items lying on the current floor
    public Dictionary<Position, Item> FloorItems { get; } = new();
    public MessageLog Log { get; } = new();
    public int Depth { get; private set; }
    public int Score { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.Playing;

    // Move (or bump-attack) in a direction
    public void MovePlayer(Direction direction)
    {
        if (Status != GameStatus.Playing)
        {
            return;
        }

        var target = Player.Position.Step(direction);

        if (MonsterAt(target) is { } monster)
        {
            ResolveAttack(Player, monster);
            EndPlayerTurn();
        }
        else if (Map.IsWalkable(target))
        {
            Player.Position = target;
            TryPickUp();
            EndPlayerTurn();
        }
        // Walking into a wall does nothing and does not cost a turn
    }

    // Use carried health potion (costs a turn)
    public void UseHealthPotion()
    {
        if (Status != GameStatus.Playing)
        {
            return;
        }

        var index = Player.Inventory.FirstIndexOf<HealthPotion>();
        if (index < 0)
        {
            Log.Add("YOU HAVE NO POTIONS.");
            return;
        }

        Player.Inventory.RemoveAt(index)!.Apply(this);
        EndPlayerTurn();
    }

    // Use the stairs under the hero to go deeper
    public void Descend()
    {
        if (Status != GameStatus.Playing)
        {
            return;
        }

        if (Map.Tiles[Player.Position] != TileType.StairsDown)
        {
            Log.Add("THERE ARE NO STAIRS HERE.");
            return;
        }

        if (Depth >= MaxDepth)
        {
            Status = GameStatus.Won;
            Log.Add("YOU CLIMB INTO THE LIGHT. YOU ESCAPED!");
            return;
        }

        Depth++;
        var level = new DungeonGenerator(_rng).Generate(_width, _height);
        Map = level.Map;
        Player.Position = level.PlayerStart;
        Monsters.Clear();
        FloorItems.Clear();
        SpawnMonsters(level);
        SpawnItems(level);
        RecomputeFieldOfView();
        Log.Add($"YOU DESCEND TO FLOOR {Depth}.");
    }

    // Resolves a single attack, applies damage, logs it, and handles death
    public void ResolveAttack(Actor attacker, Actor defender)
    {
        var damage = Math.Max(1, attacker.AttackPower - defender.Defense + _rng.Next(-1, 2));
        defender.TakeDamage(damage);
        Log.Add($"{attacker.Name} HITS {defender.Name} FOR {damage}.");

        if (defender.IsAlive)
        {
            return;
        }

        switch (defender)
        {
            case Monster slain:
                Score += slain.Bounty;
                Log.Add($"{slain.Name} IS SLAIN. +{slain.Bounty}");
                break;
            case Entities.Player:
                Status = GameStatus.Lost;
                Log.Add("YOU HAVE FALLEN.");
                break;
        }
    }

    // Collect gold
    public void CollectGold(int amount)
    {
        Player.AddGold(amount);
        Score += amount;
    }

    private void TryPickUp()
    {
        if (!FloorItems.Remove(Player.Position, out var item))
        {
            return;
        }

        if (item.ConsumeOnPickup)
        {
            item.Apply(this);
        }
        else if (Player.Inventory.TryAdd(item))
        {
            Log.Add($"YOU PICK UP {item.Name}.");
        }
        else
        {
            FloorItems[Player.Position] = item;
            Log.Add($"YOUR PACK IS FULL.");
        }
    }

    public Monster? MonsterAt(Position position) =>
        Monsters.FirstOrDefault(m => m.IsAlive && m.Position == position);

    public bool CanMonsterEnter(Position position) =>
        Map.IsWalkable(position) && position != Player.Position && MonsterAt(position) is null;

    private void EndPlayerTurn()
    {
        // Iterate a snapshot: a monster could be removed (or the player could die) mid-turn
        foreach (var monster in Monsters.ToList())
        {
            if (Status != GameStatus.Playing)
            {
                break;
            }

            if (monster.IsAlive)
            {
                monster.TakeTurn(this);
            }
        }

        Monsters.RemoveAll(m => !m.IsAlive);
        RecomputeFieldOfView();

        if (!Player.IsAlive)
        {
            Status = GameStatus.Lost;
        }
    }

    private void SpawnMonsters(GeneratedLevel level)
    {
        var occupied = new HashSet<Position> { Player.Position };

        // Skip the first room
        foreach (var room in level.Rooms.Skip(1))
        {
            var spawnCount = _rng.Next(0, 3);
            var freeCells = room.InteriorCells()
                .Where(c => Map.IsWalkable(c) && Map.Tiles[c] != TileType.StairsDown && !occupied.Contains(c))
                .ToList();

            for (var i = 0; i < spawnCount && freeCells.Count > 0; i++)
            {
                var index = _rng.Next(freeCells.Count);
                var position = freeCells[index];
                freeCells.RemoveAt(index);
                occupied.Add(position);
                Monsters.Add(MonsterFactory.CreateForDepth(Depth, position, _rng));
            }
        }
    }

    private void SpawnItems(GeneratedLevel level)
    {
        foreach (var room in level.Rooms)
        {
            var drops = _rng.Next(0, 2) + 1;
            for (var i = 0; i < drops; i++)
            {
                if (ItemFactory.Roll(Depth, _rng) is not { } item)
                {
                    continue;
                }

                var freeCells = room.InteriorCells()
                    .Where(c => Map.IsWalkable(c)
                                && Map.Tiles[c] != TileType.StairsDown
                                && c != Player.Position
                                && MonsterAt(c) is null
                                && !FloorItems.ContainsKey(c))
                    .ToList();

                if (freeCells.Count == 0)
                {
                    break;
                }

                FloorItems[freeCells[_rng.Next(freeCells.Count)]] = item;
            }
        }
    }

    private void RecomputeFieldOfView() => FieldOfView.Compute(Map, Player.Position, Player.SightRadius);
}
