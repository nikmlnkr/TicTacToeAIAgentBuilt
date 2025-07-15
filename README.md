# 🎮 Unity Tic Tac Toe Game

A complete, ready-to-play tic-tac-toe game built with Unity 2022.3 LTS. This project demonstrates clean architecture, event-driven programming, and modern Unity UI practices.

## ✨ Features

- **Complete Game Logic**: Full tic-tac-toe implementation with win detection and draw handling
- **Modern UI**: Clean, responsive interface using Unity's UI Toolkit and TextMeshPro
- **Visual Feedback**: Winning line highlighting and interactive button states
- **Score Tracking**: Persistent score display across multiple games
- **Event-Driven Architecture**: Decoupled game logic using C# events
- **Mobile-Ready**: UI designed to work on various screen sizes

## 🛠️ Unity Packages & Dependencies

This project uses the following Unity packages (configured in `Packages/manifest.json`):

### Core Packages
- **Unity UI (com.unity.ugui: 1.0.0)** - Essential for Canvas, Button, and Image components
- **Input System (com.unity.inputsystem: 1.7.0)** - Modern input handling system
- **TextMeshPro (com.unity.textmeshpro: 3.0.6)** - Enhanced text rendering for better UI typography

### Development Packages
- **Universal Render Pipeline (com.unity.render-pipelines.universal: 14.0.8)** - Modern rendering pipeline
- **Visual Scripting (com.unity.visualscripting: 1.9.0)** - Node-based visual scripting support
- **Timeline (com.unity.timeline: 1.7.5)** - Animation and cutscene system

### IDE Support
- **Visual Studio Code (com.unity.ide.vscode: 1.2.5)** - VS Code integration
- **Visual Studio (com.unity.ide.visualstudio: 2.0.18)** - Visual Studio integration
- **Rider (com.unity.ide.rider: 3.0.24)** - JetBrains Rider integration

### Testing & Development
- **Test Runner (com.unity.test-runner: 1.1.33)** - Unit testing framework
- **Collaboration Proxy (com.unity.collab-proxy: 2.0.5)** - Version control support

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── TicTacToeGame.cs          # Core game logic and state management
│   ├── TicTacToeCell.cs          # Individual cell behavior
│   └── GameManager.cs            # UI coordination and game flow
├── Scenes/
│   └── TicTacToeGame.unity       # Main game scene
├── Prefabs/
│   └── CellPrefab.prefab         # Reusable cell UI prefab
└── Materials/                     # (Reserved for future use)

Packages/
└── manifest.json                 # Package dependencies

ProjectSettings/
└── ProjectVersion.txt            # Unity version information
```

## 🎯 Game Architecture

### Core Components

1. **TicTacToeGame** - Pure game logic class
   - Manages game state and board
   - Implements win/draw detection
   - Provides events for UI updates

2. **TicTacToeCell** - Individual cell component
   - Handles user input
   - Manages visual state (X, O, empty)
   - Provides click feedback

3. **GameManager** - Main game coordinator
   - Connects game logic to UI
   - Manages score tracking
   - Handles game flow and animations

### Key Features

- **Event-Driven**: Uses C# events for clean separation between game logic and UI
- **Modular Design**: Each component has a single responsibility
- **Extensible**: Easy to add new features like AI players or different board sizes
- **Type-Safe**: Strong typing with enums for game states and cell states

## 🚀 Getting Started

### Prerequisites
- Unity 2022.3 LTS or newer
- Git (for cloning the repository)

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd <repository-name>
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open" and select the project folder
   - Unity will automatically resolve package dependencies

3. **Load the Main Scene**
   - Navigate to `Assets/Scenes/`
   - Double-click `TicTacToeGame.unity`

4. **Play the Game**
   - Click the Play button in Unity Editor
   - The game should start immediately

### Building for Platforms

The project is configured to build for multiple platforms:

- **Standalone (Windows/Mac/Linux)**
- **WebGL** (for web deployment)
- **Mobile (iOS/Android)** (with responsive UI)

## 🎮 How to Play

1. **Start the Game**: The game begins automatically with Player X's turn
2. **Make Moves**: Click on empty cells to place your mark (X or O)
3. **Win Condition**: Get three marks in a row (horizontal, vertical, or diagonal)
4. **New Game**: Click "New Game" button to start fresh
5. **Score Tracking**: Scores are displayed and updated automatically

## 🔧 Customization

### Modifying Game Rules
- Edit `TicTacToeGame.cs` to change win conditions or board size
- Adjust `CheckGameEnd()` method for different victory patterns

### UI Customization
- Modify `CellPrefab.prefab` to change cell appearance
- Edit colors and fonts in the GameManager component
- Adjust layout in the main scene

### Adding Features
- **AI Player**: Extend `GameManager` to include computer player logic
- **Different Board Sizes**: Modify the grid generation in `GameManager`
- **Sound Effects**: Add AudioSource components and sound clips
- **Animations**: Use Unity's Animation system for enhanced visual feedback

## 🐛 Troubleshooting

### Common Issues

1. **Missing Packages**: If packages fail to resolve, check internet connection and Unity version
2. **Script Compilation Errors**: Ensure all script files are in the correct namespace
3. **UI Not Responsive**: Verify Canvas Scaler settings for different screen sizes

### Performance Tips

- The game is lightweight and should run smoothly on most devices
- For mobile deployment, consider reducing UI complexity for older devices
- Use Unity Profiler to monitor performance if needed

## 📝 Development Notes

### Design Decisions

1. **Separation of Concerns**: Game logic is completely separate from UI, making it easy to test and modify
2. **Event-Driven Architecture**: Reduces coupling between components
3. **Unity Best Practices**: Follows Unity coding conventions and component patterns
4. **Scalability**: Architecture supports adding more complex features

### Future Enhancements

- [ ] AI opponent with difficulty levels
- [ ] Online multiplayer support
- [ ] Different board sizes (4x4, 5x5)
- [ ] Custom themes and skins
- [ ] Sound effects and music
- [ ] Save/load game state
- [ ] Statistics and achievements

## 📄 License

This project is provided as-is for educational and demonstration purposes. Feel free to use, modify, and distribute according to your needs.

## 🤝 Contributing

Feel free to submit issues and enhancement requests. Contributions are welcome!

---

**Built with Unity 2022.3 LTS** 🎮