// Libraries LoRaWAN / SPI / OLED Display
#include <lmic.h>
#include <hal/hal.h>
#include <SPI.h>
#include <U8x8lib.h>
#include <U8g2lib.h>
//Libraries for BME280
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
//BME280 definition festlegen
#define SDA 4
#define SCL 15

TwoWire I2Cone = TwoWire(1);
Adafruit_BME280 bme;

float temperature = 0;
float humidity = 0;
float pressure = 0;

uint8_t tx_payload[5];

// LoRaWAN Network Session Key (drag and drop als msb )
static const PROGMEM u1_t NWKSKEY[16] = {  };
// LoRaWAN App Session Key (Drag and Drop als msb)
static const u1_t PROGMEM APPSKEY[16] = { };
// LoRaWAN Device Address
static const u4_t DEVADDR = YOUR_DEVADDRESS;

void startBME() {
  delay(1000);
  I2Cone.begin(SDA, SCL, 100000);
  bool status1 = bme.begin(0x76, &I2Cone);
  if (!status1) {
    Serial.println("Kein BME280 Sensor gefunden!");
    while (1)
      ;
  }
  delay(3000);
}

// Display passend konfigurieren
U8G2_SSD1306_128X64_NONAME_F_SW_I2C u8g2(U8G2_R0, /* clock=*/15, /* data=*/4, /* reset=*/16);
U8X8_SSD1306_128X64_NONAME_SW_I2C u8x8(15, 4, 16);

// These callbacks are only used in over-the-air activation, so they are
// left empty here (we cannot leave them out completely unless
// DISABLE_JOIN is set in config.h, otherwise the linker will complain).
void os_getArtEui(u1_t* buf) {}
void os_getDevEui(u1_t* buf) {}
void os_getDevKey(u1_t* buf) {}

static osjob_t sendjob;

// Senden alle 120 Sekunden
const unsigned TX_INTERVAL = 600;

// LoRa Heltec V2 Pin Map
const lmic_pinmap lmic_pins = {
  .nss = 18,
  .rxtx = LMIC_UNUSED_PIN,
  .rst = 14,
  .dio = { 34, 26, 35 },
};

void do_send(osjob_t* j) {
  // Check if there is not a current TX/RX job running
  if (LMIC.opmode & OP_TXRXPEND) {
    Serial.println(F("OP_TXRXPEND, not sending"));
  } else {
    // Prepare upstream data transmission at the next possible time.
    temperature = bme.readTemperature();
    humidity = bme.readHumidity();
    pressure = bme.readPressure() / 100;
    Serial.print("Sending LoRa Packet to TTN Gatway: ");
    Serial.print("Sending Complete Data: ");

    uint8_t payload[5];

    int tmp = ((int)(temperature * 100)) + 5000;
    int pre = (int)(pressure * 10);
    byte hum = (int)(humidity * 2);

    payload[0] = tmp >> 8;
    payload[1] = tmp;
    payload[2] = pre >> 8;
    payload[3] = pre;
    payload[4] = hum;

    int i = 0;
    while (i < sizeof(payload)) {
      tx_payload[i] = payload[i];
      i++;
    }

    LMIC_setTxData2(1, tx_payload, sizeof(tx_payload), 0);
    Serial.println(F("Packet queued"));
  }

  // Next TX is scheduled after TX_COMPLETE event.
}

void setup() {
  Serial.begin(115200);
  SPI.begin(5, 19, 27, 18);
  u8g2.begin();
  u8g2.clearBuffer();  // Internen Display Speicher komplett lÃ¶schen
  u8g2.sendBuffer();   // Inhalt des Speichers direkt auf das Display pushen
  delay(2000);

  u8x8.begin();
  u8x8.clear();
  u8x8.setFont(u8x8_font_chroma48medium8_r);
  u8x8.drawString(0, 2, "Read: BME280 ");
  delay(2000);

  u8x8.clear();
  u8x8.drawString(0, 2, " Push Message to");
  u8x8.drawString(0, 3, "   TTN Gateway  ");
  delay(1500);
  u8x8.clear();
  u8x8.begin();
  u8x8.setFont(u8x8_font_open_iconic_weather_4x4);
  u8x8.drawString(0, 2, "A");
  u8x8.setFont(u8x8_font_chroma48medium8_r);
  u8x8.drawString(4, 3, "Send Data..");
  delay(3000);
  u8x8.clear();


Wire.begin();
  startBME();
  //Belegung Heltec LoRa V2 Board
  Serial.println(F("Booting..."));
  // LMIC init
  os_init();
  // Reset the MAC state. Session and pending data transfers will be discarded.
  LMIC_reset();

#ifdef PROGMEM.
  uint8_t appskey[sizeof(APPSKEY)];
  uint8_t nwkskey[sizeof(NWKSKEY)];
  memcpy_P(appskey, APPSKEY, sizeof(APPSKEY));
  memcpy_P(nwkskey, NWKSKEY, sizeof(NWKSKEY));
  LMIC_setSession(0x1, DEVADDR, nwkskey, appskey);
#else
  LMIC_setSession(0x1, DEVADDR, NWKSKEY, APPSKEY);
#endif

#if defined(CFG_eu868)
  LMIC_setupChannel(0, 868100000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(1, 868300000, DR_RANGE_MAP(DR_SF12, DR_SF7B), BAND_CENTI);  // g-band
  LMIC_setupChannel(2, 868500000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(3, 867100000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(4, 867300000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(5, 867500000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(6, 867700000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(7, 867900000, DR_RANGE_MAP(DR_SF12, DR_SF7), BAND_CENTI);   // g-band
  LMIC_setupChannel(8, 868800000, DR_RANGE_MAP(DR_FSK, DR_FSK), BAND_MILLI);    // g2-band
#endif

  // Start job
  do_send(&sendjob);

  LMIC.dn2Dr = DR_SF9;
}

void loop() {
  os_runloop_once();
}

void onEvent(ev_t ev) {
  Serial.print(os_getTime());
  Serial.print(": ");
  switch (ev) {
    case EV_SCAN_TIMEOUT:
      Serial.println(F("EV_SCAN_TIMEOUT"));
      break;
    case EV_BEACON_FOUND:
      Serial.println(F("EV_BEACON_FOUND"));
      break;
    case EV_BEACON_MISSED:
      Serial.println(F("EV_BEACON_MISSED"));
      break;
    case EV_BEACON_TRACKED:
      Serial.println(F("EV_BEACON_TRACKED"));
      break;
    case EV_JOINING:
      Serial.println(F("EV_JOINING"));
      break;
    case EV_JOINED:
      Serial.println(F("EV_JOINED"));
      break;
    case EV_RFU1:
      Serial.println(F("EV_RFU1"));
      break;
    case EV_JOIN_FAILED:
      Serial.println(F("EV_JOIN_FAILED"));
      break;
    case EV_REJOIN_FAILED:
      Serial.println(F("EV_REJOIN_FAILED"));
      break;
    case EV_TXCOMPLETE:
      Serial.println(F("EV_TXCOMPLETE (includes waiting for RX windows)"));
      if (LMIC.txrxFlags & TXRX_ACK)
        Serial.println(F("Received ack"));
      if (LMIC.dataLen) {
        Serial.println(F("Received "));
        Serial.println(LMIC.dataLen);
        Serial.println(F(" bytes of payload"));
      }
      // Schedule next transmission
      os_setTimedCallback(&sendjob, os_getTime() + sec2osticks(TX_INTERVAL), do_send);
      break;
    case EV_LOST_TSYNC:
      Serial.println(F("EV_LOST_TSYNC"));
      break;
    case EV_RESET:
      Serial.println(F("EV_RESET"));
      break;
    case EV_RXCOMPLETE:
      // data received in ping slot
      Serial.println(F("EV_RXCOMPLETE"));
      break;
    case EV_LINK_DEAD:
      Serial.println(F("EV_LINK_DEAD"));
      break;
    case EV_LINK_ALIVE:
      Serial.println(F("EV_LINK_ALIVE"));
      break;
    default:
      Serial.println(F("Unknown event"));
      break;
  }
}
