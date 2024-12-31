import numpy as np
import cv2

def color_check(Client):
    test = ImageReadTCP_One(Client, 'Center')
    cv2.imshow('image', test)
    cv2.waitKey(0)
    cv2.destroyAllWindows()
    R = test[:, :, 0]
    G = test[:, :, 1]
    B = test[:, :, 2]
    if np.mean(R) > np.mean(G) and np.mean(R) > np.mean(B):
        color = 1  # Red Color
    elif np.mean(G) > np.mean(R) and np.mean(G) > np.mean(B):
        color = 2  # Green Color
    else:
        color = 3  # Blue Color
    return color