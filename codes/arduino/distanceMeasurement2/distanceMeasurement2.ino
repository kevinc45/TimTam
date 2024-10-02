// The code is for the second Arduino board (Player 2)

#define P2_ECHO_PIN_1 2
#define P2_TRIG_PIN_1 3
#define P2_ECHO_PIN_2 4
#define P2_TRIG_PIN_2 5
#define P2_ECHO_PIN_3 6
#define P2_TRIG_PIN_3 7
#define P2_ECHO_PIN_4 8
#define P2_TRIG_PIN_4 9

void setup() {
  Serial.begin(9600);
  
  // Player 1, Sensor 1
  pinMode(P2_ECHO_PIN_1, INPUT);
  pinMode(P2_TRIG_PIN_1, OUTPUT);

  // Player 1, Sensor 2
  pinMode(P2_ECHO_PIN_2, INPUT);
  pinMode(P2_TRIG_PIN_2, OUTPUT);

  // Player 1, Sensor 3
  pinMode(P2_ECHO_PIN_3, INPUT);
  pinMode(P2_TRIG_PIN_3, OUTPUT);

  // Player 1, Sensor 4
  pinMode(P2_ECHO_PIN_4, INPUT);
  pinMode(P2_TRIG_PIN_4, OUTPUT);
}

long getDistance(int trigPin, int echoPin) {
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  long duration = pulseIn(echoPin, HIGH);
  long distance = duration * 0.034 / 2;

  return distance;
}

void loop() {

  long p2d1 = getDistance(P2_TRIG_PIN_1, P2_ECHO_PIN_1);
  long p2d2 = getDistance(P2_TRIG_PIN_2, P2_ECHO_PIN_2);
  long p2d3 = getDistance(P2_TRIG_PIN_3, P2_ECHO_PIN_3);
  long p2d4 = getDistance(P2_TRIG_PIN_4, P2_ECHO_PIN_4);
  
  // Send the distances in the format: p2d1,p2d2,p2d3,p2d4
  Serial.print(p2d1);
  Serial.print(",");
  Serial.print(p2d2);
  Serial.print(",");
  Serial.print(p2d3);
  Serial.print(",");
  Serial.println(p2d4);

  delay(1000);
}
