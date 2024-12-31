import socket

def TCPInit(IP_Address, Port, Name):
    TCP_Handle = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    TCP_Handle.connect((IP_Address, Port))
    print("Connected to server")
    SetName = f"Name:{Name}"
    writeTCP(TCP_Handle, SetName)
    return TCP_Handle