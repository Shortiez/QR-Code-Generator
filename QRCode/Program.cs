// See https://aka.ms/new-console-template for more information

using QRCodeGen;

var numeric = new QRCode("8675309", ErrorCorrectionLevel.L);
//var alphaNumerics = new QRCode("Hello World", ErrorCorrectionLevel.Q);
//var bytes = new QRCode("Hello World", ErrorCorrectionLevel.H);
