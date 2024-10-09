import socket
import threading
import torch
import cv2
import numpy as np

def calculate_checksum(packet):
    checksum = sum(packet[:-1]) & 0xFF
    checksum = (~checksum + 1) & 0xFF
    return checksum

model = torch.hub.load('ultralytics/yolov5', 'yolov5s', pretrained=True)

cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Error: Could not open video stream from webcam.")
    exit()

SERVER_IP = '127.0.0.1' 
SERVER_PORT = 5555
BUFFER_SIZE = 1024

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect((SERVER_IP, SERVER_PORT))

def send_data_to_unity(detected_classes):
    num_objects = len(detected_classes)
    if num_objects == 0:
        return

    class_indices = [key for key, value in model.names.items() if value in detected_classes]

    packet = [255, 2, 3, num_objects] + class_indices + [0]
    packet[-1] = calculate_checksum(packet)

    client_socket.sendall(bytes(packet))

def process_video_and_send():
    while True:
        ret, frame = cap.read()
        if not ret:
            print("Error: Could not read frame.")
            break

        img_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        results = model(img_rgb)

        detected_classes = []
        for det in results.xyxy[0]:
            cls = int(det[5])
            class_name = model.names[cls]
            detected_classes.append(class_name)

        send_data_to_unity(detected_classes)

        results.render()

        img_bgr = np.squeeze(results.ims[0])
        img_bgr = cv2.cvtColor(img_bgr, cv2.COLOR_RGB2BGR)

        cv2.imshow('YOLOv5 Object Detection', img_bgr)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    client_socket.close()
    cv2.destroyAllWindows()

video_thread = threading.Thread(target=process_video_and_send)
video_thread.start()