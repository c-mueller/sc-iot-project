Subscribing:
	topics: 
		--> room001/input/temperature
		--> room001/input/humidity
		--> room001/input/co2
		--> room001/input/particulate_matter

		--> room001/output/heater
		--> room001/output/air_conditioning
		--> room001/output/window
		--> room001/output/air_cleaner

		--> outside/dust
		--> outside/temperature

		--> core/

das core topic ist der client für kommunikation core zu MQTT; core client subscribt sich auf alle anderen topics

Mögliche Sensorwerte:
	CO2 Sensor:
		--> gemessen in ppm
		--> alles über 1000 ist schlecht
	Temperatur:
		--> gemessen in Grad
		--> 20 Grad angenehm --> 20 Grad Zielbereich --> geregelt über Heizung / Klima
		--> über 25 und unter 15 keine Fenster mehr auf
	Luftfeuchtigkeit:
		--> gemessen in %
		--> 0%..100%
		--> optimal wäre 50% für einen Büroraum
	particulate_matter:
		--> gemessen in Mikrogramm pro Kubikmeter
		--> Grenzwert: 50 Mikrogramm pro Kubikmeter

Aktuatoren mögliche Zustände:
	Belüftungsanlage(Fenster):
		--> true/false
	Heizung:
		--> true/false + Zieltemperatur
	Klimaanlage:
		--> true/false + Zieltemperatur
	Luftreiniger:
		--> true/false

MQTT_to_Core_JSON:
- rooms Array --> Erweiterung für mehrere Räume einfach möglich
	- jeder Raum mit Zimmer Nummer -->  Core kann zwischen Toilette, Küche, Büroraum ODER outside differenzieren
	- measurement Array für die verschiedenen Sensorwerte
		--> sensortyp --> dust, humidity, CO2,...
		--> timestamp nach ISO 8601 Format --> best practise für JSON
- outside für sensoren außerhalb des Gebäudes
	--> measurement Array identisch zu room measurement

MQTT_Broker_to_Actor:
- broker itself already knows, who whould receive the message 
- broker selects the receiving topics itself
- state "true/false" für Fenster, Klima, Heizung, Luftreiniger
- targetTemperatur für Klimaanlage und Heizung --> bleibt drin, ist aber unnötig

MQTT_Sensor_to_MQTTBroker:
- location--> für outside ist die ID null
- timestamp ISO 8601
- sensortyp 
- sensorwert

MQTT "Broker to Core"
wird von core/ client generiert
