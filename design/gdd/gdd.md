# Game Design Document (GDD): Dragon Runner 2D (TP05)

## 1. Executive Summary
- **Title**: Dragon Runner 2D
- **Project**: TP05_C1_Rey_Runner2D
- **Genre**: 2D Side Scroller Infinity / Endless Runner
- **Engine**: Unity 2D (URP)
- **Target Audience / References**: Google Dino, Jetpack Joyride, Flappy Bird

## 2. Core Mechanics (MDA Framework)
### 2.1 Movement & Scrolling
- **Horizontal Stationary Player**: The dragon is fixed on the X-axis ($X \approx -5$). The ground, obstacles, and parallax layers scroll left at speed defined in `RunnerConfigSO`.
- **Variable Height Jump**: Pressing and holding Space or 'W' applies continuous vertical force for dynamic jump control.
- **Visual Feedback**:
  - `TrailRenderer` attached to dragon tail.
  - `ParticleSystem` triggers dust clouds on ground impact / landing.

### 2.2 Obstacles & Spawning
- **Random Spawner**: Spawns obstacles (`Rockon.png` and variations) at randomized intervals and heights.
- **Procedural Animations**: Obstacles have subtle rotation/bobbing.

### 2.3 Power-Ups & Collectibles
- **Pickable Animation**: Sinusoidal floating (`SineBobber`) and rotation.
- **Power-Up 1 - Invincibility (5s)**: Grants immune state, flashing visual feedback, and active countdown timer on the HUD.
- **Power-Up 2 - Extra Life (+1)**: Restores or grants an extra life, updating HUD hearts/count.

### 2.4 Parallax & Biome System
- **3+ Background Layers**: Far background, mid-ground, near silhouette scrolling at relative speed ratios ($0.1\times, 0.3\times, 0.6\times$).
- **BiomeDataSO**: ScriptableObject architecture enabling hot-swapping visual biomes seamlessly.

### 2.5 Audio System & AudioMixer
- **Centralized AudioManager**: Persistent across scenes (`DontDestroyOnLoad`).
- **AudioMixer Channels**: Master, Music, SFX with logarithmic dB volume conversion.
- **Interactions**: Menu BGM, Gameplay BGM, UI Click, Jump, Land, Collectible Pickup, Game Over.

### 2.6 Scene Architecture
- **MainMenuScene**: Start game, Settings (AudioMixer sliders), Credits, Quit.
- **GameplayScene**: Runner gameplay, HUD, Pause menu with Settings overlay, Game Over screen.
