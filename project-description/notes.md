# Smart Cities and IoT - Autonomous Air Quality Control

## AI Planning

### States

| Ventilation | Heater | AC  | Air Purifier  | Relevant |
| ----------- | ------ | --- | ------------  | -------- |
| Off         | Off    | Off | Off           | Yes      |
| Off         | Off    | Off | On            | Yes      |
| Off         | Off    | On  | Off           | Yes      |
| Off         | Off    | On  | On            | Yes      |
| Off         | On     | Off | Off           | Yes      |
| Off         | On     | Off | On            | Yes      |
| Off         | On     | On  | Off           | No       |
| Off         | On     | On  | On            | No       |
| On          | Off    | Off | Off           | Yes      |
| On          | Off    | Off | On            | No(1)    |
| On          | Off    | On  | Off           | No(1)    |
| On          | Off    | On  | On            | No(1)    |
| On          | On     | Off | Off           | No(1)    |
| On          | On     | Off | On            | No(1)    |
| On          | On     | On  | Off           | No(1)    |
| On          | On     | On  | On            | No(1)    |

Result 7 Relevant states

- 1) If the window is open / Ventilation is active everything else should be disabled (for simplicity)

### Übergänge im Graph

![](https://notepad.krnl.eu/uploads/upload_d5a9f687809029ff2591e94e155ce23a.png)

#### Edge #1

- (Temp_in too high & Temp_out lower than upper temp threshold OR
- Temp_in too low & Temp_out higher than lower temp threshold) AND
- Humidity_out below threshold

#### Edge #2

- Temp_in is in normal range (room temperature) OR
- Temp_out higher than upper temp threshold OR
- Temp_out lower than lower temp threshold OR
- Humidity_out above threshold

#### Edge #3

- Temp_in too low AND 
- (Temp_out lower than lower temp threshold OR 
- (Temp_out higher than lower temp threshold AND Humidity_out above threshold))

#### Edge #4

- Temp_in is in normal range (room temperature) AND 
- Humidity_out below threshold (maybe)

#### Edge #5

- Temp_in too high AND 
- (Temp_out higher than upper temp threshold OR
- (Temp_out lower than upper temp threshold AND Humidity_out above threshold))

#### Edge #6

- Air purity reaches critical ppm threshold

#### Edge #7

- Air purity reaches turn off ppm treshold

#### Edge #8

- Humidity_out below threshold AND
- Air purity outside is below outside turn off ppm threshold AND
- (Temp_in too high & Temp_out lower than upper temp threshold OR
- Temp_in too low & Temp_out higher than lower temp threshold)


## Architekturentwurf

### Programmiersprache(n)

TODO

- UI: Angular + TypeScript
- Sensorsimulation (potentiell), Backend + UI: Go (+ Templating), .NET Core, Angular +  TypeScript
- Controller: 

### Sensor und Aktuator Umsetzung (Simulation)

- Microsoft Azure IoT Device Simulation (Discontinued seit 6.5.21)
- IoTIFY (garantiert viel $$$)
- Bevywise (https://www.bevywise.com) (30 Tage Testversion danach $$$)
- AWS IoT Device Simulator (vmtl. Kreditkarte und $$$)
- Irgendwas von Oracle (vmtl. viel $$$)
- Selbstcoden (1337 H4xx0r)

### Physical Layer kommunikation mit Restsystem

- Message Queue (MQTT)
- Pro Aktuator ein Topic auf den Aktuatoren subscriben
- vmtl. 1 Topic für alle Sensoren aus dem Sensordaten vom Ubiqutous Layer ausgelesen werden

### AI Planning

- AI Plan modeliert mit PDDL (Planning Domain Definition Language)
- PDDL Model in Software einbinden -> via Solver oder sowas -> komme die nächsten Wochen
- alternativ -> eigene Programmlogik

### State Info UI (Presentation Layer)

- Angular (o.ä.)
- Refresh Werte in Zeitinterval
- Admin Dashboard Template

### Wie wird das System gezeigt?

1. Theoretisches (Diagramme und bla...)
2. State Info UI
3. Sensor UI
4. MQ Sniffing
5. 

#### Extrazeug was für die Präsentation gemacht werden muss

- UI für änderung von Sensorwerten
- UI die aktuellen Ort "zeigt"
- Einige technische Werte und Sachen, die zeigen, dass das System funktioniert
- 

## Projekt Idee

Büro
Krankenhauszimmer?
Arztpraxis/Raum?

### Sensors

- Partikelsensor (Innen u. Aussen)
- CO2-Sensor (Innen)
- Temperatursensor (Innen u. Aussen)
- Luftfeuchtigkeit (Aussen)
- Zeiterkennung


### Actuators

- Belüftungsanlage (Fenster)
- Heizung
- Klimaanlage
- Luftreiniger


### Functional Requirements

#### FA-1
- Name: FA-1
- Title: Autonomous temperature regulation
- Description: The system should be able to regulate the temperature of the room based on system information and human presence
- Dependencies: FA-8 FA-10
- Priority: +


#### FA-2
- Name: FA-2
- Title: Autonomous humidity regulation
- Description: The system should be able to regulate the humidity of the room based on system information and human presence
- Dependencies: FA-9 FA-10
- Priority: +

#### FA-3
- Name: FA-3
- Title: Autonomous air quality regulation
- Description: The system should be able to regulate the air quiality of the room based on system information and human presence. Particle count (Particulate matter) and $CO_2$ concentration are the basis for measuring the air quality.
- Dependencies: FA-6 FA-7 FA-10
- Priority: ++

#### FA-4
- Name: FA-4
- Title: Night regulation
- Description: The System should reduce its functionality to a minimum during night time.
- Dependencies: FA-1 FA-2 FA-3
- Priority: 0

#### FA-5
- Name: FA-5
- Title: Display system activity to User
- Description: The System should be able to show its activity and monitoring information to the user
- Dependencies: Everything else
- Priority: --

#### FA-6
- Name: FA-6
- Title: Recognize stale air
- Description: The System should be able to recognize when the air has gone stale. This is based on the $CO_2$ concentration indoors.
- Dependencies: None
- Priority: ++

#### FA-7
- Name: FA-7
- Title: Recognize increased particulate matter concentration
- Description: The System should be able to recognize when the air contains an increased concentration of particulate matter.
- Dependencies: None
- Priority: ++

#### FA-8
- Name: FA-8
- Title: Monitor indoor and outdoor temperature
- Description: The System should be able to recognize the temperature indoors and outdoors of its operating location.
- Dependencies: None
- Priority: ++

#### FA-9
- Name: FA-9
- Title: Monitor indoor and outdoor humidity
- Description: The System should be able to recognize the humidity indoors and outdoors of its operating location.
- Dependencies: None
- Priority: ++

#### FA-10
- Name: FA-10
- Title: Detect human presence
- Description: The System should be able to detect human presence indoors.
- Dependencies: None
- Priority: -


### Non-functional Requirements

#### NFA-1

- ID: NFA-1
- Title: Realworld application
- Description: The system should be able to be easily transformed to a real world running smart home system.
- Dependencies: None
- Priority: -

#### NFA-2

- ID: NFA-2
- Title: Automation with minimal user intervention
- Description: The system should utilze AI-planning in order to automate its autonomous functionalities with minimal user intervention.
- Dependencies: None
- Priority: ++

#### NFA-3

- ID: NFA-3
- Title: Loosely coupled system
- Description: The system should be loosely coupled regarding its communication between devices.
- Dependencies: None
- Priority: ++

#### NFA-4

- ID: NFA-4
- Title: Distributed System
- Description: The system should be distributed across multiple (at least two) devices.
- Dependencies: None
- Priority: ++


## System Design

- Edge Devices (Physical)
    - Senors
    - Actuators
- Gateway & Auswertungslogik für Sensoren (Ubiquitous)
    - Recieves sensor data and evaluates it
    - evaluated data will be passed on to data repository
    - recieved actuator instructions from AI Planning component
    - translates them and sends commands to actuators
- Data Repository (Reasoning)
    - accumulative repository for information put into the system
    - knowledge base for AI planning
    - only recieves and stores information
- AI Planning component (Reasoning)
    - takes information from data repository
    - decides on actions that should be taken
    - handles AI planning
    - instructs actuators through gateway
    - instructs UI on what to show
- User Interface (Presentation)
    - Recieves user input from visual GUI
    - evaluates user input and passes it on to the data repository
    - recieves data from AI planning component to visualize for the user
        - sollte vlt. neue componente machen
