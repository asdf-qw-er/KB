import socket
import speech_recognition as sr
import threading

def calculate_checksum(data):
    checksum = 0
    for byte in data:
        checksum ^= byte
    return checksum

def recognize_speech_from_mic(recognizer, microphone):
    response = {
        "success": False,
        "error": None,
        "transcription": None
    }

    with microphone as source:
        print("Listening...")
        try:
            audio = recognizer.listen(source, timeout=1, phrase_time_limit=3)
        except sr.WaitTimeoutError:
            print("Timeout: No speech detected within the time limit.")
            return response

    try:
        response["transcription"] = recognizer.recognize_google(audio, language="ko-KR")
        response["success"] = True
    except sr.RequestError:
        response["error"] = "API unavailable"
    except sr.UnknownValueError:
        response["error"] = "Unable to recognize speech"

    return response

def send_to_unity(message, client_socket):
    try:
        message_bytes = message.encode('utf-8')
        message_length = len(message_bytes)

        packet = bytearray([255, 2, 1])
        packet.append(message_length)
        packet.extend(message_bytes)
        checksum = calculate_checksum(packet)
        packet.append(checksum)

        client_socket.sendall(packet)
        print(f"Sent to Unity: {packet}")
    except Exception as e:
        print(f"Failed to send message: {e}")

def process_speech(client_socket, recognizer, microphone):
    while True:
        response = recognize_speech_from_mic(recognizer, microphone)

        if response["success"]:
            print(f"You said: {response['transcription']}")
            send_to_unity(response['transcription'], client_socket)
        else:
            print("I didn't catch that. What did you say?")

        if response["error"]:
            print(f"ERROR: {response['error']}")

def main():
    recognizer = sr.Recognizer()
    microphone = sr.Microphone()

    with microphone as source:
        recognizer.adjust_for_ambient_noise(source)
        print("Calibration completed. Starting STT loop.")

    server_ip = '127.0.0.1'
    port = 5555

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        client_socket.connect((server_ip, port))
        print(f"Connected to Unity server at {server_ip}:{port}")
    except Exception as e:
        print(f"Failed to connect to Unity server: {e}")
        return

    speech_thread = threading.Thread(target=process_speech, args=(client_socket, recognizer, microphone))
    speech_thread.start()

if __name__ == "__main__":
    main()