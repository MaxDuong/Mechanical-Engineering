import socket
import numpy as np
import cv2

def ImageReadTCP_One(TCP_Handle, CameraSelect):
    writeTCP(TCP_Handle, f"Camera:{CameraSelect},test,test")
    data = b''
    while len(data) == 0:
        data = TCP_Handle.recv(4096)
    while True:
        part = TCP_Handle.recv(4096)
        if not part:
            break
        data += part
    with open('image.png', 'wb') as f:
        f.write(data)
    image = cv2.imread('image.png')
    return image