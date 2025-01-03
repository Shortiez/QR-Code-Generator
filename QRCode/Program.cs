// See https://aka.ms/new-console-template for more information

using QRCodeGen;

var numeric = new QRCode("8675309", ErrorCorrectionLevel.L);
var alphaNumerics = new QRCode("HELLO WORLD", ErrorCorrectionLevel.L);
var bytes = new QRCode("Hello, world!", ErrorCorrectionLevel.L);
