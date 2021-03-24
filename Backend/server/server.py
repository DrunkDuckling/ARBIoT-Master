from flask import Flask, json
import requests

# Useful links:
# - converts curl into python code: https://curl.trillworks.com/#python
# - requests docs: https://requests.readthedocs.io/en/master/

companies = [{"id": 1, "name": "Company One"}, {"id": 2, "name": "Company Two"}]

api = Flask(__name__)
loHost = "127.0.0.1"
loPort = "8080"

exHost = "0.0.0.0"
exPort = "80"


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


@api.route('/query', methods=['GET'])
def get_query():
    data = '"SELECT ?pred ?obj WHERE {brick:Sensor ?pred ?obj .}"'
    response = requests.put('http://localhost:8001/query', data=data)
    return response.json()


@api.route('/update', methods=['GET'])
def get_update():
    data = '"PREFIX brick: <https://brickschema.org/schema/1.1.0/Brick#>\\n\\nDELETE { brick:Sensor rdfs:subClassOf ?obj .} WHERE {brick:Sensor rdfs:subClassOf ?obj .}"'
    response = requests.put('http://localhost:8001/update', data=data)
    return response.json()


@api.route('/companies', methods=['GET'])
def get_companies():
    return json.dumps(companies)


@api.route('/companies', methods=['POST'])
def post_companies():
    return json.dumps({"success": True}), 201


if __name__ == '__main__':
    api.run(debug=True, host=loHost)
    # app.run(host=exHost, port=exPort, debug=False) #For running on remote server
