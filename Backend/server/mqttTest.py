import paho.mqtt.client as mqtt


# 0: Connection successful
# 1: Connection refused – incorrect protocol version
# 2: Connection refused – invalid client identifier
# 3: Connection refused – server unavailable
# 4: Connection refused – bad username or password
# 5: Connection refused – not authorised
# 6-255: Currently unused.
def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("connected OK Returned code=", rc)
    else:
        print("Bad connection Returned code=", rc)

    client.subscribe("ou44/+/+")


def on_message(client, userdata, msg):
    print("Topic: " + msg.topic + " MSG: " + str(msg.payload))
    goData = msg.payload
    # char = str(msg.payload)
    # if char == 'x':
    #    client.disconnect()


global goData

client = mqtt.Client("p1")
client.on_connect = on_connect
client.on_message = on_message
client.connect("127.0.0.1", 1883, 60)
client.loop_forever()
