function Decoder(bytes, port) {
  var tmp = (bytes[0]<<8 | bytes[1]);
  
    return {
      temperature: (tmp-5000)/100
    }
}
