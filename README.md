# Little Hades

A small top-down, turn-based **rogue-lite** built on the `TheAdventure` skeleton
(C# / .NET 10, rendered directly with SDL2 via Silk.NET).
Inspired by *Hades*: you are a shade trying to claw your way out of the Underworld.
Descend through five procedurally generated floors, fight enemies, grab gold and
relics, and reach the final stairs alive.

## Build & run

Requires the **.NET 10 SDK**. From the repository root:

```sh
dotnet run
```

That restores packages (including the native SDL2 libraries), builds, and launches
the game window. The project keeps `<Nullable>enable</Nullable>` and
`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`, so it builds warning-free from
a clean clone on Windows.

## How to play

| Keys | Action |
|------|--------|
| Arrow keys / `W` `A` `S` `D` | Move one tile or attack by walking into a monster |
| `Q` | Drink a health potion |
| `Enter` / `.` | Descend the stairs when standing on them |
| `Esc` | Return to the main menu |

On the title screen use the arrows to pick **Play**, **High Scores**, or **Quit**.

### Goal

- **Win:** reach and descend the stairs on floor 5 to escape.
- **Lose:** your health reaches 0.

### Things you'll find

- `@` — you. `S` Shade, `K` Skeleton, `W` Wraith (it acts twice per turn).
- `!` Health Potion (carried, drink with `Q`), `*` Gold, `/` Strength Shard (+attack),
  `+` Vitality Heart (+max health). Gold and relics are collected by stepping on them.

Your **score** (monster bounties + gold) is saved to a high-score table after every
run, at `%APPDATA%\LittleHades\highscores.json`.

## Project structure

```
Core/         Position (grid math) and the generic Grid<T> container
Rendering/    SDL platform wrapper (IDisposable), Color, the block font, the world view
Input/        Per-frame keyboard state
Game/         Turn engine and rules
  Map/        Tiles, rooms, the procedural dungeon generator, field of view
  Entities/   Actor base class, Player, Monster + Shade/Skeleton/Wraith
  Items/      IItem, Item base, potions/gold/relics, the inventory
Persistence/  Async JSON high-score store + SaveGameException
Screens/      IScreen state machine: title menu, play, game-over, high scores
GameApp.cs    The input -> update -> render game loop
Program.cs    Entry point
```

## Code highlights

The codebase leans on idiomatic C# features:

- **Generics** — `Grid<T>` backs the tile map and the visibility layers; `Inventory`
  exposes `CountOf<T>()` / `FirstIndexOf<T>()`.
- **Interfaces** — `IScreen` drives the screen state machine; `IItem` abstracts items.
- **Inheritance & polymorphism** — `Actor → Player`/`Monster`, with concrete
  `Shade`/`Skeleton`/`Wraith`; the Wraith overrides `TakeTurn` to act twice.
- **LINQ** — monster AI pathing, room/spawn selection, and high-score sorting.
- **Pattern matching** — input mapping, tile/combat resolution, item rolls by depth.
- **async / await** — the high-score table is loaded and saved asynchronously.
- **IDisposable** — `SdlPlatform` owns and releases all native SDL resources.
- **Custom exceptions** — `SaveGameException` wraps high-score I/O failures.
- **Special Thanks** — Maths used for distances was inspired from https://codereview.stackexchange.com/questions/120933/calculating-distance-with-euclidean-manhattan-and-chebyshev-in-c