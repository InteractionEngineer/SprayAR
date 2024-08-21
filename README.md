# Technische Dokumentation

Dieses Dokument beinhaltet die technische Dokumentation des Projektes "SprayAR" von Jona König und David Credo. Es soll einen Überblick über die Systeme und Komponenten des Projektes geben und deren Funktionsweisen erläutern, um so die Wartung und Weiterentwicklung des Projektes zu erleichtern. Ferner werden die notwendigen Schritte zur Einrichtung des Projektes und zur Erstellung einer Build-Version beschrieben.

Projekttitel: SprayAR
Unity Version: 2022.3.17f1 **Nochmal prüfen!**
Verwendete AR/VR Hardware: Open XR, Meta Quest 3
Teammitglieder: Jona König, David Credo

## Inhaltsverzeichnis

1. [Einleitung](#einleitung)
    - [Ordnerstruktur](#ordnerstruktur)
    - [Einrichtung und Deployment](#einrichtung-und-deployment)
2. [Grundlegende Architektur](#grundlegende-architektur)
    - [Event-Bus](#event-bus)
    - [State Machine](#state-machine)
    - [OSC-Service](#osc-service)
3. [Eigenleistungen und 3rd-Party-Assets](#eigenleistungen-und-3rd-party-assets)
    - [Eigenleistungen](#eigenleistungen)
    - [3rd-Party-Assets](#3rd-party-assets)
4. [Teilsysteme und Komponenten (UML)](#teilsysteme-und-komponenten-uml)
    - [State Diagramm der Spraying Can State Machine](#state-diagramm-der-spraying-can-state-machine)
    - [Klassendiagramm des OSC-Service](#klassendiagramm-des-osc-service)
    - [Klassendiagramm des Spraying Can Systems](#klassendiagramm-des-spraying-can-systems)

## Einleitung

### Ordnerstruktur

Im Projekt sind folgende Ordner und Dateien für die Weiterentwicklung relevant:

- **Assets**: Enthält alle Assets des Projektes, wie 3D-Modelle, Texturen, Materialien, Prefabs, Skripte, Szenen, etc.
- **Packages**: Enthält alle externen Packages, die im Projekt verwendet werden.
- **Doxyfile**: Konfigurationsdatei für die Dokumentation des Projektes.

### Einrichtung und Deployment

Der Deployment-Prozess ist weitestgehend identisch zum Standardprozess in Unity. Es sind jedoch spezielle Schritte notwendig, damit die Kommunikation mittels OSC zwischen dem ESP32 und Unity funktioniert. 

1. IP Adresse der Quest 3 herausfinden (Einstellungen -> WLAN -> Verbindungsdetails)
2. Gefundene IP Adresse im Quellcode des ESP32 anpassen
3. IP Adresse des ESP32 herausfinden (Serial Monitor gibt die IP Adresse aus, wenn der ESP32 startet)
4. Unity -> Szenenhierarchie -> OSCManager -> OSC Service -> Spraying Can IP anpassen (IP Adresse des ESP32)
5. Unity -> Szenenhierarchie -> OSCManager -> OSC Service -> Spraying Can Port ggf. anpassen (Standardport: 9999)
6. Sicherstellen, dass das Build-Target auf Android steht
7. Build-Prozess starten

## Grundlegende Architektur

### Event-Bus

Der Event-Bus ist ein zentrales Element des Projektes, um die Kommunikation zwischen den verschiedenen Komponenten zu ermöglichen. Er basiert auf dem Observer-Pattern und ermöglicht es, dass Komponenten auf Ereignisse reagieren können, ohne direkt voneinander abhängig zu sein. Der Event-Bus ist als statische Klasse implementiert und kann von überall im Projekt aufgerufen werden.

### State Machine

Das State Machine Pattern wird verwendet, um den Zustand der Spraying Can zu verwalten. Sie besteht aus mehreren States, die die verschiedenen Zustände der Spraying Can repräsentieren.

Nachfolgend ist ein State-Diagramm der Spraying Can State Machine dargestellt:
![State Diagramm der Spraying Can State Machine](./Docs/UML/StateMachine.png)

### OSC-Service

Der OSC-Service ermöglicht die Kommunikation zwischen dem ESP32 und Unity mittels OSC. Der OSC-Service empfängt OSC-Nachrichten vom ESP32 und leitet sie an die entsprechenden Komponenten weiter. Ebenso sendet der OSC-Service OSC-Nachrichten, wenn Ereignisse in Unity auftreten (Z.B. wenn die Farbe der Spraying Can aufgefüllt wird).

