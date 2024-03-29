from flask import Flask, json, abort
import requests
import paho.mqtt.client as mqtt

# Useful links:
# - converts curl into python code: https://curl.trillworks.com/#python
# - requests docs: https://requests.readthedocs.io/en/master/

api = Flask(__name__)
loHost = "127.0.0.1"
loPort = "5000"

# host="0.0.0.0" will make the page accessible
# by going to http://[ip]:5000/ on any computer in
# the network.
exHost = "0.0.0.0"
exPort = "80"

# Topic is used to subscribe to a specific subject from the mqtt broker.
topic = "ou44/+/+"

# Array used for storing MQTT data; it is updated when new values are provided
dataList = []

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

        #print(q)
        # data = '"SELECT ?pred ?obj WHERE {brick:Sensor ?pred ?obj .}"'
        data = '"' + q.strip().replace("\r", " ").replace("\n", " ") + '"'
        # Used to find errors in the call
        # print(q.strip().replace("\r", " ").replace("\n", " ")[:240])
        response = requests.put('http://localhost:8001/query', data=data)

        json_object = response.json()
        #print(json_object)

        sensorlist = []
        for x in json_object['resultset']:
            data = {}
            # ----------- Sensor type -----------
            text = x[0]
            text2 = text[text.find('#'):]
            sensortype = text2[1:]
            print(sensortype)
            # ----------- uuids -----------
            uuid = x[1]
            uuid2 = uuid[uuid.find('#'):]
            uuid3 = uuid2[8:]
            # ----------- Room number -----------
            room = x[3]
            # ----------- add to list -----------
            data['sensortype'] = sensortype
            data['uuid'] = uuid3
            data['room'] = room
            sensorlist.append(data)

        brick_list = {'sensors': sensorlist}
        return_list = json.dumps(brick_list, indent=4, sort_keys=True)

        is_valid = validateJSON(return_list)
        print("Given JSON string is ", is_valid)

        #return response.json()
        return return_list
    else:
        abort(400, "Abort error 400 - The given parameter does not exist. Supported: e22-604-0 or e20-604-0")


def validateJSON(jsonData):
    try:
        json.loads(jsonData)
    except ValueError as err:
        return False
    return True


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
    global datathing
    data = {}
    goData = {"topic": msg.topic, "sample": msg.payload.decode('utf8').replace("'", '"')}

    res = json.loads(msg.payload.decode('utf8').replace("'", '"'))
    res['topic'] = msg.topic

    if len(dataList) == 0:
        dataList.append(res)
    elif res not in dataList:
        print(id(res))
        present = False
        for i, item in enumerate(dataList):
            if res['topic'] == item['topic']:
                print("Updated Values in: " + res['uuid'])
                dataList[i] = res
                present = True
        if not present:
            print("added the sensor: " + res['uuid'])
            print(id(res))
            dataList.append(res)




    #print(dataList)


    # print("message received  ", str(msg.payload.decode("utf-8")), \
    #      "topic", msg.topic, "retained ", msg.retain)
    # if msg.retain == 1:
    #    print("This is a retained message")


# https://stackoverflow.com/questions/12232304/how-to-implement-server-push-in-flask-framework
@api.route('/go', methods=['GET'])
def get_go():
    #print(goData)
    #s = json.dumps(goData, indent=4, sort_keys=True)
    finalList = {'livedata': dataList}
    s = json.dumps(finalList, indent=4, sort_keys=True)


    #print(goData)
    return finalList



@api.route('/')
def index():
    return 'OK: Connection is on'


if __name__ == '__main__':
    # Start mqtt and instantiate on connect/message for the broker, then try to connect and run loop
    client = mqtt.Client("p1")
    client.on_connect = on_connect
    client.on_message = on_message
    client.connect("127.0.0.1", 1883, 60)
    client.loop_start()

    # Run the flask server with given parameters
    #api.run(debug=True, host=loHost, port=loPort)
    api.run(host=exHost, port=exPort, debug=True, threaded=True) #For running on remote server (
