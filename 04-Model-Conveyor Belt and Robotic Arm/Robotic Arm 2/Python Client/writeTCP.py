def writeTCP(TCP_Handle, message):
    message = message.encode('utf-8')
    message_size = len(message)
    TCP_Handle.sendall(message_size.to_bytes(1, 'big') + message)