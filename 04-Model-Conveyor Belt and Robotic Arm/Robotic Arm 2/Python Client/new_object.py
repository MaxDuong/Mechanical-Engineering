def new_object(ClientHandle, init):
    writeTCP(ClientHandle, f"NewObj:{init}")