

# 🚁 **Drone Rescue Mission: Echoes in the Frost**

🔹 *Drone Rescue Mission: Echoes in the Frost* is a Unity-based action-adventure game that showcases advanced game programming techniques.
Players pilot a rescue drone through a dynamically generated blizzard-covered mountain range to locate and save three stranded characters—**Steve**, **Pete**, and **Kate**—by dropping life-saving first aid kits.
The project delivers a thrilling and immersive gameplay experience through **realistic drone mechanics**, **interactive characters**, **visually rich particle effects**, and a fully integrated **HUD system**.

---

## 📑 **Table of Contents**

* 📌 [Project Overview](#-project-overview)
* 🚀 [Key Features](#-key-features)
* 💾 [Installation](#-installation)
* ▶️ [Running the Executable](#-running-the-executable)
* 🎮 [Gameplay Instructions](#-gameplay-instructions)
* 💻 [Scripts Overview](#-scripts-overview)
* ✅ [Requirements Compliance](#-requirements-compliance)
* ⚙️ [Setup Guide](#-setup-guide)
* 🧩 [Dependencies](#-dependencies)
* 🤝 [Contributing](#-contributing)
* 📜 [License](#-license)
* 🙌 [Acknowledgments](#-acknowledgments)

---

## 📌 **Project Overview**

🧊 *Echoes in the Frost* tasks players with navigating hostile snowy terrain as a drone pilot.
Your mission: **locate and assist three survivors** by deploying kits and avoiding environmental dangers.
Unity is used to dynamically render terrain and particle effects in real-time, with intelligent behavior and animation systems for characters.

---

## 🚀 **Key Features**

* 🏔️ **Procedural Terrain** – Height-map based mountainous regions with randomized snow layers.
* 🚁 **Drone Mechanics** – Realistic lift, yaw, pitch, and movement using physics-based inputs.
* 👤 **Character Behavior** – Animated survivors with interactive gestures and root motion.
* 🧰 **First Aid Kit Physics** – Kits inherit drone velocity; destroyed if velocity > 8 m/s.
* ❄️ **Dynamic Particle Effects** – Snow, fog during flight, fire after crashes.
* 🧭 **HUD System** – Real-time display of drone health, character status, and distance.
* 🔊 **Immersive Audio** – Dynamic propeller sound, blizzard ambiance, background music.
* 🏆 **Game Flow** – Win by rescuing all characters; lose after 3 high-velocity crashes.

---

## 💾 **Installation**

1. 🧬 Clone the repository:

   ```bash
   git clone https://github.com/ZeyadArafa/drone-rescue-mission.git
   ```
2. 📂 Open the project in **Unity 2021.3+**
3. 🧱 Import provided assets (terrain, skybox, characters, etc.)
4. ⚙️ Configure scene and references via **Inspector**
5. 🚀 Build or run in Unity Editor

---

## ▶️ **Running the Executable**

📥 **Download** the `.exe` and accompanying `*_Data` folder from the **Releases** section.

💻 **System Requirements**:

* OS: Windows 10+
* CPU: Intel i5 or equivalent
* RAM: 8 GB+
* GPU: DirectX 11 compatible
* Storage: \~1 GB

🧩 **Launch**:

* Place `.exe` and `*_Data` folder in the same directory
* Double-click to start

🔧 **Troubleshooting**:

* Update GPU drivers
* Install DirectX / Visual C++ Redistributables
* Check Unity logs for errors

---

## 🎮 **Gameplay Instructions**

🎯 **Objective**: Rescue **Steve**, **Pete**, and **Kate** by dropping first aid kits within 10 meters.

⌨️ **Controls**:

* `W/A/S/D`: Move
* `Space`: Ascend (Lift)
* `Q/E`: Yaw (Rotate)
* `Z/C`: Pitch (Tilt)
* `M/N`: Adjust lift force (5–25)
* `Enter`: Drop kit
* `F1`: Toggle HUD

🛠️ **Mechanics**:

* Characters wave when drone is ≤ 50m
* Drop kits ≤ 10m for interaction
* Collisions > 3 m/s = damage (3 hits = crash)

🏆 **Win**: All rescued → *“Mission Accomplished!”*
❌ **Lose**: Crash after 3 hits → *“Mission Failed!”*

---

## 💻 **Scripts Overview**

* 🎵 `AudioManager.cs` – Manages all in-game sounds
* 🧍 `CharacterBehavior.cs` – Controls animation and responses
* 🧑‍💻 `CharacterProfileHUD.cs` – HUD for rescued character display
* 👥 `CharacterSpawner.cs` – Spawns characters on safe terrain
* 📏 `DistanceDisplay.cs` – Shows nearest unsaved character distance
* 💔 `DroneHealth.cs` – Tracks drone hits and crash effects
* 🧰 `FirstAidKit.cs` – Manages destruction on high impact
* 🎯 `FirstAidKitDropper.cs` – Handles kit-dropping logic
* 🎮 `GameManager.cs` – Monitors win/lose state
* ❄️ `ParticleEffectsManager.cs` – Activates snow, fog, fire VFX
* 🔄 `PropellerController.cs` – Spins propellers, syncs with lift
* 🌄 `TerrainTextureRandomizer.cs` – Randomizes terrain textures

---

## ✅ **Requirements Compliance**

✔️ Meets all criteria:

* 🎨 Randomized terrain with 80% snow
* 🌌 Configured skybox
* 🎥 Orbital camera with flare and recentering
* 🛸 Drone setup with Move.cs & physics settings
* 🎮 Full control scheme implemented
* 🧍 Character spawning with root motion
* 💡 Visual and audio feedback for every state
* ✅ Bonus: Character Profile HUD + Sound Integration

---

## ⚙️ **Setup Guide**

🗺️ **Scene Setup**:

* Import terrain, apply skybox, position camera and drone
  🧾 **Inspector Configuration**:
* Assign components (scripts, kits, audio, UI)
  📦 **Assets**:
* Characters, kits, sounds, skybox, terrain textures
  🔍 **Testing**:
* Validate logic in Unity Editor and check console logs

---

## 🧩 **Dependencies**

* 🧠 Unity 2021.3+
* 🆎 TextMeshPro (UI text)
* 📁 External Assets:

  * Terrain height map
  * Snow & mountain textures
  * Character prefabs & animations
  * Drone prefab (with propellers)
  * First aid kit prefab
  * Snow, fog, fire particle systems
  * Skybox material
  * Audio clips

---

## 🤝 **Contributing**

1. 🍴 Fork the repository
2. 🌿 Create a feature branch
3. 💬 Commit your changes
4. 🚀 Push to GitHub
5. 📥 Submit a Pull Request

📌 Follow Unity C# best practices and include helpful comments.

---

## 📜 **License**

🔓 This project is licensed under the **MIT License**. See `LICENSE` for full details.

---

## 🙌 **Acknowledgments**

* 🎨 **Assets**: Provided prefabs, sounds, particles, and visuals
* 💡 **Inspiration**: Survival adventure themes reimagined with Unity

---

## 🚁 **Drone Rescue Mission: Echoes in the Frost**

🎮 *Fly with precision. Rescue with purpose. Conquer the storm with Unity mastery.* ❄️

---
