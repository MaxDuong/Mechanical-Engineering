import time

def func_grab(ClientHandle, Grab):
    message = f"ModGrab:{Grab}"
    writeTCP(ClientHandle, message)
    time.sleep(0.054)