# AI Usage Disclosure

## Tools used

- **Claude Opus 4.8**, used through **Claude Code** (Anthropic's agentic coding CLI).

## How it was used

- **Agentic coding** for the hardest parts of the project:
  the unsafe SDL2 interop, the rendering helpers, the procedural dungeon generator,
  the field-of-view algorithm, the generic grid container, and the asynchronous
  high-score persistence layer.
- **Rubber-ducking / design discussion** for the overall architecture (the screen
  state machine, the turn engine, how the pieces fit together).

The remaining gameplay code — the turn engine and combat (`GameWorld`), the screens,
the entity/item subclasses, the factories, the inventory, and the geometry in
`Position` — was written/driven by me, with AI used at most for small tweaks and
review. Those files are **not** marked as AI-generated.

## Fully AI-generated files

The following files are fully AI-generated and are wrapped in
`// AI-generated` … `// end AI-generated` markers:

- `Core/Grid.cs`
- `Rendering/SdlPlatform.cs`
- `Rendering/WorldView.cs`
- `Rendering/BlockFont.cs`
- `Game/FieldOfView.cs`
- `Game/Map/DungeonGenerator.cs`
- `Game/Entities/Monster.cs`
- `Persistence/HighScoreStore.cs`
- `Persistence/ScoreSerializerContext.cs`

## Fully AI-generated documentation

These Markdown files were also generated with AI (they contain no C# source, so they
do not count toward the C# authorship cap, but they are disclosed here for honesty):

- `README.md`
- `AI_USAGE.md`

## Authorship share

The fully AI-generated C# files total **765 lines** out of **2300** lines of C# in the
repository (**~33%**), or **~40%** if the skeleton files provided by the starter repo
(`KeyCodes.cs`, `MouseButton.cs`, `SdlContext.cs`) are excluded — below the 50% cap.

Including the two AI-generated Markdown docs (**117 lines**) in the totals gives
**882 / 2417 lines (~36%)** across all committed text — under 50%.
