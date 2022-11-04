#include <KeyMatrix.h>

KEY_MATRIX_PHONE(keypad, 4, 5, 6, 7, 8, 9, 10);

void setup() {
  Serial.begin(9600);
}

void loop() {
  // poll events from the keypad
  if (keypad.pollEvent()) {
    // print to serial
    Serial.println(event_to_string(keypad.event));
  }
}

// helper function to print events

String event_to_string(const KeyMatrixEvent &ev) {
  String s;
  switch (ev.type) {
    case KM_NONE: s = "KM_NONE ("; break;
    case KM_KEYUP: s = "KM_KEYUP ("; break;
    case KM_KEYDOWN: s = "KM_KEYDOWN ("; break;
    case KM_TEXT: s = "KM_TEXT ("; break;
    case KM_MODE: s = "KM_MODE ("; break;
  }
  if (ev.type == KM_MODE) {
    switch (ev.c) {
      case KM_MODE_LOWER: s += "KM_MODE_LOWER"; break;
      case KM_MODE_UPPER: s += "KM_MODE_UPPER"; break;
      case KM_MODE_NUM: s += "KM_MODE_NUM"; break;
    }
  } else {
    s += ev.c;
  }
  s += ")";
  return s;
}
