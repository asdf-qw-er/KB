import cv2
import numpy as np
from tensorflow.keras.models import load_model
from keras.preprocessing.image import img_to_array
from collections import defaultdict
import time
import socket
import threading

# 감정 정의
emotion_dict = {
    0: 'Anger', 
    1: 'Disgust', 
    2: 'Fear', 
    3: 'Happy', 
    4: 'Sad', 
    5: 'Surprise', 
    6: 'Neutral'
}

# 모델 로드 - efficientnet모델 활용
model = load_model('efficientnet_fer2013.h5')

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

# Haar Cascade 얼굴 검출기
face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

# 감정 카운트 변수 초기화
emotion_count = defaultdict(int)
total_predictions = 0
start_time = time.time()

# 웹캠 열기
video_capture = cv2.VideoCapture(0)

while True:
    ret, frame = video_capture.read()
    if not ret:
        break

    gray_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    faces = face_cascade.detectMultiScale(gray_frame, scaleFactor=1.3, minNeighbors=5)

    for (x, y, w, h) in faces:
        face_img = gray_frame[y:y + h, x:x + w]
        face_resized = cv2.resize(face_img, (48, 48))  
        face_resized = face_resized.astype('float32') / 255.0  
        face_resized = img_to_array(face_resized)
        face_resized = np.expand_dims(face_resized, axis=0)

        # 감정 예측
        emotion_preds = model.predict(face_resized)[0]
        predicted_emotion = np.argmax(emotion_preds)

        # 감정 카운트
        emotion_count[predicted_emotion] += 1
        total_predictions += 1

        # 얼굴 영역에 감정 결과 표시
        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 0, 255), 2)
        cv2.putText(frame, f'Emotion: {emotion_dict[predicted_emotion]} ({np.max(emotion_preds):.3f})', 
                    (x, y - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.6, (36, 255, 12), 2)
        threading.Thread(target=send_emotion_data, args=(predicted_emotion,)).start()

    # 감정 비율 계산 및 출력
    if total_predictions > 0:
        y_offset = 20
        for emotion_idx, count in emotion_count.items():
            emotion_label = emotion_dict[emotion_idx]
            emotion_percentage = (count / total_predictions) * 100
            cv2.putText(frame, f'{emotion_label}: {emotion_percentage:.2f}%', (10, y_offset), 
                        cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2)
            y_offset += 30

    # 결과 프레임 출력
    cv2.imshow('Video', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# 웹캠 종료
video_capture.release()
cv2.destroyAllWindows()
