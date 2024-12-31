import numpy as np
from roboticstoolbox import DHRobot, RevoluteDH
from spatialmath import SE3
import time

def func_data(ClientHandle, q, b):
    message = f"ModRobot:{q[b,0]*180/np.pi},{q[b,1]*180/np.pi},{q[b,2]*180/np.pi},{q[b,3]*180/np.pi},{q[b,4]*180/np.pi},{q[b,5]*180/np.pi}"
    writeTCP(ClientHandle, message)
    time.sleep(0.054)

def func_grab(ClientHandle, Grab):
    message = f"ModGrab:{Grab}"
    writeTCP(ClientHandle, message)
    time.sleep(0.054)

def writeTCP(TCP_Handle, message):
    message = message.encode('utf-8')
    message_size = len(message)
    TCP_Handle.sendall(message_size.to_bytes(1, 'big') + message)

def TCPInit(IP_Address, Port, Name):
    import socket
    TCP_Handle = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    TCP_Handle.connect((IP_Address, Port))
    print("Connected to server")
    SetName = f"Name:{Name}"
    writeTCP(TCP_Handle, SetName)
    return TCP_Handle

def main():
    name = "Matlab"
    Client = TCPInit('127.0.0.1', 55001, name)

    L1 = 0.0411
    L2 = 0.192
    L3 = 0.3163
    L4 = 0.0759
    L5 = 0.306
    L6 = 0.233
    thetha1 = 0
    thetha2 = 90
    thetha3 = 0
    thetha4 = 0
    thetha5 = -90
    thetha6 = 0
    alpha1 = 90
    alpha2 = 0
    alpha3 = 90
    alpha4 = 270
    alpha5 = 90
    alpha6 = 0
    r1 = L1
    r2 = L3
    r3 = L4
    r4 = 0
    r5 = 0
    r6 = 0
    d1 = L2
    d2 = 0
    d3 = 0
    d4 = L5
    d5 = 0
    d6 = L6

    L = [
        RevoluteDH(d=d1, a=r1, alpha=np.radians(alpha1)),
        RevoluteDH(d=d2, a=r2, alpha=np.radians(alpha2)),
        RevoluteDH(d=d3, a=r3, alpha=np.radians(alpha3)),
        RevoluteDH(d=d4, a=r4, alpha=np.radians(alpha4)),
        RevoluteDH(d=d5, a=r5, alpha=np.radians(alpha5)),
        RevoluteDH(d=d6, a=r6, alpha=np.radians(alpha6))
    ]
    robot = DHRobot(L)
    joints = [np.radians(thetha1), np.radians(thetha2), np.radians(thetha3), np.radians(thetha4), np.radians(thetha5), np.radians(thetha6)]
    # robot.plot(joints)
    # robot.teach(joints)

    grab = 2  # activate EE (0 - release the object, 1 - grab, 2 - do nothing)
    t = np.linspace(0, 2, 21)

    X1 = 0.462
    Y1 = 0.209
    Z1 = 0.04

    T = SE3(X1, -Y1, Z1) * SE3.Rx(np.radians(180))
    qi1 = robot.ikine_LM(T).q
    qf1 = joints
    q = robot.jtraj(qf1, qi1, t)

    b = 1
    for a in range(len(q.q)):
        func_data(Client, q.q, b)
        b += 1

    # take object
    grab = 1
    func_grab(Client, grab)
    time.sleep(4.5)

    # back to initial pos
    b = 1
    q = robot.jtraj(qi1, qf1, t)
    for a in range(len(q.q)):
        func_data(Client, q.q, b)
        b += 1

    # release object
    grab = 0
    func_grab(Client, grab)
    time.sleep(0.5)

if __name__ == "__main__":
    main()