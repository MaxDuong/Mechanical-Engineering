from opcua import Server
from datetime import datetime
import time

print(1)

# Create a server instance
server = Server()

# Set server properties
server.set_endpoint("opc.tcp://127.0.0.1:4840/freeopcua/server/")
server.set_server_name("Python OPC UA Server")

# Set namespace
uri = "http://example.org"
idx = server.register_namespace(uri)

# Create a variable node
objects = server.get_objects_node()
myobj = objects.add_object(idx, "MyObject")
myvar = myobj.add_variable(idx, "MyVariable", 0)
myvar.set_writable()

# Start the server
server.start()
print("OPC UA Server started at opc.tcp://127.0.0.1:4840/freeopcua/server/")

try:
    while True:
        # Update variable
        myvar.set_value(datetime.now().strftime("%H:%M:%S"))
        time.sleep(1)
finally:
    server.stop()
    print("Server stopped.")