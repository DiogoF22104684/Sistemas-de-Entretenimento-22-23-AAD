#include "pitches.h"
#include <KeyMatrix.h>

KEY_MATRIX_PHONE(keypad, 4, 5, 6, 7, 11, 9, 10);

// notes in the melody:
int melody[] = {
  NOTE_C4, NOTE_G3, NOTE_G3, NOTE_A3, NOTE_G3, 0, NOTE_B3, NOTE_C4, 0
};

// note durations: 4 = quarter note, 8 = eighth note, etc.:
int noteDurations[] = {
  4, 8, 8, 4, 4, 4, 4, 4, 1
};

int buzzerPin = 8;

int receiverPin = 3;
bool receiverState = true;
bool prevReceiverState = true;

bool ring = false;
bool keys = false;
bool receiverUp = false;

bool upInterrupt = false;

void setup() {
  // iterate over the notes of the melody:
  pinMode(receiverPin, INPUT_PULLUP);

  Serial.begin(9600);
  Serial.println("start");
}

void loop() {

  if(Serial.available() > 0)
  {
    String inStr = Serial.readString();
    inStr.trim();
      Serial.println("message");

    if (inStr == "ring" && !receiverUp)
    {
        Serial.println("ring");

      ring = true;
    }
  }

  prevReceiverState = receiverState;
  receiverState = (bool)digitalRead(receiverPin);
  if((prevReceiverState && !receiverState) || upInterrupt) // Levantou
  {
    upInterrupt = false;
    receiverUp = true;
    if(ring)
    {
      ring = false;
      keys = true;
      Serial.println("up");
    }
  }
  else if(!prevReceiverState && receiverState) // Pousou
  {
    receiverUp = false;
    if(keys)
    {
      keys = false;
      Serial.println("down");
    }
  }
  
  if(ring)
    RingRing();

  if(keys)
    ReadKeys();
  
}

void ReadKeys()
{
  delay(50);
  
  if (keypad.pollEvent() && keypad.event.type == KM_KEYDOWN) 
    Serial.println(keypad.event.c);
}

void RingRing()
{
  for (int thisNote = 0; thisNote < 8; thisNote++) {
    // to calculate the note duration, take one second divided by the note type.
    //e.g. quarter note = 1000 / 4, eighth note = 1000/8, etc.
    int noteDuration = 1000 / noteDurations[thisNote];
    tone(buzzerPin, melody[thisNote], noteDuration);

    // to distinguish the notes, set a minimum time between them.
    // the note's duration + 30% seems to work well:
    int pauseBetweenNotes = noteDuration * 1.30;
    delay(pauseBetweenNotes);
    // stop the tone playing:
    noTone(buzzerPin);

    prevReceiverState = receiverState;
    receiverState = (bool)digitalRead(receiverPin);
    if(prevReceiverState && !receiverState)
    {
      upInterrupt = true;
      return;
    }
  }
}
