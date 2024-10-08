import torch
import cv2
import numpy as np

# YOLOv5 모델 로드
print("Loading YOLOv5 model...")
model = torch.hub.load('ultralytics/yolov5', 'yolov5s', pretrained=True)
print("Model loaded.")

# 웹캠 설정
cap = cv2.VideoCapture(0)  # 0번 카메라 사용
if not cap.isOpened():
    print("Error: Could not open video stream from webcam.")
    exit()

print("Starting video stream...")

while True:
    # 프레임 캡처
    ret, frame = cap.read()
    if not ret:
        print("Error: Could not read frame.")
        break

    # 프레임을 RGB로 변환
    img_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # 객체 탐지 수행
    results = model(img_rgb)

    # 탐지 결과를 프레임에 렌더링
    results.render()

    # 렌더링된 프레임을 BGR로 변환 (OpenCV에서 디스플레이를 위해)
    img_bgr = cv2.cvtColor(np.squeeze(results.ims[0]), cv2.COLOR_RGB2BGR)

    # 프레임을 화면에 디스플레이
    cv2.imshow('YOLOv5 Object Detection', img_bgr)

    # 'q' 키를 누르면 종료
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# 자원 해제
cap.release()
cv2.destroyAllWindows()
print("Video stream ended.")