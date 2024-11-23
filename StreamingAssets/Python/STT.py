import socket
import threading

import whisper
import pyaudio
import numpy as np
import librosa
from pydub import AudioSegment, effects
import noisereduce as nr

model = whisper.load_model("medium")

def calculate_checksum(data):
    checksum = 0
    for byte in data:
        checksum ^= byte
    return checksum

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

def reduce_noise(audio_data):
    audio_array, sr = librosa.load(librosa.util.buf_to_float(audio_data, n_bytes=2), sr=16000)
    reduced_noise_audio = nr.reduce_noise(y=audio_array, sr=sr)
    return reduced_noise_audio, sr

def preprocess_audio(audio_data):
    audio_segment = AudioSegment(
        data=audio_data,
        sample_width=2,
        frame_rate=16000,
        channels=1
    )
    audio_segment = effects.normalize(audio_segment)
    return audio_segment

def record_voice(duration=5, rate=16000, chunk=1024):
    p = pyaudio.PyAudio()
    stream = p.open(format=pyaudio.paInt16, channels=1, rate=rate, input=True, frames_per_buffer=chunk)
    frames = []

    print("Listening..")
    for _ in range(0, int(rate / chunk * duration)):
        data = stream.read(chunk)
        frames.append(np.frombuffer(data, dtype=np.int16))

    stream.stop_stream()
    stream.close()
    p.terminate()

    audio_data = np.hstack(frames).astype(np.float32) / 32768.0
    return audio_data

def recognize_speech_with_whisper(audio_data):
    try:
        result = model.transcribe(audio_data, fp16=False, language="ko")
        return {
            "success": True,
            "error": None,
            "transcription": result["text"].strip()
        }
    except Exception as e:
        return {
            "success": False,
            "error": str(e),
            "transcription": None
        }

def voice_recognition(client_socket):
    while True:
        audio_data = record_voice()
        response = recognize_speech_with_whisper(audio_data)

        if response["success"]:
            print(f"Result: {response['transcription']}")
            send_to_unity(response['transcription'], client_socket)
        else:
            print(f"ERROR: {response['error']}")

def main():
    server_ip = '127.0.0.1'
    port = 5555
    client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        client_socket.connect((server_ip, port))
        print(f"Connected to Unity server at {server_ip}:{port}")
    except Exception as e:
        print(f"Failed to connect to Unity server: {e}")
        return

    while True:
        voice_recognition(client_socket)

    # voice_recognition_thread = threading.Thread(target=voice_recognition, args=(client_socket,))
    # voice_recognition_thread.start()

if __name__ == "__main__":
    main()