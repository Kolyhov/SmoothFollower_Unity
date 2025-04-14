# SmoothFollower — Procedural Second Order Target Tracking for Unity 2D/3D

SmoothFollower is a Unity component that enables physically-inspired, smooth tracking of a target using second-order dynamics. It supports both UI and world-space objects, cursor or transform targets, adjustable dynamics, and per-axis control. Ideal for floating UI cursors, camera smoothing, trailing effects, or animated elements that require natural motion.

## Features

- Target following via second-order system
- Adjustable dynamics parameters: Frequency, Damping, Initial Response
- Target can be a world-space `Transform` or the screen-space `Cursor`
- Works with UI and 3D objects
- Per-axis movement multipliers (X/Y/Z)
- Runtime toggle between `Update` or `FixedUpdate` for integration flexibility

---

## Parameters

### `F` — Frequency (Hz)
Determines how fast the follower reacts to changes in target position.

- Higher values result in quicker response.
- Low values produce slow, inertial movement.

**Recommended ranges:**
- `5–10`: Robotic, fast-following
- `1–3`: Smooth and elastic
- `0.5–1`: Heavy, slow, fluid-like movement

---

### `Z` — Damping ratio (ζ)
Controls how the motion settles around the target.

- `0`: No damping — infinite oscillation
- `0 < Z < 1`: Underdamped — bouncy or springy
- `Z = 1`: Critically damped — smooth approach without overshooting
- `Z > 1`: Overdamped — slow convergence, like moving through thick fluid

---

### `R` — Initial response
Controls how the follower initiates its motion.

- `R = 0`: Slow start
- `R > 0`: Immediate reaction
- `R > 1`: Overshoots the target
- `R < 0`: Anticipates motion before it happens

---

## Usage

1. Add the `SmoothFollower` script to any object in your scene.
2. Set the `Target Type` (Transform or Cursor).
3. Tune parameters `F`, `Z`, `R` to achieve the desired motion.
4. Use `Offset` to shift the final position if needed.
5. Configure per-axis multipliers (X/Y/Z) to restrict or dampen motion in specific directions.

---

## Example Use Cases

- Floating cursor that softly trails the system cursor
- Camera smoothing behind a player character
- Floating text or health bars that follow enemies

---

## Author

Created by **Oleksii Koliukhov**  
GitHub: [https://github.com/Kolyhov](https://github.com/Kolyhov)  
Based in Odesa, Ukraine 🇺🇦  