function Decoder(bytes, port) {
  var tmp = (bytes[0]<<8 | bytes[1]);
  var pre = (bytes[2]<<8 | bytes[3]);
  var hum = (bytes[4]);
  
    return {
      temperature: (tmp-5000)/100,
      airpressure: pre/10,
      humidity: hum/2,
    }
}
