# AARPG Tutorial — C# Translation

A C# translation of [Michael Games' AARPG tutorial series](https://www.youtube.com/@MichaelGamesOfficial) built in Godot 4. The series starts [here](https://youtu.be/QPeycNt29tY?si=Viehkem9jw0uMWAk).

The original tutorial uses GDScript. This repo follows the same episodes but implements everything in idiomatic C#, with some structural differences noted below.

---

## Differences from the GDScript Tutorial

### General C# Idioms
These are general C# patterns that apply throughout rather than episode-specific decisions:
- File-scoped namespaces (`namespace Foo;` instead of wrapping the whole file in braces)
- State methods return `null` to mean "stay in current state" — GDScript returns `self`
- `GetValueOrDefault` used for dictionary lookups with a fallback value — GDScript handles missing keys implicitly with `null`
- GDScript's `@onready` has no direct C# equivalent. If you want to follow Michael's approach more closely, assign node references in `_Ready()` instead of using `[Export]`:
  ```csharp
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
      _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
  }
  ```

### Episode 1
- **Enums over strings:** Player state and animation direction are defined as proper C# `enum` types rather than raw strings
- **SetDirection:** Used `Vector2.Abs()` dominant-axis comparison instead of raw direction checks, which avoids the moonwalk bug Michael fixes in Episode 7

### Episode 2
- **Enum folder:** Enums moved to a dedicated `Player/Enum/` folder for organization

### Episode 3
- **`IState` interface + abstract `State` base class:** Michael's GDScript state machine uses duck typing — any node with the right methods works as a state. In C#, an `IState` interface defines the contract (`Enter`, `Exit`, `Process`, etc.) and the abstract `State` base class provides shared references (e.g. `Player`, `StateMachine`). This is the "program to an interface, not an implementation" principle — it makes the state machine more explicit, compiler-enforced, and easier to extend without breaking existing states.
- **`StateMachine.GetState<T>()`:** State transitions use a generic method rather than returning string names. This is type-safe and prevents typos — `StateMachine.GetState<StateAttack>()` instead of `return "Attack"`

### Episode 5
- **Signal subscriptions:** `AnimationFinished += EndAttack` / `-=` is used instead of GDScript's `connect`/`disconnect`. The `+=` pattern requires manual cleanup with `-=` (done in `Exit()`) — easy to forget compared to GDScript's more explicit `disconnect`. Note: `AnimationFinished` is a built-in Godot signal — no `[Signal]` delegate needed here.
- **Custom signals:** Follow the C# Godot convention — `[Signal]` on a delegate ending in `EventHandler`, emitted via `EmitSignal(SignalName.X)` rather than GDScript's `signal name(params)` / `name.emit()`. The `EventHandler` suffix is required — Godot strips it to derive the signal name, so `DamagedEventHandler` becomes the `Damaged` signal.

### Episode 6
- **Naming:** The shared nodes folder is named `Common/` instead of `GeneralNodes/`, and `PlayerInteractionsHost` is named `PlayerInteractionsManager`
- **Node references as exports:** Rather than using hardcoded `GetNode("../../Some/Path")` strings, node references are exposed as `[Export]` properties and assigned by dragging nodes into Inspector slots in the editor. This makes the code resilient to scene tree restructuring. See [Editor Wiring](#editor-wiring) below for the full mapping.
- **`InputActions.cs`:** Input action strings centralized in a static class instead of scattered string literals
- **Pattern matching:** Switch expressions and `is` type casts used where applicable (e.g. `area is HitBox hitBox` in HurtBox, direction mapping in `PlayerInteractionsManager`)
- **`async void Enter()`:** `StateAttack.Enter()` uses `async void` with `await ToSignal()` to delay hitbox activation. GDScript handles this inline with `yield`. `async void` is generally discouraged in C# but is the accepted pattern for Godot lifecycle methods that need to await

### Episode 7
- **Moonwalk bug:** Michael fixes this with angle/TAU math — not needed here since it was already handled in Episode 1 with `Vector2.Abs()`
- **HurtBox color:** The tutorial uses yellow for the HurtBox debug color — this repo uses green
- **HurtBox naming:** The tutorial renames the HurtBox to `AttackHurtBox` — this repo keeps it as `HurtBox`

### Episode 8
- **Globals folder:** Michael creates a `Globals/` folder with a `GlobalLevelManager.gd` script. This repo uses the existing `Common/` folder for shared/global scripts instead
- **Autoload access:** GDScript autoloads are accessible like static methods anywhere in the project. In C#, `GlobalLevelManager` exposes a static `Instance` property set in `_Ready()`, so it can be accessed as `GlobalLevelManager.Instance` without `GetNode` calls
- **Nullable return types:** `State.Process()`, `Physics()`, and `HandleInput()` are marked `State?` to explicitly declare that returning `null` ("stay in current state") is intentional
- **Collection expressions:** `List<State>` initialized with `[]` instead of `new List<State>()` — C# 12 collection expression syntax
- **Non-null pattern:** `FirstOrDefault() is { }` used instead of `is State` type check — more precise since it matches any non-null value rather than checking the type
- **Simplified type reference:** `HurtBox.cs` uses `HitBox` directly via a `using` statement instead of the fully qualified `HitBox.HitBox`
- **`using` aliases:** Used to shorten verbose Godot types, e.g. `using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>;` — `List<T>` is generally preferred in C#, but Godot signal parameters require Godot's own array types.

---

## Editor Wiring

Because node references use `[Export]` rather than hardcoded `GetNode` paths (added in commit `e88bbe8`), you need to assign them in the Godot editor after cloning. Open each scene and drag the listed nodes into the corresponding Inspector slots.

### `player.tscn` — Attack state node (`StateMachine > Attack`)
| Property                | Node to assign                                |
|-------------------------|-----------------------------------------------|
| Player Animation Player | `AnimationPlayer`                             |
| Attack Animation Player | `Sprite2D/AttackEffectSprite/AnimationPlayer` |
| Audio Stream Player 2D  | `Audio/AudioStreamPlayer2D`                   |
| Hurt Box                | `Interactions/HurtBox`                        |

### `player.tscn` — Interactions node (`Interactions`)
| Property | Node to assign       |
|----------|----------------------|
| Player   | `Player` (root node) |

### `Plant.tscn`
| Property | Node to assign |
|----------|----------------|
| Hit Box  | `HitBox`       |

---

## Commit History by Episode

> **Note:** This repo wasn't originally intended to be shared, so the first few commits cover multiple episodes. Going forward commits map to individual episodes.

| Commit    | Episodes | Description                                                                      |
|-----------|----------|----------------------------------------------------------------------------------|
| `4cf93fd` | 1–3      | Initial setup — player movement and state machine                                |
| `a070345` | —        | Refactor — moved shared state logic into base `State` class                      |
| `f7e64b1` | —        | Refactor — moved enums into dedicated `Player/Enum/` folder                      |
| `c3ea106` | 4–5      | Attack state, terrain tilemap, terrain collisions                                |
| `e88bbe8` | 6        | HitBox/HurtBox system, shrubs/plants, `PlayerInteractionsManager`, export wiring |
| `774a925` | 7        | Episode 7 cleanup — moonwalk fix not needed, export wiring                       |
| `eaa51c5` | 8        | Epidode 8 - Auto Camera Limits + small refactors                                 |

