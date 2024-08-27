# Technical documentation

This document contains the technical documentation of the “SprayAR” project by Jona König and David Credo. It is intended to provide an overview of the systems and components of the project and explain how they work in order to facilitate the maintenance and further development of the project. It also describes the steps required to set up the project and create a build version.

Project title: SprayAR
Unity version: 2022.3.22f1
XR stack used: Open XR, Meta Quest 3
Team members: Jona König, David Credo

## Table of contents

1. [Introduction](#introduction)
    - [Folder structure](#folder-structure)
    - [Setup and deployment](#setup-and-deployment)
2. [Basic architecture](#basic-architecture)
    - [Event bus](#event-bus)
    - [State Machine](#state-machine)
    - [OSC-Service](#osc-service)
3. [3rd-party assets](#3rd-party-assets)
    - [Sounds](#sounds)
    - [Graphics](#graphics)

## Introduction

### Folder structure

In the project, the following folders and files are relevant for further development:

- **Assets**: Contains all assets of the project, such as 3D models, textures, materials, prefabs, scripts, scenes, etc.
- **Packages**: Contains all external packages used in the project.
- **Doxyfile**: Configuration file for the documentation of the project.

### Setup and deployment

The deployment process is largely identical to the standard process in Unity. However, special steps are necessary to ensure that communication between the ESP32 and Unity via OSC works. 

First of all: The corresponding repository for the ESP32 can be found at the following link: [SprayAR-ESP32](https://github.com/InteractionEngineer/Schuetteldose)

1. find out the IP address of the Quest 3 (Settings -> WLAN -> Connection details)
2. adjust the IP address found in the source code of the ESP32
3. find out the IP address of the ESP32 (Serial Monitor displays the IP address when the ESP32 starts)
4. unity -> scene hierarchy -> OSCManager -> OSC Service -> Spraying Can IP adapt (IP address of the ESP32)
5. adjust Unity -> Scene hierarchy -> OSCManager -> OSC Service -> Spraying Can Port if necessary (default port: 9999)
6. make sure that the build target is set to Android
7. start build process

## Basic architecture

### Overview of the overall system

![System overview](./Docs/Diagrams/Systemarchitektur.png)

### Event bus

The event bus is a central element of the project to enable communication between the various components. It is based on the observer pattern and enables components to react to events without being directly dependent on each other. The event bus is implemented as a static class and can be called from anywhere in the project.

### OSC service

The OSC service enables communication between the ESP32 and Unity via OSC. It receives OSC messages from the ESP32 and forwards them to the corresponding components. It also sends OSC messages when events occur in Unity (e.g. when the color of the spraying can is filled).

Class diagram of the OSC service:
![Class diagram of the OSC service](./Docs/Diagrams/OSCClassDiagram.png)


### Spraying Can System

The Spraying Can System consists of several components: 

- **SprayCan** The SprayCan class is the main component of the system. It contains the logic for spraying as well as the relevant data (color, fill level, etc.). It also delegates work to other components: the state machine and the feedback system.
- **State machine** The state machine manages the status of the spraying can. It consists of several states that represent the different states of the spraying can. Further information on the state machine can be found in the section [State machine](#state-machine).
- **Feedback system** The feedback system is responsible for the visual and haptic feedback of the various processes of the spraying can. It consists of several components that represent the various feedbacks (e.g. filling the color or spraying, and displaying the UI).

A class diagram of the Spraying Can system is shown below:
![Class diagram of the Spraying Can System](./Docs/Diagrams/SprayingCanClassDiagram.png)

#### State Machine

The state machine pattern is used to manage the state of the spraying can. It consists of several states that represent the different states of the spraying can.

A state diagram of the spraying can state machine is shown below:
![State Diagram of Spraying Can State Machine](./Docs/Diagrams/StateMachine.png)

## 3rd party assets

The following 3rd party assets were used in the project:

### Sounds

- [Pixabay](https://pixabay.com/sound-effects/search/air-pump-62999/)
  - air-pump-62999.mp3
  - smoke-machine-spray-3-185122.mp3
  - system-notification-199277.mp3

### Graphics

- [Icons8](https://icons8.de/icons/set/battery)
  - icons8-batterie-voll-geladen-100.png
  - icons8-leere-batterie-100.png
  - icons8-volle-batterie-100.png