#include <Adafruit_NeoPixel.h>
#define PIN 6
#define BUFFLEN 64*3+1
Adafruit_NeoPixel strip = Adafruit_NeoPixel(64, PIN, NEO_GRB + NEO_KHZ800);
char pos;
char colordata[3];
char color;
void setup() {
  // put your setup code here, to run once:
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
  Serial.begin(115200);
}

void loop() {
  // put your main code here, to run repeatedly: 

  int result;

  if (Serial.available() > 0) {
    result = Serial.read();
    if (result==0xFF) {
      strip.show();
      pos=0;
      color=0;
    }
    else {
      colordata[color++] = (byte) result>>2;
      if (color>=3) {
        color=0;
        strip.setPixelColor(pos,strip.Color(colordata[0],colordata[1],colordata[2]));
        pos++;
      }
    }
  }
}
