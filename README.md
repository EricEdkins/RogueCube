# RogueCube

A roguelite built in Unity 6 with C#.

## The game

The player dodges and shoots enemies, earning gold from kills to spend on upgrades between runs. A short loop that rewards movement over standing still.

## Systems worth noting

**Blink mechanic.** Short-range teleport used as the primary defensive move. Positions the player instantly rather than accelerating them, which changes how dodging feels — you commit to a destination instead of steering toward one.

**Face-distributed projectile spawning.** Projectiles originate from points distributed across the faces of the cube rather than from a single muzzle, so fire direction is tied to the player's orientation instead of a fixed forward vector.

**Enemy merge system.** Enemies combine into larger, tougher units under certain conditions, so the threat scales through consolidation rather than only by spawning more bodies.

## Built with

- Unity 6
- C# for all gameplay logic

## Repository structure

```
Assets/           game code and assets
Packages/         Unity package manifest
ProjectSettings/  project configuration
```

## Notes

Solo project. All gameplay code is mine.

The `.gitattributes` file marks shaders, Unity metadata, and package files as vendored or generated so GitHub's language statistics reflect the C# that was actually written rather than the assets Unity generates.
