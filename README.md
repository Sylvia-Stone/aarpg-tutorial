# AARPG Tutorial: C# Translation

A C# translation of [Michael Games' AARPG tutorial series](https://www.youtube.com/@MichaelGamesOfficial) built in Godot 4. The series starts [here](https://youtu.be/QPeycNt29tY?si=Viehkem9jw0uMWAk).

The original tutorial uses GDScript. I follow the same episodes but implement everything in idiomatic C#, with some structural differences noted below.

---

## Differences from the GDScript Tutorial

### General C# Idioms
These are general C# patterns that apply throughout rather than episode-specific decisions:
- State methods return `null` to mean "stay in current state", GDScript returns `self`
- `GetValueOrDefault` used for dictionary lookups with a fallback value, GDScript handles missing keys implicitly with `null`
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
- **`StateMachine.GetState<T>()`:** State transitions use a generic method rather than returning string names. This is type-safe and prevents typos: `StateMachine.GetState<Attack>()` instead of `return "Attack"`. This was originally paired with an `IState` interface, which was later replaced in Episode 9.

### Episode 5
- **Signal subscriptions:** `AnimationFinished += EndAttack` / `-=` is used instead of GDScript's `connect`/`disconnect`. The `+=` pattern requires manual cleanup with `-=` (done in `Exit()`), which is easy to forget compared to GDScript's more explicit `disconnect`. Note: `AnimationFinished` is a built-in Godot signal, so no `[Signal]` delegate needed here.
- **Custom signals:** Follow the C# Godot convention: `[Signal]` on a delegate ending in `EventHandler`, emitted via `EmitSignal(SignalName.X)` rather than GDScript's `signal name(params)` / `name.emit()`. The `EventHandler` suffix is required: Godot strips it to derive the signal name, so `DamagedEventHandler` becomes the `Damaged` signal.

### Episode 6
- **Naming:** The shared nodes folder is named `Common/` instead of `GeneralNodes/`, and `PlayerInteractionsHost` is named `PlayerInteractionsManager`
- **Node references as exports:** Rather than using hardcoded `GetNode("../../Some/Path")` strings, node references are exposed as `[Export]` properties and assigned by dragging nodes into Inspector slots in the editor. This makes the code resilient to scene tree restructuring. See [Editor Wiring](#editor-wiring) below for the full mapping.
- **`InputActions.cs`:** Input action strings centralized in a static class instead of scattered string literals
- **Pattern matching:** Switch expressions and `is` type casts used where applicable (e.g. `area is HitBox hitBox` in HurtBox, direction mapping in `PlayerInteractionsManager`)
- **`async void Enter()`:** `StateAttack.Enter()` uses `async void` with `await ToSignal()` to delay hitbox activation. GDScript handles this inline with `yield`. `async void` is generally discouraged in C# but is the accepted pattern for Godot lifecycle methods that need to await

### Episode 7
- **Moonwalk bug:** Michael fixes this with angle/TAU math, not needed here since it was already handled in Episode 1 with `Vector2.Abs()`
- **HurtBox color:** The tutorial uses yellow for the HurtBox debug color, I use green
- **HurtBox naming:** The tutorial renames the HurtBox to `AttackHurtBox`, I keep it as `HurtBox`

### Episode 8
- **Globals folder:** Michael creates a `Globals/` folder with a `GlobalLevelManager.gd` script. I use the existing `Common/` folder for shared/global scripts instead
- **Autoload access:** GDScript autoloads are accessible like static methods anywhere in the project. In C#, `GlobalLevelManager` exposes a static `Instance` property set in `_Ready()`, so it can be accessed as `GlobalLevelManager.Instance` without `GetNode` calls
- **Nullable return types:** `State.Process()`, `Physics()`, and `HandleInput()` are marked `State?` to explicitly declare that returning `null` ("stay in current state") is intentional
- **Collection expressions:** `List<State>` initialized with `[]` instead of `new List<State>()` (C# 12 collection expression syntax)
- **Non-null pattern:** `FirstOrDefault() is { }` used instead of `is State` type check, which is more precise since it matches any non-null value rather than checking the type
- **Type alias for namespace conflict:** `HurtBox.cs` uses `using HitboxArea2D = AARPGtutorial.Common.HitBox.HitBox;` to resolve the ambiguity between the `HitBox` namespace and `HitBox` class, a consequence of the namespace and class sharing the same name
- **`using` aliases:** Used to shorten verbose Godot types, e.g. `using GodotVector2Array = Godot.Collections.Array<Godot.Vector2>;`. `List<T>` is generally preferred in C#, but Godot signal parameters require Godot's own array types.

### Episode 9
- **Attribute placement:** `[Export]`, `[Signal]`, and other attributes are placed on their own line above the declaration rather than inline (standard C# formatting convention)
- **Player node references as exports:** `AnimationPlayer`, `Sprite2D`, and `PlayerStateMachine` in `PlayerCharacter.cs` were still using `GetNode` with string constants from earlier episodes. Converted to `[Export]` properties to match the pattern established in Episode 6.
- **Enemy node references as exports:** `AnimationPlayer`, `Sprite2D`, and `EnemyStateMachine` in `Enemy.cs` wired up as `[Export]` properties, consistent with the Episode 6 pattern.
- **Player renamed to PlayerCharacter:** `Player` is both the class name and a namespace, causing ambiguity. Renamed the class to `PlayerCharacter` to resolve it.

#### Major Architecture Refactor
Michael's Episode 9 introduced significant structural changes. Taking that opportunity, I went further by establishing shared base classes to keep things DRY and set up a clean foundation for future work:

- **`Actor`** (`Common/Actor.cs`): shared base for `PlayerCharacter` and `Enemy`. Direction tracking, sprite flipping, animation, and `SetDirection()` all live here instead of being duplicated in both classes.
- **`State`** (`Common/States/State.cs`): universal base for all states. Replaces the broken `IState` interface from Episode 3. All states share the same contract (`Init`, `Enter`, `Exit`, `Process`, `Physics`, `HandleInput`).
- **`StateMachine<TActor, TState>`** (`Common/States/StateMachine.cs`): generic abstract base. Child state discovery, `GetState<T>()`, `ChangeState()`, and the process loop are shared. `PlayerStateMachine` and `EnemyStateMachine` only handle actor-specific initialization.

State files were also reorganized:
- `Scripts/States/` folders removed, states now live in `Player/States/` and `Enemies/States/`
- Class names drop the actor prefix (`PlayerStateIdle` -> `Idle`) since the namespace implies context
- `PlayerStateWalk` and `EnemyStateWander` unified as `Move`

> **Note for anyone following along:** This refactor changes class names, file paths, and wiring for all state scripts. It's a significant divergence from Michael's structure. I will try to keep it closer to the tutorial in the future, but for this episode you might be better off pulling down this commit and looking through it than replicating it step by step. See [Editor Wiring](#editor-wiring) for current mappings.

---

## Editor Wiring

Because node references use `[Export]` rather than hardcoded `GetNode` paths, you need to assign them in the Godot editor. Open each scene and drag the listed nodes into the corresponding Inspector slots.

### Episodes 6-8

#### `player.tscn` - Attack state node (`StateMachine > Attack`)
| Property                | Node to assign                                |
|-------------------------|-----------------------------------------------|
| Player Animation Player | `AnimationPlayer`                             |
| Attack Animation Player | `Sprite2D/AttackEffectSprite/AnimationPlayer` |
| Audio Stream Player 2D  | `Audio/AudioStreamPlayer2D`                   |
| Hurt Box                | `Interactions/HurtBox`                        |

#### `player.tscn` - Interactions node (`Interactions`)
| Property | Node to assign       |
|----------|----------------------|
| Player   | `Player` (root node) |

#### `Plant.tscn`
| Property | Node to assign |
|----------|----------------|
| Hit Box  | `HitBox`       |

---

### Episode 9+

#### `player.tscn` - PlayerCharacter node (root)
| Property         | Node to assign    |
|------------------|-------------------|
| Animation Player | `AnimationPlayer` |
| Sprite 2D        | `Sprite2D`        |
| State Machine    | `StateMachine`    |

#### `player.tscn` - Attack state node (`StateMachine > Attack`)
| Property                | Node to assign                                |
|-------------------------|-----------------------------------------------|
| Player Animation Player | `AnimationPlayer`                             |
| Attack Animation Player | `Sprite2D/AttackEffectSprite/AnimationPlayer` |
| Audio Stream Player 2D  | `Audio/AudioStreamPlayer2D`                   |
| Hurt Box                | `Interactions/HurtBox`                        |

#### `player.tscn` - Interactions node (`Interactions`)
| Property         | Node to assign                |
|------------------|-------------------------------|
| Player Character | `PlayerCharacter` (root node) |

#### `Slime.tscn` - Slime node (root)
| Property         | Node to assign    |
|------------------|-------------------|
| Animation Player | `AnimationPlayer` |
| Sprite 2D        | `Sprite2D`        |
| State Machine    | `StateMachine`    |

#### `Slime.tscn` - Idle state node (`StateMachine > Idle`)
| Property   | Node to assign      |
|------------|---------------------|
| Next State | `StateMachine/Move` |

#### `Slime.tscn` - Move state node (`StateMachine > Move`)
| Property   | Node to assign      |
|------------|---------------------|
| Next State | `StateMachine/Idle` |

#### `Plant.tscn`
| Property | Node to assign |
|----------|----------------|
| Hit Box  | `HitBox`       |

---

## Commit History by Episode

> **Note:** This wasn't originally intended to be shared, so the first few commits cover multiple episodes. Going forward commits map to individual episodes.

| Commit    | Episodes | Description                                                                            |
|-----------|----------|----------------------------------------------------------------------------------------|
| `4cf93fd` | 1-3      | Initial setup: player movement and state machine                                       |
| `a070345` | -        | Refactor: moved shared state logic into base `State` class                             |
| `f7e64b1` | -        | Refactor: moved enums into dedicated `Player/Enum/` folder                             |
| `c3ea106` | 4-5      | Attack state, terrain tilemap, terrain collisions                                      |
| `e88bbe8` | 6        | HitBox/HurtBox system, shrubs/plants, `PlayerInteractionsManager`, export wiring       |
| `774a925` | 7        | Episode 7 cleanup: moonwalk fix not needed, export wiring                              |
| `b2e0ba8` | -        | Added README                                                                           |
| `eaa51c5` | 8        | Episode 8: Auto Camera Limits + small refactors                                        |
| `246a62d` | -        | Updated README for Episode 8                                                           |
| `e5bb347` | 8        | HitBox type alias fix: resolved namespace/class name conflict in `HurtBox.cs`          |
| `3e93eda` | 9        | Episode 9: Implemented slime enemy and major refactor of states, actors, statemachines |
| TBD       | -        | Update README                                                                          | 
