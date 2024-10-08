from gtts import gTTS
from playsound import playsound
import os
import tempfile

def text_to_speech(text, lang='ko'):
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
    text = input("텍스트를 입력하세요: ")
    text_to_speech(text)