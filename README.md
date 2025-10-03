<p align="center">
<img src="Documentation/icon.png"/>
</p>

# Nuclear Ability System

[![Nuget](https://img.shields.io/nuget/v/NuclearAbilitySystem)](https://www.nuget.org/packages/NuclearAbilitySystem/)

A flexible ability system that allows creating, configuring, and executing abilities in games

Can be used in Unity 2020.1.4 or later (C# 8)

## Installation
You can install it via NuGet:

```bash
Install-Package NuclearAbilitySystem
```

## Project Structure

The solution is divided into several projects:

1. **NuclearAbilitySystem** - Core framework with interfaces and base implementations
   - Defines core abstractions for units, abilities, and state management
   - Provides event system and command queue implementations
   - No game-specific logic or features

2. **NuclearAbilitySystem.Implementation** - Reference implementation with concrete features
   - Example implementations of units, abilities, and status effects
   - Can be used as-is or replaced with custom implementations

3. **NuclearAbilitySystem.Tests** - Unit and integration tests

### Key Benefits

- **Flexible Architecture** - Core framework is completely decoupled from specific game mechanics
- **State Management** - Copy-on-write pattern for safe state transitions
- **Event System** - Powerful event bus for game logic
- **Extensible** - Easy to add custom implementations of any component

## For Developers

### Core Architecture

The system is built on these key abstractions:

1. **IUnit** - Base interface for game entities
2. **IUnitFeature** - Extensible unit capabilities
3. **IAbility** - Defines unit actions and their execution
4. **ICombatState** - Manages game state with copy-on-write pattern
5. **ICombatEventBus** - Handles event-based communication

### Extending Functionality

#### Creating a New Unit Feature

1. Define the feature interface:

```csharp
public interface IMyFeature : IUnitFeature
{
    void DoSomething();
}
```

2. Implement the mutable version:

```csharp
public class MyFeature : IMyFeature, IUnitFeatureMutable
{
    private ICombatStateMutable _combatState;
    
    public void Connect(ICombatStateMutable combatState)
    {
        _combatState = combatState;
        // Subscribe to events if needed
    }
    
    public void Disconnect()
    {
        // Clean up event subscriptions
        _combatState = null;
    }
    
    public IUnitFeatureMutable DeepClone()
    {
        return new MyFeature();
    }
    
    public void DoSomething()
    {
        // Feature implementation
    }
}
```

3. Add extension methods for easy usage:

```csharp
public static class UnitExtensions
{
    public static T WithMyFeature<T>(this T unit) where T : IUnitMutable
    {
        unit.AddFeature(new MyFeature());
        return unit;
    }
    
    public static IMyFeature GetMyFeature(this IUnit unit)
    {
        return unit.GetUnitFeature<IMyFeature>();
    }
}
```

4. Use your new feature:

```csharp
// Adding the feature to a unit
var unit = new TestUnit("Hero").WithMyFeature();

// Using the feature
unit.GetMyFeature().DoSomething();
```

### Best Practices

1. Keep features small and focused
2. Use events for cross-feature communication
3. Make features immutable when possible
4. Document public APIs thoroughly
5. Write unit tests for all features

## Usage

### Creating a Unit

```csharp
// Create a unit with basic features
var unit = new TestUnit("Player1", health: 100, damage: 10, position: Vector2.Zero);

// Add abilities
unit.WithAbility(new FireballAbility());

// Add status effects
unit.WithBullyStatusEffect();
unit.WithDefenderStatusEffect();
```

### Setting Up Combat State

```csharp
var state = new CombatStateBuilder()
    .WithUnit(playerUnit)
    .WithUnit(enemyUnit)
    .WithAbilityContext(new AbilityContextHolder())
    .Build();
```

### Executing Abilities

```csharp
// Execute ability at index 0 from player to enemy
var result = state.ExecuteAbility(
    sourceId: playerUnit.Id, 
    abilityIndex: 0, 
    targetId: enemyUnit.Id
);

// Process the result
foreach (var command in result.Commands)
{
    // Visualize commands in your game
}
```

### Handling Events

```csharp
// Subscribe to damage events
state.CombatEventBus.Connect<PreDamageEvent, DamageEventResult>((e, prev) => 
{
    // Modify or cancel damage
    return new DamageEventResult(shouldContinue: true);
});
```

## Contributing

### Issues

Issues are very valuable to this project.

- Ideas are a valuable source of contributions others can make
- Problems show where this project is lacking
- With a question you show where contributors can improve the user experience

### Pull Requests

Pull requests are, a great way to get your ideas into this repository.

## License

* MIT