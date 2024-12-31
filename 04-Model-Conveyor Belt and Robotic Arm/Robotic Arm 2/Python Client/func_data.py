import time

def func_data(ClientHandle, Q, b):
    message = f"ModRobot:{Q[b,0]*180/np.pi},{Q[b,1]*180/np.pi},{Q[b,2]*180/np.pi},{Q[b,3]*180/np.pi},{Q[b,4]*180/np.pi},{Q[b,5]*180/np.pi}"
    writeTCP(ClientHandle, message)
    time.sleep(0.054)