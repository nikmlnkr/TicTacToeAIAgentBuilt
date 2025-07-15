# Unity Tic Tac Toe Game

A classic Tic Tac Toe game built with Unity 6000.1.5f1.

## Features

- **Classic Tic Tac Toe Gameplay**: Play the traditional 3x3 grid game
- **Two Player Local Multiplayer**: X and O players take turns
- **Visual Feedback**: Winning combinations are highlighted in green
- **Game State Management**: Win detection, draw detection, and game restart
- **Clean UI**: Simple and intuitive user interface
- **Sound Support**: Built-in sound manager for future audio enhancements
- **Smooth Animations**: UI transitions and button animations

## Scripts Overview

### Core Game Logic
- **TicTacToeGame.cs**: Main game logic handling moves, win/draw detection, and game state
- **UIManager.cs**: Manages UI elements, transitions, and user interactions
- **SoundManager.cs**: Singleton pattern sound manager for audio effects

### Key Features
- Win detection for all 8 possible winning combinations (3 rows, 3 columns, 2 diagonals)
- Visual feedback with green highlighting for winning cells
- Game state management (active/inactive)
- Turn-based gameplay with clear status updates
- Restart functionality to play multiple rounds

## Unity Version
- **Unity 6000.1.5f1**
- Compatible with Unity 6000.x series

## Project Structure
```
Assets/
├── Scenes/
│   └── TicTacToeGame.unity    # Main game scene
├── Scripts/
│   ├── TicTacToeGame.cs       # Core game logic
│   ├── UIManager.cs           # UI management
│   └── SoundManager.cs        # Audio management
├── Prefabs/                   # Game prefabs (ready for expansion)
└── Materials/                 # Materials for UI elements

ProjectSettings/               # Unity project configuration
```

## How to Play
1. Open the project in Unity 6000.1.5f1 or newer
2. Load the `TicTacToeGame` scene
3. Click Play in Unity Editor
4. Players take turns clicking empty cells
5. First player to get 3 in a row wins!
6. Click restart to play again

## Development Notes
- The game uses Unity's UI system for all interface elements
- All scripts are well-documented with clear variable names
- The code follows Unity best practices
- Easily extensible for additional features like AI, networking, or different board sizes

## Future Enhancements
- Add AI opponent
- Sound effects and background music
- Particle effects for wins
- Multiple difficulty levels
- Network multiplayer
- Different board themes
- Score tracking

## Getting Started
1. Clone this repository
2. Open in Unity 6000.1.5f1+
3. Open `Assets/Scenes/TicTacToeGame.unity`
4. Press Play to start the game

Enjoy playing Tic Tac Toe!