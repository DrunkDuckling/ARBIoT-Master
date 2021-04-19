# Python program to read old and create new json file
# needs to be placed into a for-loop iterating new files
import json
import os

base_dir = '/Users/Frederik/Desktop/Master/Backend/Backend test/GO/convetJsonPYScript/data'

# Get all files in the directory
data_list = []

for file in os.listdir(base_dir):

    # If file is a json, construct it's full path and open it, append all json data to list
    if 'json' in file:
        # Get path to the dir with file
        json_path = os.path.join(base_dir, file)

        # Open and load json file
        with open(json_path) as data_file:
            data = json.load(data_file)
            print("load file: " + file)
            # Create new input for file
            newData = {"topic": "ou44/uuids/"+data["uuid"], "samples": []}

            # Iterating through data file and input data into "samples" key
            for i in data['Readings']:
                if i[0] > 1541875820000.0:
                    newData["samples"].append({"time": (i[0]-1541875820000.0)/1000, "value": i[1]})

            data.update(newData)

            print("update file with new values.")
            # Delete old key "Readings" as it has been replaced by "samples"
            del data['Readings']
            print("Delete old value 'Readings':")
        # Append newly created json file into a collection of json's
        print("Added: " + file + " To collections file")
        print(" ")
        data_list.append(data)

# Make the file into json format (Serializing json)
newData_List = json.dumps(data_list, indent=4)

# Writing to data/new/config_old.json
with open("../sdu-iot-mqtt-siggen-master/config2.json", "w") as outfile:
    outfile.write(newData_List)
print("Successfully wrote file into: data/new ")
# Closing file
data_file.close()
