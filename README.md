# AARPG Tutorial: C# Translation

A C# translation of [Michael Games' AARPG tutorial series](https://www.youtube.com/@MichaelGamesOfficial) built in Godot 4. Series starts [here](https://youtu.be/QPeycNt29tY?si=Viehkem9jw0uMWAk), Michael's GitHub is [here](https://github.com/michaelmalaska/aarpg-tutorial).

I follow the same episodes but implement everything in C#, with some differences along the way.

> [!NOTE]
> I'm doing this for fun. I'm a professional C# dev but come from web, so game dev and Godot conventions are new territory for me. Learning as I go!
---

## Differences from the GDScript Tutorial

### General C# Idioms
- If you're following along you'll find a lot of things different, mostly C# conventions. Passing raw strings is frowned upon so I use enums, I swap patterns out when I find better ones, stuff like that. For example, switching `EmitSignal(SignalName.Damaged...)` to the generated `EmitSignalDamaged()`.
- GDScript's `@onready` has no direct C# equivalent. If you want to follow Michael's approach more closely, assign node references in `_Ready()` instead of using `[Export]`:
  ```csharp
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
      _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
  }
  ```
- The loose strings bothered me, so now every time Michael adds an @onready variable, I instead export it and wire it up in the editor (just drag and drop the node/animationplayer etc. to the relevant export).
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

## Differences by Episode

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
- **Node references as exports:** Instead of hardcoded `GetNode("../../Some/Path")` strings, I expose node references as `[Export]` properties and drag them in via the Inspector. Way easier to maintain when you move things around.
- **`InputActions.cs`:** Input action strings centralized in a static class instead of scattered string literals
- **`async void Enter()`:** `StateAttack.Enter()` originally used `async void` with `await ToSignal()` to delay hitbox activation, which is bad practice. Later replaced with `GetTree().CreateTimer(.075).Timeout += () => HurtBox.Monitoring = true;`, which is purely event-based.

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

> [!TIP]
> This is optional! If you're just following along with Michael's tutorial you can skip this entirely and stay closer to his structure. I went further here because I wanted to, not because you have to.

Michael's Episode 9 introduced some big structural changes. I used it as an excuse to go further and add shared base classes:

- **`Actor`** (`Common/Actor.cs`): shared base for `PlayerCharacter` and `Enemy`. Direction tracking, sprite flipping, animation, and `SetDirection()` all live here instead of being duplicated in both classes.
- **`State`** (`Common/States/State.cs`): universal base for all states. Replaces the broken `IState` interface from Episode 3. All states share the same contract (`Init`, `Enter`, `Exit`, `Process`, `Physics`, `HandleInput`).
- **`StateMachine<TActor, TState>`** (`Common/States/StateMachine.cs`): generic abstract base. Child state discovery, `GetState<T>()`, `ChangeState()`, and the process loop are shared. `PlayerStateMachine` and `EnemyStateMachine` only handle actor-specific initialization.

State files were also reorganized:
- `Scripts/States/` folders removed, states now live in `Player/States/` and `Enemies/States/`
- Class names drop the actor prefix (`PlayerStateIdle` -> `Idle`) since the namespace implies context
- `PlayerStateWalk` and `EnemyStateWander` unified as `Move`


### Episode 10
- **Realization:** I ran into some behavior differences from Michael's version, like the overlapping hitbox/hurtbox, that I think came from missing some context in his restructure. Everything still ends up in the same playable state by the end of the episode though.
- **Slime HurtBox removed:** The slime scene had both a HitBox and a HurtBox. The HurtBox was causing the slime to immediately register as hit on startup due to area overlap. It has been removed for now and will be re-added when enemy contact damage is implemented.

#### Post-Episode 10 (Light Housekeeping)
No behavior changes, just cleanup.
- **Organization:** Gave all classes a consistent structure using `#regions`, and updated the casing to `AarpgTutorial` to match Rider's conventions. Rider's structure tab is really nice once regions are in place.
- **`Bounds` class:** Replaced the `GodotVector2Array` that was getting passed around between `LevelTileMap`, `LevelManager`, and `PlayerCamera` with a simple `Bounds` class (`RefCounted`) holding `Left`, `Top`, `Right`, `Bottom`. Cleaner to pass around, and `RefCounted` means it frees itself automatically.

### Episode 11
- I kept fairly close to the tutorial for this episode.
- **State abstraction:** I moved the `Init(TActor actor)` call and the `Initialize` implementation up into the base `StateMachine<TActor, TState>` as `PlayerStateMachine` and `EnemyStateMachine` were pretty much the same class. Michael made some interesting structural decisions here that gave me a good opportunity to clean things up.
- **Folder and class rename:** I renamed the `Player/` folder to `PlayerCharacter/` and the `PlayerCharacter` class back to `Player`, to bring it closer to Michael's naming. This also resolves the namespace/class collision that forced the rename in Episode 9.
- **XML documentation:** I added XML doc comments to all classes and non-trivial methods. I find it helps me keep things organized.

> [!TIP]
> If you're not following my namespace exactly, just skip this one. There's a commit right after this episode called `Solution Rename` that renames the namespace to `AarpgTutorial` across the whole solution. Doing it manually is a pain: it touches `.csproj` files, requires rewiring exports in Godot, and regenerating the project. Just pull it down if you want it.

### Episode 12
- **`HeartGui` refactor:** I rolled the update method into the setter.
- **Player Hud refactor:** I found this easier to follow by keeping it to a single for loop. 

### Episode 13
- Stuck very close to the tutorial on this one.

### Episode 14
- **`[ExportToolButton]`:** Used instead of the exported bool snap-to-grid trick to get a real button in the inspector. Ended up removing it though, see the caution note below.
- **C# event cleanup:** Added `LevelLoadStarted -= FreeLevel` in `FreeLevel()`. GDScript cleans up signal connections automatically on node free, C# doesn't.
- **`async void` for frame delays:** Originally used `async void _Ready()` with `await ToSignal(GetTree(), ProcessFrame)` to delay `LevelLoadFinished`. Bad practice, so moved to a private `async Task` method called with `_ =` discard from `_Ready()`.

> [!CAUTION]
> Avoid tool scripts in C# if you can help it. They repeatedly disconnect export links and remove themselves from nodes, and recovering them is a pain even with git. Not worth it. 

### Episode 15
- **`JSON`:** Used C# built in file writing instead of the Godot version
- **`LoadNewLevel`:** Await works differently for C# than Godot. In C# it waits until the whole method finishes, and we have to await on `LoadNewLevel` because it has awaits inside of it, so it has to be async up the chain as far as we can go. I got around this by injecting a lambda method to happen during `FadeIn()` and `FadeOut()`.
- **Slime:** The slime's death animation had some lingering issues, a shadow and hurtbox that stuck around. I added keyframes to stop the hurtbox from monitoring and turned the shadow invisible, both set to trigger when we make the slime invisible.
- **Bug:** Found a bug in the next episode where mouse input stopped working on the menu, probably from this episode's work. Fixed by bumping up the CanvasLayer layer on the Pause Menu node.

### Episode 16
- **Font:** Added a custom font to better match the look of Michael's UI.
- **Pause Menu wiring:** No `@onready` renaming needed in the Pause Menu control if you've been following the export-for-onready pattern. They'll stay wired!
- **Bug fix - PauseMenu CanvasLayer layer:** Another CanvasLayer was rendering on top of the pause menu and eating mouse input, making buttons unresponsive to the mouse (keyboard/gamepad still worked). Fixed by setting the PauseMenu CanvasLayer `layer` to `10`. I missed it earlier as I've been mainly using a gamepad.
- **Singleton `Instance` moved to `_EnterTree`:** C# autoloads don't give you a typed static reference automatically, so I use an `Instance = this` pattern. I moved it out of `_Ready` because `_Ready` runs bottom-up (children first), so a child could try to access `Instance` before the parent sets it. `_EnterTree` runs top-down, so it's always ready.

### Episode 17
- **No `@tool` on ItemPickup:** Tried getting the tool script working for the editor texture preview, but hit a casting issue where it runs before `[GlobalClass]` resources get resolved. See the caution note in Episode 14.
- **Inventory updates in place:** I wanted to see if I could update inventory slots in place rather than clearing and rebuilding them on every change, so I strayed from the tutorial here. Slot nodes are created once in `_Ready` and their data is swapped out in `UpdateInventory`. One thing to watch out for: if you have any `InventorySlotUI` nodes saved as children under the `GridContainer` in `PauseMenu.tscn`, delete them or they'll double up at runtime.
- **Bugs:** I definitely introduced some bugs by trying to change the item consume system to be in place. Check below.

#### Post-Episode 17 (Bug Fixes + Cleanup)
- **Camera bounds on transition:** `UpdateBounds()` wasn't firing in `_Ready()`, moved it to `_EnterTree` which runs every time you enter the scene.
  - Not sure when this was introduced
- **Gamepad UI focus:** Focus wasn't being grabbed when opening the pause menu on gamepad.
- **Gamepad UI exit:** Added `ui_cancel` listener so you can actually close the menu.
- **Renaming:** Renamed some things that were confusing me, like `SlotData.cs` to `ItemStack.cs`.
- **XML comments:** Filled in missing doc comments.
- **Item Consume not Updating in UI:** See  [Post-Episode 19 Bug Fix](#post-episode-19-bug-fix)

### Episode 18
- **DTO:** I took a slightly different approach than the tutorial. Created a Data Transfer Object to be the intermediary between Godot's arrays and a C# list, then I used Linq to transfer back and forth, and kept all save logic to the save manager. If you're new to C# you get to really see how powerful Linq can be here!

### Episode 19
- **Item drop abstraction:** Pulled the drop logic out of the destroy state and into a static `Drop()` method on `ItemPickup`. It felt wrong having scatter/spawn logic living inside a state that's really just about the enemy dying. Now any actor can call it, and the destroy state just hands off the work.
- **Resources:** Moved the item resources (gem.tres, apple.tres, etc.) to a resource folder, trying to keep things tidy. 
- **ItemSpawn:** Renamed ItemPickup to ItemSpawn. Seemed more on the nose about what it's actually doing.

#### Post-Episode 19 Bug Fix
- **Item consume count not updating in UI:** The data was right but the inventory screen wouldn't refresh until you closed and reopened it. `ItemStack.SetQuantity` was only calling `EmitChanged()` when quantity hit zero which we didn't need anymore. Remove `if (_quantity <= 0)` from that method

### Episode 20
- **Inventory items not removing after loading a save:** `ConnectSlots()` was never being called after loading inventory from a save file. Items picked up during a session had their `Changed` event properly wired up through `AddItem`, but anything restored from disk was missing the subscription entirely. Consuming those items fired `EmitChanged()` into the void and nothing happened. Fixed by calling `ConnectSlots()` in `SaveManager.Load()` right after the slots are set.
- **Inventory count still not updating for mid-stack decrements:** Turned out yesterday's fix was only half of it. `ItemStack.SetQuantity` was updated to always emit `Changed`, but `InventoryData.OnSlotChanged` was only calling `EmitChanged()` when it found a depleted slot to clear. So 3→2 and 2→1 decrements still fired nothing and the label stayed stale. Moved `EmitChanged()` outside the `if` block so it always propagates.