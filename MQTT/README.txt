Subscribing:
	topics: 
		--> room001/input/temperature
		--> room001/input/humidity
		--> room001/input/co2
		--> room001/input/dust

		--> room001/output/heater
		--> room001/output/air_conditioning
		--> room001/output/window
		--> room001/output/air_cleaner

		--> outside/dust
		--> outside/temperature

Mögliche Sensorwerte:
	CO2 Sensor:
		--> gemessen in ppm
		--> alles über 1000 ist schlecht
	Temperatur:
		--> gemessen in Grad
		--> 20 Grad angenehm --> 20 Grad Zielbereich --> geregelt über Heizung / Klima
	Luftfeuchtigkeit:
		--> gemessen in %
		--> 0%..100%
		--> optimal wäre 50% für einen Büroraum
	Staubsensor:
		--> gemessen in Mikrogramm pro Kubikmeter
		--> Grenzwert: 50 Mikrogramm pro Kubikmeter

Aktuatoren mögliche Zustände:
	Belüftungsanlage(Fenster):
		--> offen / geschlossen
	Heizung:
		--> an/aus + Zieltemperatur?
	Klimaanlage:
		--> an/aus + Zieltemperatur
	Luftreiniger:
		--> an/aus

MQTT_to_Core_JSON:
- rooms Array --> Erweiterung für mehrere Räume einfach möglich
	- jeder Raum mit Zimmer Nummer --> einfache Zuordnung der Sensorwerte im Core
	- jedes Room Array Element mit typ --> Core kann zwischen Toilette, Küche, Büroraum differenzieren
	- measurement Array für die verschiedenen Sensorwerte
		--> sensortyp --> dust, humidity, CO2,...
		--> IDEE: alle values immer als String speichern? --> geparst wird im core --> dann speichern in DB
		--> timestamp nach ISO 8601 Format --> best practise für JSON
- outside für sensoren außerhalb des Gebäudes
	--> measurement Array identisch zu room measurement

MQTT_to_Actor:
- json nicht unbedingt nötig. 

MQTT_Sensor_to_MQTTBroker: