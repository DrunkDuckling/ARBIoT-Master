#!/usr/bin/env python3

import sys
import json
import requests

if len(sys.argv) != 4:
    print('Syntax: %s RDF_SERVER_HOST RDF_SERVER_PORT SPARQL_FILENAME' % sys.argv[0])
    print('        %s 127.0.0.1 8001 test.rq' % sys.argv[0])
    sys.exit(1)

host     =     sys.argv[1]
port     = int(sys.argv[2])
filename =     sys.argv[3]

with open(filename) as fo:
    lines = ''.join(fo.readlines())

q = json.dumps(lines, sort_keys=True, indent=4, separators=(',', ': '))

r = requests.put('http://%s:%d/query' % (host, port), data=q)

r = json.loads(r.text)
print(json.dumps(r, sort_keys=True, indent=4, separators=(',', ': ')))

