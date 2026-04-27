# AARPG Tutorial: C# Translation

A C# translation of [Michael Games' AARPG tutorial series](https://www.youtube.com/@MichaelGamesOfficial) built in Godot 4. The series starts [here](https://youtu.be/QPeycNt29tY?si=Viehkem9jw0uMWAk). Michael's GitHub link to the tutorial is [here](https://github.com/michaelmalaska/aarpg-tutorial)

The original tutorial uses GDScript. I follow the same episodes but implement everything in idiomatic C#, with some structural differences noted below.

> **Note:** I am doing this for fun and to learn Godot game development. I am a professional C# developer, but work in web development, so there are things about Godot and game development conventions/patterns that I am learning! That's why I'm here!
---

## Differences from the GDScript Tutorial

### General C# Idioms
- There are many things you are going to find different from the tutorial if you're following along. A lot of this has to do with C# conventions. Like, passing raw strings is usually frowned upon, and enums preferred. I'm not new to C#, but I am new to game development and Godot, so you'll find I update things at certain points if I find a better way to do it. Like switching EmitSignal(SignalName.Damaged... blah blah) to EmitSignalDamaged();
- GDScript's `@onready` has no direct C# equivalent. If you want to follow Michael's approach more closely, assign node references in `_Ready()` instead of using `[Export]`:
  ```csharp
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
      _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
  }
  ```
- The loose strings bothered me, so now every time Michael adds an @onready variable, I instead export it and wire it up in the editor (just drag and drop the node/animationplayer etc. to the relevant export). See [Editor Wiring](#editor-wiring)
    - There are pros and cons to this approach.
        - Pros: better for refactoring. References will stay linked when moving files around the tree.
        - Cons: If you edit the export variable, rename it or move the code file, you will need to rewire everything again, so keep that in mind.

#### Godot Memory Management
Godot signals only accept **Variant-compatible types** (no custom C# classes!), which in practice means anything that inherits from `GodotObject`. For custom C# classes, that means picking the right base to inherit from:

| Base class             | Memory management                                                      | When to use                                   |
|------------------------|------------------------------------------------------------------------|-----------------------------------------------|
| `RefCounted`           | Automatic - freed when ref count hits zero                             | Code-only data carriers (DTOs, value objects) |
| `Node`                 | Freed with parent when in scene tree; call `QueueFree()` if outside it | Anything with scene presence                  |
| `GodotObject` directly | Manual `Free()` required, no safety net                                | Avoid unless you have a specific reason       |

If you need to pass a custom object through a signal, inherit `RefCounted` and you're done.

### Episode 1
- **Enums over strings:** Player state and animation direction are defined as proper C# `enum` types rather than raw strings
- **SetDirection:** Used `Vector2.Abs()` dominant-axis comparison instead of raw direction checks, which avoids the moonwalk bug Michael fixes in Episode 7

### Episode 2
- **Enum folder:** Enums moved to a dedicated `Player/Enum/` folder for organization

### Episode 3
- **`StateMachine.GetState<T>()`:** State transitions use a generic method rather than returning string names. This is type-safe and prevents typos: `StateMachine.GetState<Attack>()` instead of `return "Attack"`. This was originally paired with an `IState` interface, which was later replaced in Episode 9.

### Episodes 4-5
- I mostly kept everything to the way Michael had it.

### Episode 6
- **Naming:** The shared nodes folder is named `Common/` instead of `GeneralNodes/`, and `PlayerInteractionsHost` is named `PlayerInteractionsManager`
- **Node references as exports:** Rather than using hardcoded `GetNode("../../Some/Path")` strings, node references are exposed as `[Export]` properties and assigned by dragging nodes into Inspector slots in the editor. This makes the code resilient to scene tree restructuring. See [Editor Wiring](#editor-wiring) below for the full mapping.
- **`InputActions.cs`:** Input action strings centralized in a static class instead of scattered string literals
- **`async void Enter()`:** At the time of this commit, `StateAttack.Enter()` used `async void` with `await ToSignal()` to delay hitbox activation, this is bad practice and was later replaced with `GetTree().CreateTimer(.075).Timeout += () => HurtBox.Monitoring = true;`, which is purely event-based and needs no async at all.

### Episode 7
- **Moonwalk bug:** Michael fixes this with angle/TAU math, not needed here since it was already handled in Episode 1 with `Vector2.Abs()`
- **HurtBox color:** The tutorial uses yellow for the HurtBox debug color, I use green
- **HurtBox naming:** The tutorial renames the HurtBox to `AttackHurtBox`, I keep it as `HurtBox`

### Episode 8
- **Globals folder:** Michael creates a `Globals/` folder with a `GlobalLevelManager.gd` script. I use the existing `Common/` folder for shared/global scripts instead
- **Autoload access:** GDScript autoloads are accessible like static methods anywhere in the project. In C#, `GlobalLevelManager` exposes a static `Instance` property set in `_Ready()`, so it can be accessed as `GlobalLevelManager.Instance` without `GetNode` calls

### Episode 9
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

> **Note for anyone following along:** This refactor changes class names, file paths, and wiring for all state scripts. It's a significant divergence from Michael's structure. I will try to keep it closer to the tutorial in the future. See [Editor Wiring](#editor-wiring) for current mappings.

### Episode 10
- **Realization:** There may be some context from Michael's restructure that I missed, as I ran into some behavior differences, like the overlapping hitbox/hurtbox causing issues that didn't seem to affect Michael's version.
    - I'll still get everything in the same playable state at the end of episodes. I will go through his repo and make sure we're 1:1 (except for abstraction layers) when we get to episode 14.
    - I will go through his repo and make sure we're 1:1 (except for abstraction layers) when we get to episode 14.
- **Slime HurtBox removed:** The slime scene had both a HitBox and a HurtBox. The HurtBox was causing the slime to immediately register as hit on startup due to area overlap. It has been removed for now and will be re-added when enemy contact damage is implemented.

### Post-Episode 10 (Light Housekeeping)
All existing functionality is unchanged. No scene wiring was affected.
- **`Organization`** Reorganized classes to have a consistent structure and use #regions. I also updated the casing to `AarpgTutorial` to match Rider's conventions.
    - I'm using Rider for development, and the regions are really nice when looking at the structure tab.
- **`Bounds` class:** `Common/Bounds.cs` is a `RefCounted` subclass holding `Left`, `Top`, `Right`, `Bottom` as `int` properties. Replaces the `GodotVector2Array` that was threaded between `LevelTileMap`, `LevelManager`, and `PlayerCamera`. `RefCounted` objects are reference-counted and free themselves automatically when no longer referenced, so no `QueueFree()` is needed. The `using GodotVector2Array = ...` alias in those files is gone.

### Episode 11
- I kept fairly close to the tutorial for this episode.
- **State abstraction:** I moved the `Init(TActor actor)` call and the `Initialize` implementation up into the base `StateMachine<TActor, TState>` as `PlayerStateMachine` and `EnemyStateMachine` were pretty much the same class. Michael made some interesting structural decisions here that gave me a good opportunity to clean things up.
- **Folder and class rename:** I renamed the `Player/` folder to `PlayerCharacter/` and the `PlayerCharacter` class back to `Player`, to bring it closer to Michael's naming. This also resolves the namespace/class collision that forced the rename in Episode 9.
- **XML documentation:** I added XML doc comments to all classes and non-trivial methods. I find it helps me keep things organized.

> **Solution Rename:** There is a commit right after this episode called `Solution Rename` this was to make it more idiomatic and align the namespace `AarpgTutorial` across the solution and in Godot. I also changed all exports to `Public` so I won't have to rewire if we decide to use it outside the class.
>
> **I highly recommend** just pulling this down if your following my code closely or just not renaming yours. It takes a bit of effort to fix manually, requiring edits to .csproj files, rewiring all exports in Godot, and regenerating the project in Godot. I will try to keep file structure renames to a minimum going forward.

### Episode 12
- **`HeartGui` refactor:** I rolled the update method into the setter.
- **Player Hud refactor:** I found this easier to follow by keeping it to a single for loop. 

### Episode 13
- Stuck very close to the tutorial on this one.

### Episode 14
- **`[ExportToolButton]`:** Used instead of the exported bool snap-to-grid trick. Renders a real button in the inspector.
- **C# event cleanup:** Added `LevelLoadStarted -= FreeLevel` in `FreeLevel()`. GDScript cleans up signal connections automatically on node free, C# doesn't.
- **`async void` for frame delays:** Originally used `async void _Ready()` with `await ToSignal(GetTree(), ProcessFrame)` to delay `LevelLoadFinished`. Bad practice, so moved to a private `async Task` method called with `_ =` discard from `_Ready()`.

### Episode 15
- **`JSON`:** Used C# built in file writing instead of the Godot version
- **`LoadNewLevel`:** Await works differently for C# than Godot. In C# it waits until the whole method finishes, and we have to await on `LoadNewLevel` because it has awaits inside of it, so it has to be async up the chain as far as we can go. I got around this by injecting a lambda method to happen during `FadeIn()` and `FadeOut()`.
- **Tools & Rider:** These two don't always play nicely together. Rider triggers an autosave function constantly that I'm very fond of. The issue is this can cause problems where tool scripts unlink from their scenes. No more tool script.
- **Slime:** The slime's death animation had some lingering issues, a shadow and hurtbox that stuck around. I added keyframes to stop the hurtbox from monitoring and turned the shadow invisible, both set to trigger when we make the slime invisible.
- **Bug:** Next episode commit I found a bug where the mouse wasn't working on the menu, most likely from this episodes work. Bump up the layer on the CanvasLayer node for the Pause Menu, and that should sort it out. Fixed in next episode's commit. 
- **Minor Refactoring**

### Episode 16
- **Font:** Added a custom font to better match the look of Michael's UI.
- **Pause Menu wiring:** No `@onready` renaming needed in the Pause Menu control if you've been following the export-for-onready pattern. They'll stay wired!
- **Bug fix - PauseMenu CanvasLayer layer:** Another CanvasLayer was rendering on top of the pause menu and eating mouse input, making buttons unresponsive to the mouse (keyboard/gamepad still worked). Fixed by setting the PauseMenu CanvasLayer `layer` to `10`. I missed it earlier as I've been mainly using a gamepad.
- **Singleton `Instance` moved to `_EnterTree`:** In Godot with C#, autoloads don't give you a typed static reference automatically, so we use an `Instance = this` pattern to access singletons from anywhere in the project. The reason I moved them out of `_Ready` is that I learned `_Ready` runs bottom-up (children first, parents after), so a child could try to access `Instance` before the parent has set it. `_EnterTree` runs top-down, so `Instance` is always set before any child needs it.
### Episode 17
- **No `@tool` on ItemPickup:** I did attempt to get the tool script working for the editor texture preview, but ran into a casting issue where the tool script runs before our `[GlobalClass]` resource gets cast to `ItemData`, throwing a bunch of errors. I've stepped back from tool scripts for now since they've been a bit finicky. If you find a fix, let me know!
- **Inventory updates in place:** I wanted to see if I could update inventory slots in place rather than clearing and rebuilding them on every change, so I strayed from the tutorial here. Slot nodes are created once in `_Ready` and their data is swapped out in `UpdateInventory`. One thing to watch out for: if you have any `InventorySlotUI` nodes saved as children under the `GridContainer` in `PauseMenu.tscn`, delete them or they'll double up at runtime.
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

### Episode 9-10

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
| Hit Box          | `HitBox`          |

#### `Slime.tscn` - Idle state node (`StateMachine > Idle`)
| Property   | Node to assign      |
|------------|---------------------|
| Next State | `StateMachine/Move` |

#### `Slime.tscn` - Move state node (`StateMachine > Move`)
| Property   | Node to assign      |
|------------|---------------------|
| Next State | `StateMachine/Idle` |

#### `Slime.tscn` - Stun state node (`StateMachine > Stun`)
| Property   | Node to assign      |
|------------|---------------------|
| Next State | `StateMachine/Idle` |

#### `Plant.tscn`
| Property | Node to assign |
|----------|----------------|
| Hit Box  | `HitBox`       |

---

### Episode 11+

Scene moved from `Player/player.tscn` to `PlayerCharacter/player.tscn`. Root node renamed from `PlayerCharacter` to `Player`.

#### `PlayerCharacter/player.tscn` - Player node (root)
| Property                 | Node to assign          |
|--------------------------|-------------------------|
| Animation Player         | `AnimationPlayer`       |
| Sprite 2D                | `Sprite2D`              |
| State Machine            | `StateMachine`          |
| Effect Animation Player  | `EffectAnimationPlayer` |
| Hit Box                  | `HitBox`                |

#### `PlayerCharacter/player.tscn` - Attack state node (`StateMachine > Attack`)
| Property                | Node to assign                                |
|-------------------------|-----------------------------------------------|
| Player Animation Player | `AnimationPlayer`                             |
| Attack Animation Player | `Sprite2D/AttackEffectSprite/AnimationPlayer` |
| Audio Stream Player 2D  | `Audio/AudioStreamPlayer2D`                   |
| Hurt Box                | `Sprite2D/HurtBox`                            |

#### `PlayerCharacter/player.tscn` - Stun state node (`StateMachine > Stun`)
| Property   | Node to assign      |
|------------|---------------------|
| Idle State | `StateMachine/Idle` |

#### `PlayerCharacter/player.tscn` - Interactions node (`Interactions`)
| Property | Node to assign       |
|----------|----------------------|
| Player   | `Player` (root node) |

#### `Slime.tscn` - unchanged from Episode 9-10

#### `Plant.tscn` - unchanged from Episode 9-10

#### `GUI/PlayerHud/PlayerHud.tscn` - PlayerHud node (root)
| Property         | Node to assign            |
|------------------|---------------------------|
| H Flow Container | `Control/HFlowContainer`  |

#### `GUI/PlayerHud/HeartGui.tscn` - HeartGui node (root)
| Property | Node to assign |
|----------|----------------|
| Sprite   | `Sprite2D`     |

---

## Commit History by Episode

> **Note:** This wasn't originally intended to be shared, so the first few commits cover multiple episodes. Going forward commits map to individual episodes.

| Commit    | Episodes | Description                                                                                               |
|-----------|----------|-----------------------------------------------------------------------------------------------------------|
| `4cf93fd` | 1-3      | Initial setup: player movement and state machine                                                          |
| `a070345` | -        | Refactor: moved shared state logic into base `State` class                                                |
| `f7e64b1` | -        | Refactor: moved enums into dedicated `Player/Enum/` folder                                                |
| `c3ea106` | 4-5      | Attack state, terrain tilemap, terrain collisions                                                         |
| `e88bbe8` | 6        | HitBox/HurtBox system, shrubs/plants, `PlayerInteractionsManager`, export wiring                          |
| `774a925` | 7        | Episode 7 cleanup: moonwalk fix not needed, export wiring                                                 |
| `b2e0ba8` | -        | Added README                                                                                              |
| `eaa51c5` | 8        | Episode 8: Auto Camera Limits + small refactors                                                           |
| `246a62d` | -        | Updated README for Episode 8                                                                              |
| `e5bb347` | 8        | HitBox type alias fix: resolved namespace/class name conflict in `HurtBox.cs`                             |
| `3e93eda` | 9        | Episode 9: Implemented slime enemy and major refactor of states, actors, statemachines                    |
| `3cb50b9` | -        | Updated README for Episode 9                                                                              |
| `37f62d9` | 10       | Updated Slime to take damage                                                                              |
| `e2b1228` | -        | Light housekeeping: namespace casing, region organization, `Bounds` class                                 |
| `9d671c9` | 11       | Player stun state, health system, damage flash, folder/class rename to match tutorial                     |
| `98d7094` | -        | Solution Rename. If you're following this code closely, I'd just pull it down to avoid renaming headaches |
| `aff1a8b` | 12       | Player HUD: heart-based health display with half-heart support                                            |
| `1ed0b3a` | -        | Fixed lingering slime shadow                                                                              |
| `538950a` | 13       | Episode 13: player spawn implementation                                                                   |
| `c06fd1f` | 14       | Episode 14: level transitions                                                                             |
| `c676dd4` | 15       | Episode 15: save/load system                                                                              |
| `b7ec5b2` | -        | Reorganization                                                                                            |
| `8c9a033` | 16       | Added inventory system                                                                                    |
| `Latest`  | 17       | Can now pick up and use items from the UI                                                                 |