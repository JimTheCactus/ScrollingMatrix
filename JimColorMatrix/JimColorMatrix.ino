#include <Adafruit_NeoPixel.h>
#define PIN 6
Adafruit_NeoPixel strip = Adafruit_NeoPixel(64, PIN, NEO_GRB + NEO_KHZ800);
void setup() {
  // put your setup code here, to run once:
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
  Serial.begin(115200);
}

void loop() {
  // put your main code here, to run repeatedly: 

  char colordata[80];
  char result;
  // Read a null terminated string
  result=Serial.readBytesUntil('\0',colordata,79);
  // If there's enough in there to display
  if (result>=64) {
    // Dump it to the screen.
    for (char i=0;i<64;i++) {
      // Unpack the pixels.
      // Bit 7 is ignored (always 1)
      // Bits 5&6 are red
      // Bits 2, 3, and 4 are green
      // Bits 0&1 are blue
      // The shifts are currently being scales down by two to dim the LEDs (1/4th power) the reasoning being that the
      // matrix is too bright indoors.
      strip.setPixelColor(i,strip.Color((colordata[i]&0x60)<<(1-2),(colordata[i]&0x1C)<<(3-2),(colordata[i]&0x3)<<(6-2)));
    }
  }
  // If we get a non-empty frame but not enough actual data
  else if (result>0) {
    // This implies a comms error. Write a red dot in the to right and black everywhere else.
    for(char i=1;i<64;i++) {
      strip.setPixelColor(i,strip.Color(0,0,0));
    }
    strip.setPixelColor(0,strip.Color(255,0,0));
  }
  // Dump the buffer to the matrix bus.
  strip.show();
}
