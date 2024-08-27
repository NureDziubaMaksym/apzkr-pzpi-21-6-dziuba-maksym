import time
import random
from machine import ADC, Pin
from umqtt.simple import MQTTClient
import network
import ujson

# Настройки MQTT
MQTT_CLIENT_ID = "micropython-emotion-demo"
MQTT_BROKER = "broker.mqttdashboard.com"
MQTT_USER = ""
MQTT_PASSWORD = ""
MQTT_TOPIC = "wokwi-emotion"

print("Connecting to WiFi", end="")
sta_if = network.WLAN(network.STA_IF)
sta_if.active(True)
sta_if.connect('Wokwi-GUEST', '')
while not sta_if.isconnected():
    print(".", end="")
    time.sleep(0.1)
print(" Connected!")

print("Connecting to MQTT server... ", end="")
client = MQTTClient(MQTT_CLIENT_ID, MQTT_BROKER, user=MQTT_USER, password=MQTT_PASSWORD)
client.connect()
print("Connected!")

bpm_pin = ADC(Pin(35))

def read_bpm():
    raw_value = bpm_pin.read()
    bpm_value = int(40 + (raw_value / 4095.0) * 130)
    return bpm_value

def generate_values():
    humidity = random.uniform(30.0, 90.0) + random.uniform(-5.0, 5.0)
    temperature_c = random.uniform(36.5, 38.0) + random.uniform(-0.5, 0.5)
    return temperature_c, humidity

def is_angry(average_temp, average_humidity, bpm_changes):
    return average_temp > 37.5 and average_humidity > 70 and max(bpm_changes) > 10

def is_sad(average_temp, average_humidity, bpm_changes):
    return 37.0 <= average_temp <= 37.5 and 50 <= average_humidity <= 70 and max(bpm_changes) <= 10

def is_boring(average_temp, average_humidity, bpm_changes):
    return average_temp < 37.0 and average_humidity < 50 and max(bpm_changes) < 5

def is_happy(average_temp, average_humidity, bpm_changes):
    return 36.5 <= average_temp <= 37.0 and 40 <= average_humidity <= 60 and 5 <= max(bpm_changes) <= 15

def determine_emotion(average_temp, average_humidity, bpm_changes):
    if is_angry(average_temp, average_humidity, bpm_changes):
        return "angry"
    elif is_sad(average_temp, average_humidity, bpm_changes):
        return "sad"
    elif is_boring(average_temp, average_humidity, bpm_changes):
        return "boring"
    elif is_happy(average_temp, average_humidity, bpm_changes):
        return "happy"
    else:
        return "neutral"

def analyze_data(data):
    avg_temp = sum([d[0] for d in data]) / len(data)
    avg_humidity = sum([d[1] for d in data]) / len(data)
    bpm_values = [d[2] for d in data]
    bpm_changes = [abs(bpm_values[i+1] - bpm_values[i]) for i in range(len(bpm_values)-1)]
    
    return determine_emotion(avg_temp, avg_humidity, bpm_changes)

def main():
    data = []
    start_time = time.time()

    while True:
        temperature_c, humidity = generate_values()
        bpm = read_bpm()

        data.append((temperature_c, humidity, bpm))
        
        if time.time() - start_time >= 5:
            dominant_emotion = analyze_data(data)
            print(f"Dominant Emotion: {dominant_emotion}")
            
            client.publish(MQTT_TOPIC, dominant_emotion)
            print(f"Sent to MQTT: {dominant_emotion}")
            
            data.clear()
            start_time = time.time()
        
        time.sleep(1)       

main()
