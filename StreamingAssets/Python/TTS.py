from gtts import gTTS
from playsound import playsound

import os
import sys
import io
import threading
import tempfile

tts_lock = threading.Lock()

def text_to_speech(text, lang='ko'):
    global tts_lock
    with tts_lock:
        try:
            tts = gTTS(text=text, lang=lang)
            with tempfile.NamedTemporaryFile(delete=False, suffix=".mp3") as tmp_file:
                tmp_filename = tmp_file.name
            tts.save(tmp_filename)
            playsound(tmp_filename)
        except Exception as e:
            print(f"오류가 발생했습니다: {e}")
        finally:
            if os.path.exists(tmp_filename):
                os.remove(tmp_filename)

if __name__ == "__main__":
    sys.stdin = io.TextIOWrapper(sys.stdin.buffer, encoding='utf-8')

    while True:
        text = input("텍스트를 입력하세요: ")
        threading.Thread(target=text_to_speech, args=(text,)).start()