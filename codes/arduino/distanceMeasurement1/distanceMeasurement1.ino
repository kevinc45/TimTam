// The code is for the first Arduino board (Player 1)

#define P1_TRIG_PIN_1 13
#define P1_ECHO_PIN_1 12
#define P1_TRIG_PIN_2 11
#define P1_ECHO_PIN_2 10
#define P1_TRIG_PIN_3 9
#define P1_ECHO_PIN_3 8
#define P1_TRIG_PIN_4 7
#define P1_ECHO_PIN_4 6

void setup() {
  Serial.begin(9600);
  
  // Player 1, Sensor 1
  pinMode(P1_ECHO_PIN_1, INPUT);
  pinMode(P1_TRIG_PIN_1, OUTPUT);

  // Player 1, Sensor 2
  pinMode(P1_ECHO_PIN_2, INPUT);
  pinMode(P1_TRIG_PIN_2, OUTPUT);

  // Player 1, Sensor 3
  pinMode(P1_ECHO_PIN_3, INPUT);
  pinMode(P1_TRIG_PIN_3, OUTPUT);

  // Player 1, Sensor 4
  pinMode(P1_ECHO_PIN_4, INPUT);
  pinMode(P1_TRIG_PIN_4, OUTPUT);
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

  long p1d1 = getDistance(P1_TRIG_PIN_1, P1_ECHO_PIN_1);
  long p1d2 = getDistance(P1_TRIG_PIN_2, P1_ECHO_PIN_2);
  long p1d3 = getDistance(P1_TRIG_PIN_3, P1_ECHO_PIN_3);
  long p1d4 = getDistance(P1_TRIG_PIN_4, P1_ECHO_PIN_4);
  
  // Send the distances in the format: p1d1,p1d2,p1d3,p1d4
  Serial.print(p1d1);
  Serial.print(",");
  Serial.print(p1d2);
  Serial.print(",");
  Serial.print(p1d3);
  Serial.print(",");
  Serial.println(p1d4);

  delay(1000);
}
