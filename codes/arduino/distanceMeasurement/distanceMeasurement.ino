#define TRIG_PIN_1 4
#define ECHO_PIN_1 3
#define TRIG_PIN_2 13
#define ECHO_PIN_2 12

void setup() {
  Serial.begin(9600);
  
  // Sensor 1 pins
  pinMode(TRIG_PIN_1, OUTPUT);
  pinMode(ECHO_PIN_1, INPUT);

  // Sensor 2 pins
  pinMode(TRIG_PIN_2, OUTPUT);
  pinMode(ECHO_PIN_2, INPUT);
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
  long distance1 = getDistance(TRIG_PIN_1, ECHO_PIN_1);
  long distance2 = getDistance(TRIG_PIN_2, ECHO_PIN_2);
  
  // Send the distances in the format: distance1,distance2
  Serial.print(distance1);
  Serial.print(",");
  Serial.println(distance2);

  delay(1000);
}
