from flask import Flask, json, abort
import requests
import paho.mqtt.client as mqtt

# Useful links:
# - converts curl into python code: https://curl.trillworks.com/#python
# - requests docs: https://requests.readthedocs.io/en/master/

api = Flask(__name__)
loHost = "127.0.0.1"
loPort = "8080"

exHost = "0.0.0.0"
exPort = "80"
topic = "ou44/+/+"


@api.route('/time', methods=['GET'])
def get_time():
    data = '42'
    response = requests.put('http://localhost:8001/time', data=data)
    return response.json()


@api.route('/store', methods=['GET'])
def get_store():
    data = '42'
    response = requests.put('http://localhost:8001/store', data=data)
    return response.json()


@api.route('/namespaces', methods=['GET'])
def get_namespaces():
    data = '42'
    response = requests.put('http://localhost:8001/namespaces', data=data)
    return response.json()


@api.route('/update', methods=['GET'])
def get_update():
    data = '"PREFIX brick: <https://brickschema.org/schema/1.1.0/Brick#>\\n\\nDELETE { brick:Sensor rdfs:subClassOf ?obj .} WHERE {brick:Sensor rdfs:subClassOf ?obj .}"'
    response = requests.put('http://localhost:8001/update', data=data)
    return response.json()


# Method used for making Query's to the brick instance.
# Accepts a var that corresponds with a room number.
# Currently only "e22-604-0" and "e20-604-0" (Study zones at ground level)
@api.route('/getData/<room>', methods=['GET'])
def get_room(room):
    if room == "e22-604-0" or room == "e20-604-0":
        q = \
            '''
            SELECT DISTINCT ?type ?sensor ?room ?roomname
            WHERE {
                ?room   rdf:type/rdfs:subClassOf* brick:Room .
                ?room   rdfs:label ?roomname .
                ?room   bf:hasPoint ?sensor .
                ?room   rdfs:label \\"''' + room + '''\\" .
                ?sensor rdf:type/rdfs:subClassOf*  brick:Sensor .
                ?sensor   rdf:type    ?type .
            }
            '''
        # data = '"SELECT ?pred ?obj WHERE {brick:Sensor ?pred ?obj .}"'
        data = '"' + q.strip().replace("\r", " ").replace("\n", " ") + '"'
        # Used to find errors in the call
        # print(q.strip().replace("\r", " ").replace("\n", " ")[:240])
        response = requests.put('http://localhost:8001/query', data=data)
        return response.json()
    else:
        abort(400, "Abort error 400 - The given parameter does not exist. Supported: e22-604-0 or e20-604-0")


# 0: Connection successful
# 1: Connection refused – incorrect protocol version
# 2: Connection refused – invalid client identifier
# 3: Connection refused – server unavailable
# 4: Connection refused – bad username or password
# 5: Connection refused – not authorised
# 6-255: Currently unused.
def on_connect(client, userdata, flags, rc):
    # if rc == 0:
    #    print("connected OK Returned code=", rc)
    if rc != 0:
        print("Bad connection Returned code=", rc)
    else:
        client.subscribe(topic)


def on_message(client, userdata, msg):
    # print("Topic: " + msg.topic + " MSG: " + str(msg.payload))
    global goData
    goData = {"topic": msg.topic, "sample": msg.payload.decode('utf8').replace("'", '"')}
    # print("message received  ", str(msg.payload.decode("utf-8")), \
    #      "topic", msg.topic, "retained ", msg.retain)
    # if msg.retain == 1:
    #    print("This is a retained message")


# https://stackoverflow.com/questions/12232304/how-to-implement-server-push-in-flask-framework
@api.route('/go', methods=['GET'])
def get_go():
    print(goData)
    s = json.dumps(goData, indent=4, sort_keys=True)
    return s


if __name__ == '__main__':
    # Start mqtt and instantiate on connect/message for the broker, then try to connect and run loop
    client = mqtt.Client("p1")
    client.on_connect = on_connect
    client.on_message = on_message
    client.connect("127.0.0.1", 1883, 60)
    client.loop_start()

    # Run the flask server with given parameters
    api.run(debug=True, host=loHost)
    # app.run(host=exHost, port=exPort, debug=False) #For running on remote server
