import numpy as np
import platform
# from roboticstoolbox import DHRobot, RevoluteDH
# from spatialmath import SE3
# import socket
# import time

def writeTCP(TCP_Handle, message):
    message = message.encode('utf-8')
    message_size = len(message)
    TCP_Handle.sendall(message_size.to_bytes(1, 'big') + message)

def TCPInit(IP_Address, Port, Name):
    import socket
    TCP_Handle = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    TCP_Handle.connect((IP_Address, Port))
    
    SetName = f"Name:{Name}"
    writeTCP(TCP_Handle, SetName)
    return TCP_Handle

Operating_System = platform.system()
IP_Address = '127.0.0.1'
if Operating_System == 'Windows':
    IP_Address = '127.0.0.1'
elif Operating_System == 'Linux':
    IP_Address = '172.22.96.1'
Port = 55001

name = "Python_Client"
Client = TCPInit(IP_Address, Port, name)
if Client:
    print("Connected to server")
else:
    print('Failed to connect to server')
