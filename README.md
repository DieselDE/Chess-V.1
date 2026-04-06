# ♟️ Chess V.1

A chess game built in Unity using C#. This is a full implementation of the classic board game, including piece movement, game logic, audio, and custom shaders.

> ⚠️ **Work in Progress** — actively under development.

---

## 🎮 About

Chess V.1 is a Unity-based chess game built from scratch. It includes custom textures, prefabs, audio, and game logic written in C#. The project focuses on correctly implementing chess rules while exploring Unity's component system.

---

## 🗂️ Project Structure

| Folder / File | Description |
|---|---|
| `Scripts/` | C# game logic — movement, rules, board state |
| `Prefabs/` | Unity prefabs for chess pieces and board elements |
| `Scenes/` | Unity scene files |
| `textures/` | Piece and board textures |
| `Audio/` | Sound effects and audio mixer |
| `TextMesh Pro/` | UI text assets |
| `ToDo` | Developer notes and planned features |

---

## 🔧 Tech Stack

- **Engine:** Unity
- **Language:** C# (73%), ShaderLab (23%), HLSL (4%)
- **UI:** TextMesh Pro

---

## ⚙️ Architecture Notes

The board state is tracked using **two separate storage systems** that must always be kept in sync:

1. `Dictionary _piecePosition` — maps board positions to piece data
2. `Tile.piece` — the Unity component on each tile holding a reference to the piece

> ⚠️ When moving a piece, **both** storage systems must be updated. Forgetting to update one will cause desync bugs.

---

## 🚀 Getting Started

### Requirements

- Unity (LTS recommended)

### Running the Project

1. Clone the repository
2. Open the project in Unity Hub
3. Open the main scene from the `Scenes/` folder
4. Press **Play**

---

## 🗺️ Roadmap

- [x] Board setup and piece prefabs
- [x] Basic piece movement
- [x] Audio integration
- [ ] Full rule enforcement (check, checkmate, en passant, castling)
- [ ] AI opponent
- [ ] V.2 planned

---

## 👤 Author

**DieselDE** (Vincent Domsta)
