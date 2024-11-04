![Image](https://i.imgur.com/PFfxdfm.jpg)

# AR Slingshot Game 🎯

Welcome to the **AR Slingshot Game**! This project introduces you to the immersive world of Augmented Reality (AR) by blending digital interactions with real-world environments. Inspired by games like **Pokémon GO** and **AR HORSE**, this interactive experience allows you to detect horizontal planes (such as tables or floors), spawn moving targets, and launch projectiles to score points.

---

## 📜 Project Overview

In this AR game, players select a real-world surface as their play area, upon which they can aim and launch projectiles at dynamically moving targets. This project serves as a foundational model for building interactive AR games and provides valuable hands-on experience with **Unity’s AR Foundation**, **ARKit**, and **ARCore**.

---

## 🔧 Technical Focus

### Key Learning Areas
This project covers three core areas:

1. **AR Fundamentals**  
   - Learn the basics of ARKit, ARCore, and ARFoundation.
   - Implement plane detection and anchoring in AR.

2. **AR Game Design**  
   - Design responsive UI for various mobile devices.
   - Develop AR interactions and mobile AR application deployment for both Android and iOS.

3. **AR Game Mechanics**  
   - Create a game mechanic that includes spawning moving targets, launching projectiles, and scoring.

By completing this project, you’ll understand the end-to-end flow of an AR game, from plane detection to interactive gameplay.

---

## 📂 Project Structure and Requirements

- **Unity Version**: Unity 2022 LTS
- **AR Plugins**: AR Foundation, ARKit XR Plugin, ARCore XR Plugin
- **Build Support**: Both Android and iOS platforms

### Setup Instructions

1. **Install Packages**  
   - Open Unity’s Package Manager and install:
     - AR Foundation
     - ARKit XR Plugin
     - ARCore XR Plugin
     - XR Plugin Management

2. **Scene Setup**  
   - Create a scene named `ARSlingshotGame`.
   - Add an **AR Session** and **AR Camera**.
   - Create an **AR Plane Manager** with the AR Plane Manager and AR Session Origin components to enable horizontal plane detection.

---

## 🎮 Gameplay Mechanics

### Plane Selection
- **Player Interaction**: Tap on a detected plane to select it.
- **Selected Plane**: Only one plane can be saved and used for gameplay, while others are discarded.
- **UI Prompt**: A **Start** button appears when a plane is selected.

### Target Spawning
- **Prefab**: A `Target` Capsule is instantiated as a prefab.
- **Positioning**: Targets are positioned on the selected plane and scale based on distance from the camera.
- **Movement**: Targets move randomly within the plane’s X/Z boundaries.

### Slingshot Mechanics
- **Projectile (Ammo)**: A Sphere named `Ammo` acts as the projectile.
- **Launch Controls**: Drag to aim and release to launch; the drag length determines launch force.
- **Gravity and Reset**: Ammo is affected by gravity and resets when it hits a target, plane, or goes out of bounds.

### UI Layout
- **Buttons**:
  - **Start**: Starts the game and initializes Ammo.
  - **Restart**: Resets the plane, score, Ammo, and targets.
  - **Quit**: Exits the game.
- **Score**: Updates based on targets hit; point value can vary with distance.
- **Ammo Counter**: Displays remaining Ammo, starting with 7.

### Replay Option
- **PlayAgainButton**: Appears when Ammo is depleted or all targets are eliminated, allowing the player to replay without changing the selected plane.

---

## 🔍 Tasks and Objectives

1. **Detect and Anchor Planes**
   - Detect horizontal planes, provide visual feedback, and anchor the detected plane.

2. **Select and Save Plane**
   - Enable plane selection, save the chosen plane, and discard others. Display a Start button upon selection.

3. **Spawn and Move Targets**
   - Create moving targets on the selected plane with constraints for scale and movement direction.

4. **Slingshot Functionality**
   - Implement drag-to-launch mechanics, including gravity, launch force based on drag length, and reset conditions.

5. **Responsive UI Setup**
   - Design a vertically aligned UI with buttons and counters that scales to various resolutions.

6. **Replay Mechanics**
   - Add a replay button to restart the game, retaining the selected plane.

7. **Trajectory Indicator**
   - Use a LineRenderer to visualize the launch trajectory for the Ammo.

8. **Build and Test**
   - Build the project for Android and iOS. Create a `.zip` of each build for distribution.

---

## 📚 Resources

### Core
- **Basics of AR**: Anchors, Keypoints, Feature Detection
- **Basics of AR**: SLAM

### Suggested
- [AR Foundation Documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.0/manual/index.html)
- [Augmented Reality Design Guidelines](https://developer.apple.com/design/human-interface-guidelines/augmented-reality/overview/)

### Supplementary
- **Designing UI for Multiple Resolutions**
- **Unity Manual**: Optimization tips

---

## 🎯 What You’ll Learn

By completing this project, you will be able to explain:
- What ARKit and ARCore are and how ARFoundation supports both
- The basics of plane detection in AR and its implementation
- How to publish AR applications for both Android and iOS
- How to create responsive, adaptive UI for mobile devices in an AR context
- The concept and importance of SLAM in AR technology

---

## 📁 Repository Structure

- **GitHub Repository**: [atlas-unity](https://github.com/Bmontezuma/atlas-unity)
- **Directory**: `unity-ar_slingshot_game`
- **Project Assets**: Stored in `Assets/`

---

## 📦 Build and Deploy

To share your AR game:

1. **Create Builds**:
   - **iOS Build**: Export as `unity-ar_slingshot_game-iOS.zip`
   - **Android Build**: Export as `unity-ar_slingshot_game-Android.zip`

2. **Upload Builds**: Share via Google Drive or Dropbox. 

Add download links here:

- iOS Build: [Link to iOS .zip](#)
- Android Build: [Link to Android .zip](#)

---

Happy coding! 😊 Enjoy exploring the world of AR!

# ***Special Thanks To***
[Sound Effect by Luca Di Alessandro from Pixabay](https://pixabay.com/users/lucadialessandro-25927643/)

[Sound Effect from Pixabay](https://pixabay.com/sound-effects/search/start%20game%20button%20sounds/)

***"GAME OVER!"***
[Sound Effect by Ivan Luzan from Pixabay](https://pixabay.com/users/ivan_luzan-34614814/)

***"Restart!"***
[Sound Effect from Pixabay](https://pixabay.com/sound-effects/search/restart%20game%20jingle/)
