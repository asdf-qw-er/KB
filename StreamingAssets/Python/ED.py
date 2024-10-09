import cv2
import numpy as np
from keras.models import load_model
import socket
import threading

emotions = ['Angry', 'Disgust', 'Fear', 'Happy', 'Sad', 'Surprise', 'Neutral']
model = load_model('fer_model.h5')

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect(('127.0.0.1', 5555))

def calculate_checksum(data):
    checksum = ~sum(data) + 1
    return checksum & 0xFF 

def send_emotion_data(emotion_index):
    packet = [255, 2, 2, 1, emotion_index]
    checksum = calculate_checksum(packet)
    packet.append(checksum)

    client_socket.send(bytearray(packet))

cap = cv2.VideoCapture(0)

def process_frame():
    while True:
        ret, frame = cap.read()
        small_frame = cv2.resize(frame, (48, 48))
        gray_frame = cv2.cvtColor(small_frame, cv2.COLOR_BGR2GRAY)
        input_data = gray_frame.reshape((1, 48, 48, 1)) / 255.0

        prediction = model.predict(input_data)
        max_index = np.argmax(prediction[0])
        emotion = emotions[max_index]

        # 예측 확률 확인 (옵션)
        # max_probability = np.max(prediction[0])
        # print(f'Predicted emotion: {emotion}, Probability: {max_probability}')

        cv2.putText(frame, emotion, (20, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

        threading.Thread(target=send_emotion_data, args=(max_index,)).start()

        cv2.imshow('Emotion Detection', frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

process_frame()

cap.release()
cv2.destroyAllWindows()

client_socket.close()