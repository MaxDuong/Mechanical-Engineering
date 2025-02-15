import numpy as np
import pandas as pd
import platform

import socket
import time

from roboticstoolbox import DHRobot, RevoluteDH
from roboticstoolbox.tools.trajectory import jtraj

from sklearn.preprocessing import MinMaxScaler
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_squared_error, mean_absolute_error

import tensorflow as tf
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.callbacks import ModelCheckpoint, EarlyStopping
from tensorflow.keras.models import load_model

from keras.models import Sequential
from keras.layers import LSTM, Dense, SimpleRNN

import cv2

class SixAxisRobot(DHRobot):
    
    def __init__(self, theta1, theta2, theta3, theta4, theta5, theta6,
                 alpha1, alpha2, alpha3, alpha4, alpha5, alpha6,
                 r1, r2, r3, r4, r5, r6,
                 d1, d2, d3, d4, d5, d6):
        self.theta1 = np.radians(theta1)
        self.theta2 = np.radians(theta2)
        self.theta3 = np.radians(theta3)
        self.theta4 = np.radians(theta4)
        self.theta5 = np.radians(theta5)
        self.theta6 = np.radians(theta6)
        self.alpha1 = np.radians(alpha1)
        self.alpha2 = np.radians(alpha2)
        self.alpha3 = np.radians(alpha3)
        self.alpha4 = np.radians(alpha4)
        self.alpha5 = np.radians(alpha5)
        self.alpha6 = np.radians(alpha6)
        self.r1 = r1
        self.r2 = r2
        self.r3 = r3
        self.r4 = r4
        self.r5 = r5
        self.r6 = r6
        self.d1 = d1
        self.d2 = d2
        self.d3 = d3
        self.d4 = d4
        self.d5 = d5
        self.d6 = d6
        
        links = [
            RevoluteDH(a=self.r1, alpha=self.alpha1, d=self.d1),
            RevoluteDH(a=self.r2, alpha=self.alpha2, d=self.d2),
            RevoluteDH(a=self.r3, alpha=self.alpha3, d=self.d3),
            RevoluteDH(a=self.r4, alpha=self.alpha4, d=self.d4),
            RevoluteDH(a=self.r5, alpha=self.alpha5, d=self.d5),
            RevoluteDH(a=self.r6, alpha=self.alpha6, d=self.d6)
        ]
        super().__init__(links, name="SixAxisRobot")

def writeTCP(TCP_Handle, message):
    message = message.encode('utf-8')
    message_size = len(message)
    TCP_Handle.sendall(message_size.to_bytes(1, 'big') + message)

def TCPInit(IP_Address, Port, Name):
    TCP_Handle = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    TCP_Handle.connect((IP_Address, Port))
    
    SetName = f"Name:{Name}"
    writeTCP(TCP_Handle, SetName)
    return TCP_Handle

def robot_angles_rotation(ClientHandle, q, i):
    q = np.degrees(q)
    message = f"ModRobot:{q[i,0]},{q[i,1]},{q[i,2]},{q[i,3]},{q[i,4]},{q[i,5]}"
    print(message)
    writeTCP(ClientHandle, message)
    time.sleep(0.054)

def robot_grab_object(ClientHandle, Grab):
    message = f"ModGrab:{Grab}"
    writeTCP(ClientHandle, message)
    print(message)
    time.sleep(0.054)

def detect_objects(image_path, image_detection_path):
    # Read the image
    image = cv2.imread(image_path)
    
    # Convert to HSV color space
    hsv = cv2.cvtColor(image, cv2.COLOR_BGR2HSV)
    
    # Define range for red color
    lower_red1 = np.array([0, 100, 100])
    upper_red1 = np.array([10, 255, 255])
    lower_red2 = np.array([160, 100, 100])
    upper_red2 = np.array([180, 255, 255])
    
    # Define range for blue color
    lower_blue = np.array([90, 100, 100])
    upper_blue = np.array([130, 255, 255])
    
    # Create masks for both colors
    mask1 = cv2.inRange(hsv, lower_red1, upper_red1)
    mask2 = cv2.inRange(hsv, lower_red2, upper_red2)
    red_mask = cv2.bitwise_or(mask1, mask2)
    blue_mask = cv2.inRange(hsv, lower_blue, upper_blue)
    
    # Combine masks
    combined_mask = cv2.bitwise_or(red_mask, blue_mask)
    
    # Find contours
    contours, _ = cv2.findContours(combined_mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    
    # Image height and width
    height, width = image.shape[:2]
    
    # List to store all detected boxes
    detected_boxes = []
    
    for contour in contours:
        area = cv2.contourArea(contour)
        
        if area > 500:  # Minimum area threshold
            # Get bounding box
            x, y, w, h = cv2.boundingRect(contour)
            
            # Calculate box center
            box_center_x = x + w//2
            box_center_y = y + h//2
            
            # Check if box is red or blue
            is_red = cv2.countNonZero(red_mask[y:y+h, x:x+w]) > 0
            is_blue = cv2.countNonZero(blue_mask[y:y+h, x:x+w]) > 0
            
            color = 'red' if is_red else 'blue' if is_blue else 'unknown'
            
            # Calculate moment center
            M = cv2.moments(contour)
            if M["m00"] != 0:
                moment_center_x = int(M["m10"] / M["m00"])
                moment_center_y = int(M["m01"] / M["m00"])
                
                # Convert to Unity coordinates
                # unity_x = (box_center_x - 248) * 0.020689655 + 14
                unity_y = (moment_center_y - 264) * (-0.0235)  # Added the new y-position formula

                # Draw visuals
                rect_color = (0, 0, 255) if color == 'red' else (255, 0, 0)  # Red or Blue
                cv2.circle(image, (box_center_x, box_center_y), 5, (0, 255, 0), -1)  # Green for box center
                cv2.rectangle(image, (x, y), (x + w, y + h), (0, 255, 0), 2)
                
                # Store detection results
                detected_boxes.append({
                    'moment_center': (moment_center_x, moment_center_y),
                    'box_center': (box_center_x, box_center_y),
                    'bbox': (x, y, w, h),
                    'color': color,
                    'unity_position': unity_y
                })
    
    cv2.imwrite(image_detection_path, image)
    
    return detected_boxes, height, width

print(1)

Operating_System = platform.system()
IP_Address = '127.0.0.1'
if Operating_System == 'Windows':
    IP_Address = '127.0.0.1'
elif Operating_System == 'Linux':
    IP_Address = '172.22.96.1'
Port = 55003

name = "Python_Client"
Client = TCPInit(IP_Address, Port, name)
if Client:
    print("Connected to server")
else:
    print('Failed to connect to server')


# Load the DataFrame from CSV
df = pd.read_csv('dataframe.csv')

# Load the trained model
model = tf.keras.models.load_model('best_model.keras')  # Replace with your model file path

# Extract the relevant columns for scaling
# Assuming 'x', 'y', 'z' are the position columns and 'theta1', 'theta2', ..., 'theta6' are the joint angle columns
xyz = df[['x', 'y', 'z']].to_numpy()
q = df[['theta1', 'theta2', 'theta3', 'theta4', 'theta5', 'theta6']].to_numpy()

# Create and fit the scalers using the actual data from the DataFrame
scaler_xyz = MinMaxScaler()
scaler_q = MinMaxScaler()

scaler_xyz.fit(xyz)  # Fit scaler_xyz on the position data
scaler_q.fit(q)      # Fit scaler_q on the joint angle data



# Define DH parameters
L1 = 0.9220639; L2 = 4.286629; L3 = 7.095994; L4 = 1.703217; L5 = 6.864536; L6 = 5.310232
theta1 = 0; theta2 = 90; theta3 = 0; theta4 = 0; theta5 = 0; theta6 = 0  
alpha1 = 90; alpha2 = 0; alpha3 = 90; alpha4 = -90; alpha5 = 90; alpha6 = 0
r1 = L1; r2 = L3; r3 = L4; r4 = 0; r5 = 0; r6 = 0
d1 = L2; d2 = 0; d3 = 0; d4 = L5; d5 = 0; d6 = L6

# Create robot with specified DH parameters
robot = SixAxisRobot(theta1, theta2, theta3, theta4, theta5, theta6, alpha1, alpha2, alpha3, alpha4, alpha5, alpha6, r1, r2, r3, r4, r5, r6, d1, d2, d3, d4, d5, d6)
joints = np.array([np.radians(theta1), np.radians(theta2), np.radians(theta3), np.radians(theta4), np.radians(theta5), np.radians(theta6)])
intial_rotations = joints


writeTCP(Client, f"Camera:center,test,test")
response = Client.recv(1024).decode()
image_name, image_path = response.split('|')
image_detection_path = image_path.replace('.png', '_detection.png')
detected_boxes, height, width = detect_objects(image_path, image_detection_path)
print(detected_boxes)


for box in detected_boxes[::-1]:
    print(box['color'], box['unity_position'])
    
    # Input position to test
    x_desired, y_desired, z_desired = 14, -box['unity_position'], 1.6
    input_position = np.array([[x_desired, y_desired, z_desired]])

    # Scale the input position using the scaler_xyz
    input_position_scaled = scaler_xyz.transform(input_position)

    # Reshape the input to match the model's expected input shape
    input_position_scaled = input_position_scaled.reshape(-1, 1, 3)

    # Predict the joint angles
    predicted_angles_scaled = model.predict(input_position_scaled)

    # Inverse transform the predicted angles to get them in the original range
    predicted_angles = scaler_q.inverse_transform(predicted_angles_scaled)

    print("Predicted Joint Angles:", predicted_angles)


    # Approach the box
    grab = 2  # activate EE (0 - release the object, 1 - grab, 2 - do nothing)
    t = np.linspace(0, 2, 21)

    # qi1 = np.radians(predicted_angles[0])
    # qf1 = joints
    qf1 = np.radians(predicted_angles[0])
    qi1 = intial_rotations

    q = jtraj(qi1, qf1, t).q  # Note: parameters swapped to move from start to end

    for i in range(len(q)):
        robot_angles_rotation(Client, q, i)

    # Take an object
    grab = 1
    robot_grab_object(Client, grab)
    time.sleep(1)


    # Back to initial pos with the last robotic arm link pointing downwards towards the ground (theta5 = -90)
    # rotate 45 if the box is red and -45 if the box is blue
    if box['color'] == 'red':
        qi1[0] = np.radians(-45)
    else:
        qi1[0] = np.radians(45)
    qi1[4] = np.radians(-90)
    q = jtraj(qf1, qi1, t).q
    for i in range(len(q)):
        robot_angles_rotation(Client, q, i)
    print(qf1)

    # release object
    grab = 0
    robot_grab_object(Client, grab)
    time.sleep(0.5)

    intial_rotations = qi1


    # break



# # Load the DataFrame from CSV
# df = pd.read_csv('dataframe.csv')

# # Load the trained model
# model = tf.keras.models.load_model('best_model.keras')  # Replace with your model file path

# # Extract the relevant columns for scaling
# # Assuming 'x', 'y', 'z' are the position columns and 'theta1', 'theta2', ..., 'theta6' are the joint angle columns
# xyz = df[['x', 'y', 'z']].to_numpy()
# q = df[['theta1', 'theta2', 'theta3', 'theta4', 'theta5', 'theta6']].to_numpy()

# # Create and fit the scalers using the actual data from the DataFrame
# scaler_xyz = MinMaxScaler()
# scaler_q = MinMaxScaler()

# scaler_xyz.fit(xyz)  # Fit scaler_xyz on the position data
# scaler_q.fit(q)      # Fit scaler_q on the joint angle data

# # Input position to test
# x_desired, y_desired, z_desired = 14, 0, 1.840666
# input_position = np.array([[x_desired, y_desired, z_desired]])

# # Scale the input position using the scaler_xyz
# input_position_scaled = scaler_xyz.transform(input_position)

# # Reshape the input to match the model's expected input shape
# input_position_scaled = input_position_scaled.reshape(-1, 1, 3)

# # Predict the joint angles
# predicted_angles_scaled = model.predict(input_position_scaled)

# # Inverse transform the predicted angles to get them in the original range
# predicted_angles = scaler_q.inverse_transform(predicted_angles_scaled)

# print("Predicted Joint Angles:", predicted_angles)

# # Define DH parameters
# L1 = 0.9220639; L2 = 4.286629; L3 = 7.095994; L4 = 1.703217; L5 = 6.864536; L6 = 5.310232
# theta1 = 0; theta2 = 90; theta3 = 0; theta4 = 0; theta5 = 0; theta6 = 0  
# alpha1 = 90; alpha2 = 0; alpha3 = 90; alpha4 = -90; alpha5 = 90; alpha6 = 0
# r1 = L1; r2 = L3; r3 = L4; r4 = 0; r5 = 0; r6 = 0
# d1 = L2; d2 = 0; d3 = 0; d4 = L5; d5 = 0; d6 = L6

# # Create robot with specified DH parameters
# robot = SixAxisRobot(theta1, theta2, theta3, theta4, theta5, theta6, alpha1, alpha2, alpha3, alpha4, alpha5, alpha6, r1, r2, r3, r4, r5, r6, d1, d2, d3, d4, d5, d6)
# joints = np.array([np.radians(theta1), np.radians(theta2), np.radians(theta3), np.radians(theta4), np.radians(theta5), np.radians(theta6)])


# # Approach the box
# grab = 2  # activate EE (0 - release the object, 1 - grab, 2 - do nothing)
# t = np.linspace(0, 2, 21)

# qi1 = np.radians(predicted_angles[0])
# qf1 = joints

# q = jtraj(qf1, qi1, t).q  # Note: parameters swapped to move from start to end

# for i in range(len(q)):
#     robot_angles_rotation(Client, q, i)

# # Take an object
# grab = 1
# robot_grab_object(Client, grab)
# time.sleep(1)


# # Back to initial pos with the last robotic arm link pointing downwards towards the ground (theta5 = -90)
# qf1[4] = np.radians(-90)
# q = jtraj(qi1, qf1, t).q
# for i in range(len(q)):
#     robot_angles_rotation(Client, q, i)
# print(qf1)

# # release object
# grab = 0
# robot_grab_object(Client, grab)
# time.sleep(0.5)