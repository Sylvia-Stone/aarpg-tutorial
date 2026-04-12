# AARPG Tutorial — C# Translation

A C# translation of [Michael Games' AARPG tutorial series](https://www.youtube.com/@MichaelGamesOfficial) built in Godot 4. The series starts [here](https://youtu.be/QPeycNt29tY?si=Viehkem9jw0uMWAk).

The original tutorial uses GDScript. This repo follows the same episodes but implements everything in idiomatic C#, with some structural differences noted below.

---

## Differences from the GDScript Tutorial

### Node References as Exports
Rather than using hardcoded `GetNode("../../Some/Path")` strings, node references are exposed as `[Export]` properties and assigned by dragging nodes into Inspector slots in the editor. This makes the code resilient to scene tree restructuring.

See [Editor Wiring](#editor-wiring) below for the full mapping.

### C# Idioms
- Enums are defined as proper C# `enum` types in a dedicated `Enum/` folder
- Input action strings are centralized in `InputActions.cs` as a static class rather than scattered string literals
- `IState` interface defined explicitly alongside the abstract `State` base class
- Pattern matching used where applicable (e.g. switch expressions, `is` type casts)
- Signals follow the C# Godot convention: `[Signal]` on a delegate ending in `EventHandler`, emitted via `EmitSignal(SignalName.X)`
- `GD.Print()` used instead of `Console.WriteLine()`

### Moonwalk Bug
The tutorial addresses a moonwalk animation bug in Episode 7 using angle/TAU math. This repo avoids the bug entirely with a `Vector2.Abs()` dominant-axis comparison in `SetDirection()`, so that fix was not needed.

### Naming
Some nodes and scripts are named more in line with C# conventions. For example, the GDScript `PlayerInteractions` becomes `PlayerInteractionsManager.cs`.

---

## Editor Wiring

Because node references use `[Export]`, you need to assign them in the Godot editor after cloning. Open each scene and drag the listed nodes into the corresponding Inspector slots.

### `player.tscn` — Attack state node (`StateMachine > Attack`)
| Property | Node to assign |
|---|---|
| Player Animation Player | `AnimationPlayer` |
| Attack Animation Player | `Sprite2D/AttackEffectSprite/AnimationPlayer` |
| Audio Stream Player 2D | `Audio/AudioStreamPlayer2D` |
| Hurt Box | `Interactions/HurtBox` |

### `player.tscn` — Interactions node (`Interactions`)
| Property | Node to assign |
|---|---|
| Player | `Player` (root node) |

### `Plant.tscn`
| Property | Node to assign |
|---|---|
| Hit Box | `HitBox` |

---

## Commit History by Episode

> **Note:** This repo wasn't originally intended to be shared, so the first few commits cover multiple episodes. Going forward commits map to individual episodes.

| Commit | Episodes | Description |
|---|---|---|
| `4cf93fd` | 1–3 | Initial setup — player movement and state machine |
| `a070345` | — | Refactor — moved shared state logic into base `State` class |
| `f7e64b1` | — | Refactor — moved enums into dedicated `Player/Enum/` folder |
| `c3ea106` | 4–5 | Attack state, terrain tilemap, terrain collisions |
| `e88bbe8` | 6 | HitBox/HurtBox system, shrubs/plants, `PlayerInteractionsManager` |
| `774a925` | 7 | Episode 7 cleanup — moonwalk fix not needed, export wiring |
| `—` | — | Added README |

