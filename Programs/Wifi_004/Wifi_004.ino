/*
 * Miriam control
 *  
 * Miriam is an isothermal amplification unit capable of real time detection that supports a 96 well PCR plate
 * This firmware allows the control over webservice so that Miriam is connected to a WIFI via string messages
 *
 * The basic configuration of the board is:
 *
 * - Temperature Sensors pins: 23,24,25,26 and sensor 5 not using but connect to 19 
 * - Heater pins: 9,10,1,13 and heat 5 not using but connect to 14 
 * - LEDS: 22,2,12,11
 *   
 * (c) 2016 Juho Terrijarvi juho@miroculus.com, Miroculus Inc.
 * (c) 2014 Juanjo Tara j.tara@arduino.cc, Arduino Verkstad AB 
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *    
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


#include <PID_v1.h>
#include <math.h>


//Selector ping
#define SEL_1 46                 // Selector ping 1
#define SEL_2 47                 // Selector ping 2
#define SEL_3 48                 // Selector ping 3
#define SEL_4 49                 // Selector ping 4

//Temperature Sensors pin
#define TH_MIDDLEBED_1 A8                 // Temperature Sensor 2
#define TH_MIDDLEBED_2 A9                 // Temperature Sensor 3
#define TH_UPPERBED A10                   // Temperature Sensor 1
#define IDDLE_MIDDLE_TEMP 20
#define IDDLE_UPPER_TEMP 20
#define Threshold_temp 0.5
//Heater pins
#define HEAT_MIDDLE 2             // Heat 1
#define HEAT_UPPER 3            // Heat 2

//LED Sensors pin
#define PANEL_LED 22			        


// manufacturer data for episco NCP18XH103F03RB 10k thermistor
// simply delete this if you don't need it
// or use this idea to define your own thermistors
#define NCP18XH103F03RB 3380.0f,298.15f,10000.0f  // B,T0,R0


// STATES FOR STATE MACHINE
#define INIT       'I'
#define CANCEL       'C'
#define HEAT_BOARDS  'H'
#define INFO       'i'
#define SET_TEMP_UPPER 'U'
#define SET_TEMP_MIDDLE 'M'
#define READ_ASSAY 'R'

int MUL[6] = {
  A0,A1,A2,A3,A4,A5};				// Multiplexer  1



byte controlLPins[] = {
  B00000000, 
  B00000001,
  B00000010,
  B00000011,
  B00000100,
  B00000101,  
  B00000110,
  B00000111,
  B00001000,
  B00001001,
  B00001010,
  B00001011,
  B00001100,
  B00001101,
  B00001110,
  B00001111}; 


// holds incoming values from Multiplexer
int sensorValues[8][12] = {
};


//Define the aggressive and conservative Tuning Parameters
double aggKp=4, aggKi=0.4, aggKd=4; // d2
double consKp=1, consKi=0.05, consKd=0.25; //1, 0.25
double Setpoint;
//Define Variables we'll be connecting to PID's
double Input_MIDDLE, Output_MIDDLE, Setpoint_MIDDLE,Input_UPPER, Setpoint_UPPER, Output_UPPER,Input_MIDDLE_2;

//PID Controller for Heater
PID PID_MIDDLE(&Input_MIDDLE_2, &Output_MIDDLE, &Setpoint_MIDDLE, consKp, consKi, consKd, DIRECT);
PID PID_UPPER(&Input_UPPER, &Output_UPPER, &Setpoint_UPPER, consKp, consKi, consKd, DIRECT);

String id = "Launch";
String sessionID = "";

// enumerating 3 major temperature scales
enum {
  T_KELVIN=0,
  T_CELSIUS,
  T_FAHRENHEIT
};

byte states[] = {INIT, CANCEL, HEAT_BOARDS, INFO, SET_TEMP_UPPER, SET_TEMP_MIDDLE, READ_ASSAY};


// serial data
String inputString = "                         ";  // a string to hold incoming data
String inputString1 = "                         ";  // a string to hold incoming data
String parameters = "                         ";
String dataString = "                         ";
boolean stringComplete = false;  // whether the string is complete
boolean stringComplete1 = false;  // whether the string is complete                    
boolean LED=false;
boolean HEAT_ON = false;

int state = INIT;
char previous_state = INIT;
int defaultState = INIT;

// message to send via wifi
boolean wifiConnected = false;
String messageToRespond[] = {"","",""};
boolean messageSent = true;
boolean readyForSend = false;
String messageToSendSerial = "";
boolean messageCommandReady = false;
String ssid = "";
String pass = "";
String results = "";

String message = "";
String webService = "104.154.70.139";
String webServiceGate = "80";
boolean noMessage = true;
boolean webServiceFirst = false;
boolean connection1 = false;
boolean connection2 = false;
boolean connection3 = false;
boolean connection4 = false;
boolean connection5 = true;
boolean webServiceGotIP = false;
boolean errorNoMsg = false;
boolean errorNoConn = false;
boolean webSeriviceOn = false;

boolean resetNoConn = false;
boolean resetSSID = false;
boolean resetAP = true;
boolean resetRunOnce = true;
boolean reset1 = false;
boolean reset2 = false;
boolean reset2_1 = false;
boolean reset3 = false;
boolean reset4 = false;
boolean reset5 = false;
boolean reset6 = false;
boolean reset7 = false;
boolean reset8 = false;
boolean reset9 = false;
boolean reset10 = false;
boolean reset10_1 = false;
boolean reset10_2 = false;
boolean reset11 = false;
boolean reset12 = false;

boolean sendMaster = false;
boolean noConnMaster = false;

boolean APerrorDisplay = true;

int rst = 0;
int rstMast = 0;
int rstSwitch = 0;

String messageError = "No Error";

// The wait timer variables for wifi delay in response
unsigned long interval=2000;  // the time we need to wait
unsigned long previousMillis=0; // millis() returns an unsigned long.

//wait timer to establish connection if available
unsigned long intervalToWait=30000;  // the time we need to wait

//wait timer if no interaction even if connected to wifi from web service, reset AP
unsigned long intervalNoResponce=60000;  // the time we need to wait
unsigned long previousMillisNoResponce1=0; // millis() returns an unsigned long.'
unsigned long previousMillisNoResponce2=0; // millis() returns an unsigned long.

void setup ()
{
  DDRL = DDRL | B00001111;  // this sets pins D3 to D7 as outputs

  // Temperature Sensors pin
  pinMode(TH_MIDDLEBED_1, INPUT);
  pinMode(TH_MIDDLEBED_2, INPUT);
  pinMode(TH_UPPERBED, INPUT);

  // Heater pins
  pinMode(HEAT_MIDDLE, OUTPUT);
  pinMode(HEAT_UPPER, OUTPUT);

  //Panel LEDS
  pinMode(PANEL_LED,OUTPUT);


  // Read temperature sensors
  ReaderSensorTemp();

  // Set degrees for PID algorithm
  Setpoint_MIDDLE = IDDLE_MIDDLE_TEMP;
  Setpoint_UPPER = IDDLE_UPPER_TEMP;

  // input string needs space
  inputString.reserve(30);  
  inputString = "";
  inputString1.reserve(30); 
  inputString1 = "";
  parameters = "";

  Serial.begin(9600);  // Begin the serial monitor at 9600bps
  Serial.println(F("Hope your day is nice and shiny!"));
  // open wifi serial and reset it
  Serial1.begin(115200);
  Serial1.print("AT+RST\r\n");
  Serial.print("AT+RST\r\n");

}


void loop () {
  
  // write information about connecting to wifi through the serial port
  if(messageToSendSerial.length() != 0) {
    Serial.println(messageToSendSerial);
    messageToSendSerial = "";
  }
  
  // print the string when a newline arrives:
  if (stringComplete) {
    // remove carriage returns and new lines
    inputString.replace("\n", "");
    inputString.replace("\r", "");
    Serial.println(inputString);
    
    // write to wifi if begins with command "AT"
    if(inputString.substring(0,2) == "AT" || messageCommandReady) {
      inputString = inputString + "\r\n";
      Serial1.print(inputString);
      messageCommandReady = false;
    }
       
    // it there is anything to do with it, do it!
    if(isState(inputString[0])) {
      previous_state = state;
      state = inputString[0];
      parameters = inputString.substring(2);
    }
    // clear the string:

    inputString = "";
    stringComplete = false;
  }

  //Wait for 10 sec in order to see if there is a wifi to connect to
  if ((unsigned long)(millis()) >= intervalToWait && resetAP && resetRunOnce) {
    resetRunOnce = false;
    resetAP = false;
    reset1 = true;
  } else if ((unsigned long)(millis()) >= intervalToWait) {
    resetRunOnce = false;
  }

  // If there is no responce and the wifi is connected to an AP but there is no internet connection etc. reset the AP
  if ((unsigned long)(millis() - previousMillisNoResponce1) >= intervalNoResponce && webSeriviceOn) {
    previousMillisNoResponce1 = millis();
    
    Serial.println("No interaction in 60 sec");
    Serial.println(messageError);
    rstMast = rstMast +1;
                
    if(wifiConnected == false || rst == 2) {
      resetNoConn = true;
      rst = 0;
    } else if(wifiConnected && errorNoMsg) {  // reset the get message 3 times
      Serial.println("nomsg");
      connection4 = true;
      stringComplete1 = true; 
      webServiceFirst = true; 
      for(int i = 0; i<3;i++) {
        if(messageToRespond[i] != "") {
          webServiceFirst = false;
        } 
      }
      if(messageError != "No Error") {
        messageError = "Web service did not respond, perhaps a bad connection. Trying to connect to " + webService + ":" + webServiceGate + " Last message: " + messageError;
      } else if (rst == 0) {
        messageError = "Web service did not respond, perhaps a bad connection. Trying to connect to " + webService + ":" + webServiceGate;
      }
      rst = rst +1;
      errorNoMsg = false;
    } else if(wifiConnected && errorNoConn) {  // reset the get message 3 times
      Serial.println("noconn");
      webServiceGotIP = true;
      stringComplete1 = true; 
      webServiceFirst = true; 
      noConnMaster = true;
      sendMaster = true;
      for(int i = 0; i<3;i++) {
        if(messageToRespond[i] != "") {
          webServiceFirst = false;
        } 
      }
      errorNoConn = false;
      if(messageError != "No Error") {
        messageError = "No contact to the web service, see that it is accesible. Trying to connect to " + webService + ":" + webServiceGate + " Last message: " + messageError;
      } else if (rst == 0) {
        messageError = "No contact to the web service, see that it is accesible. Trying to connect to " + webService + ":" + webServiceGate;
      }
      rst = rst +1;
    }
  }

  if ((((unsigned long)(millis() - previousMillisNoResponce2) >= intervalNoResponce + 60000 && resetNoConn) || rstMast == 4) && webSeriviceOn) {
    rstMast = 0;
    previousMillisNoResponce2 = millis();
    reset1 = true;
    resetNoConn = false;
    APerrorDisplay = true;
    Serial.println("Set AP, cos no interaction to web service in 120 sec");
    // if no responce in 2 min, then that means that assay also gone, thus close heat
    //previous_state = state;
    //state = 'C';
  }


  // if reset and no IP to connect to, establish AP
  if(reset1) {
    wifiConnected = false;
    Serial1.print("AT+CWMODE=2\r\n");
    Serial.print("AT+CWMODE=2\r\n");
    reset1 = false;
    reset2_1 = true;
    webSeriviceOn = false;
  }
  
  // Nothing will change until millis() increments by 1000, so a wait time to receive the response from dongle is 1 seconds
  if ((unsigned long)(millis() - previousMillis) >= interval) {
    
    previousMillis = millis();
    
    // if also the stringComplete1 is true, then that means that there is something to print
    if (stringComplete1 || noConnMaster) {
      noConnMaster = false;
      resetNoConn = false;
      previousMillisNoResponce1 = millis();
      previousMillisNoResponce2 = millis();
      stringComplete1 = false;
      Serial.print(inputString1);

      //Just record if a responce from webservice the message as a potential error message to be displayed
      if(searchString(inputString1, "+IPD") && APerrorDisplay == false) {
        messageError = inputString1.substring(searchString1(inputString1, "IPD") + 4,100);
        messageError.replace("\\","");
        messageError.replace("/","");
      }

      
      // send fail
      if((searchString(inputString1, "SEND FAIL") && searchString(inputString1, "CLOSED")) || (searchString(inputString1, "SEND OK") && searchString(inputString1, "CLOSED"))) {
        sendMaster = true;
        connection2 = false;
      }

      // send fail
      if(searchString(inputString1, "ERROR") && searchString(inputString1, "CLOSED") && rstSwitch < 60) {
        sendMaster = true;
        connection2 = false;
        rstSwitch = rstSwitch + 1;
      }

      // link not valid
      if(searchString(inputString1, "link is not valid") && searchString(inputString1, "ERROR")) {
        
        connection1 = false;
        connection2 = false;
        connection3 = false;
        connection4 = true;
        connection5 = false;
      }

      // closed before send
      if(searchString(inputString1, "CLOSED") && connection1) {
        
        connection1 = false;
        connection2 = false;
        connection3 = false;
        connection4 = false;
        connection5 = false;
        sendMaster = true;
      }

      //**
      //** Establish wifi, either AP or server and connect
      //** 

      // set the name of the network
      if(searchString(inputString1, "+CWMODE=2")) {
        if(searchString(inputString1, "OK")) {
          if(reset2_1) {
            Serial1.print("AT+CWSAP=\"Miriam" + id + "\",\"Miriam" + id + "\",1,4\r\n");
            Serial.print("AT+CWSAP=\"Miriam" + id + "\",\"Miriam" + id + "\",1,4\r\n");
            reset2_1 = false;
            reset2 = true;
          }
        }
      }
      
      // after set DHCP to softAP mode and disable
      if(searchString(inputString1, "+CWSAP")) {
        if(searchString(inputString1, "OK")) {
          if(reset2) {
            Serial1.print("AT+CWDHCP=0,1\r\n");
            Serial.print("AT+CWDHCP=0,1\r\n");
            reset2 = false;
            reset3 = true;
          }
        }
      }

      // control that mode set to AP and send multiple connections allowed
      if(searchString(inputString1, "+CWDHCP=0,1")) {
        if(searchString(inputString1, "OK")) {
          if(reset3) {
            Serial1.print("AT+CIPMUX=1\r\n");
            Serial.print("AT+CIPMUX=1\r\n");
            reset3 = false;
            reset4 = true;
          }
        }
      }

      // control that multiple connections allowed, create server
      if(searchString(inputString1, "+CIPMUX=1")) {
        if(searchString(inputString1, "OK")) {
          if(reset4) {
            Serial1.print("AT+CIPSERVER=1,80\r\n");
            Serial.print("AT+CIPSERVER=1,80\r\n");
            reset4 = false;
            reset5 = true;
          }
        }
      }

      // control that server created and ready to take new ssid and pass to connect to
      if(searchString(inputString1, "+CIPSERVER=1,80")) {
        if(searchString(inputString1, "OK")) {
          if(reset5) {
            Serial.print("Ready for ssid and pass");
            reset5 = false;
            reset6 = true;
          }
        }
      }

      // message received from web server, read and start to connect to ssid +IPD initiates the string
      if(searchString(inputString1, "+IPD") && reset6) {
        int index1 = inputString1.indexOf('$');
        int index2 = inputString1.indexOf('$', index1 + 1);
        int index3 = inputString1.indexOf('$', index2 + 1);
        int index4 = inputString1.indexOf('$', index3 + 1);
        int index5 = inputString1.indexOf('$', index4 + 1);

        // if the correct amount of dollar signs
        if(index1 != -1 && index2 != -1 && index3 != -1 && index4 != -1 && index5 != -1) {
          messageError = "No Error";
          if (inputString1.substring(index1 + 1, index2) == "ssid") {
            ssid = inputString1.substring(index2 + 1, index3);
            Serial.println("New ssid: " + ssid);
          }
          if (inputString1.substring(index3 + 1, index4) == "pass") {
            pass = inputString1.substring(index4 + 1, index5);
            Serial.println("New pass: " + pass);
          }
          message = "Miriam\r\nSSID: " + ssid + "\r\nPASS: " + pass;
          int messageLength = message.length();
          Serial1.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
          Serial.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
        } else if(searchString(inputString1, "Error")) {
          messageError.replace("\n","");
          messageError.replace("\r","");
          message = messageError;
          messageError = "No Error";
          int messageLength = message.length();
          Serial1.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
          Serial.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
        } else {
          message = "Miriam\r\nCheck the message something wrong";
          int messageLength = message.length();
          Serial1.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
          Serial.print("AT+CIPSEND=0," + String(messageLength) + "\r\n");
        }
        
      }

      if(searchString(inputString1, ">") && reset6) {
        Serial1.print(message);
        Serial.print(message);
        // if failure in sending, reset6 remains true
        if(searchString(message, "SSID")) {
          reset6 = false;
          resetSSID = true;
        } else {
          resetSSID = false;
        }
        reset7 = true;
        message = "";
      }

      // after send, close the connection
      if(searchString(inputString1, "SEND OK")) {
        if(reset7) {
          Serial1.print("AT+CIPCLOSE=0\r\n");
          Serial.print("AT+CIPCLOSE=0\r\n");
          reset7 = false;
          reset8 = true;
        }
      }

      // close AP and start in station mode
      if(searchString(inputString1, "CLOSED") && reset8 && resetSSID) {
        if(searchString(inputString1, "OK")) {
          Serial1.print("AT+CWMODE=1\r\n");
          Serial.print("AT+CWMODE=1\r\n");
          reset8 = false;
          reset9 = true;
        }
      }

      // after set DHCP to station mode and disable
      if(searchString(inputString1, "+CWMODE=1")) {
        if(searchString(inputString1, "OK")) {
          if(reset9) {
            Serial1.print("AT+CWDHCP=1,1\r\n");
            Serial.print("AT+CWDHCP=1,1\r\n");
            reset9 = false;
            reset10_1 = true;
          }
        }
      }

      // after AP has been closed, cipserver has to close
      if(searchString(inputString1, "+CWDHCP=1,1")) {
        if(searchString(inputString1, "OK")) {
          if(reset10_1) {
            Serial1.print("AT+CIPSERVER=0\r\n");
            Serial.print("AT+CIPSERVER=0\r\n");
            reset10_1 = false;
            reset10_2 = true;
          }
        }
      }

      // set cipmux to 0
      if(searchString(inputString1, "+CIPSERVER=0")) {
        if(searchString(inputString1, "OK")) {
          if(reset10_2) {
            Serial1.print("AT+CIPMUX=0\r\n");
            Serial.print("AT+CIPMUX=0\r\n");
            reset10_2 = false;
            reset10 = true;
          }
        }
      }
      
      // after AP has been closed, connect to given ssid
      if(searchString(inputString1, "+CIPMUX=0")) {
        if(searchString(inputString1, "OK")) {
          if(reset10) {
            Serial1.print("AT+CWJAP=\"" + ssid + "\",\"" + pass + "\"\r\n");
            Serial.print("AT+CWJAP=\"" + ssid + "\",\"" + pass + "\"\r\n");
            reset10 = false;
            reset11 = true;
          }
        }
      }

      // once connected register that ready for duty
      if(searchString(inputString1, "WIFI GOT IP")) {
        Serial.println("Wifi got ip");
        reset11 = true;
        reset12 = true;
        resetAP = false;
        wifiConnected = true;
        webServiceFirst = true;
        webServiceGotIP = true;
        APerrorDisplay = false;
        webSeriviceOn = true;
      }

      // if wifi is disconnected then establish AP.
      // not to be read if first reset
      if(searchString(inputString1, "WIFI DISCONNECT") && resetRunOnce == false) {
        Serial.println("Wifi disconnected");
        reset11 = false;
        reset12 = false;
        resetAP = true;
        wifiConnected = false;
        reset1 = true;
      }
      
      // If connections fails re-establish softAP
      // not yet leads to anywhere, plus not 100% sure about the "Fail"
      if(searchString(inputString1, "FAIL")) {
        if(reset12) {
          Serial.println("Wifi failed to connect, re-establish AP");
          reset11 = false;
          reset12 = false;
          resetAP = true;
          reset1 = true;
          wifiConnected = false;
        }
      }

      //**
      //** Send messages
      //**
      
      if((wifiConnected && connection5 && searchString(inputString1, "CLOSED") && searchString(inputString1, "OK")) || 
          (wifiConnected && webServiceGotIP && webServiceFirst) || sendMaster) {
        reset12 = false;
        Serial.println("AT+CIPSTART=\"TCP\",\"" + webService + "\"," + webServiceGate + "\r\n");
        Serial1.println("AT+CIPSTART=\"TCP\",\"" + webService + "\"," + webServiceGate + "\r\n");
        connection1 = true;
        connection5 = false;
        webServiceGotIP = false;
        errorNoConn = true;
        sendMaster = false;
      }

      if((wifiConnected && searchString(inputString1, "CONNECT") && searchString(inputString1, "OK") && connection1 && searchString(inputString1, "CLOSED") == false)) {
        errorNoConn = false;
        
        //if first contact to web service request sessionID
        if(webServiceFirst) {
          String msg = "GET /?message=Miriam$" + id + "$ready$ HTTP/1.1\r\nHost: " + webService + "\r\n\r\n";
          Serial.println("AT+CIPSEND=" + String(msg.length()) + "\r\n");
          Serial1.println("AT+CIPSEND=" + String(msg.length()) + "\r\n");

          messageToRespond[0] = msg;
          connection1 = false;
          connection2 = true;

          
        } else if(webServiceFirst == false) {
          for(int i = 0; i<3; i++) {
            if(messageToRespond[i] != "") {
              //if repeat try then do not add the get info
              if(searchString(messageToRespond[i], "GET /?message=") == false) {
                messageToRespond[i] = "GET /?message=" + messageToRespond[i] + " HTTP/1.1\r\nHost: " + webService + "\r\n\r\n";               
              }
              String msg = messageToRespond[i];
              Serial.println("AT+CIPSEND=" + String(msg.length()) + "\r\n");
              Serial1.println("AT+CIPSEND=" + String(msg.length()) + "\r\n");
              connection1 = false;
              connection2 = true;
              break;
            }
          }
        }
      }

      if(wifiConnected && searchString(inputString1, ">") && connection2) {
        for(int i = 0; i<3; i++) {
          if(messageToRespond[i] != "") {
            Serial.println(messageToRespond[i]);
            Serial1.println(messageToRespond[i]);
            webServiceFirst = false;
            connection2 = false;
            connection3 = true;
            errorNoMsg = true;
            break;
          }
        }
      }


      //** 
      //** Receive information
      //**

      if(wifiConnected && searchString(inputString1, "+IPD") && searchString(inputString1, "command") && connection3) {
        errorNoMsg = false;
        messageError = "No Error";

        rst = 0; 
        rstMast = 0;
        // only delete the message sent if responce is gotten, if not then retry the send
        for(int i = 0; i<3; i++) {
          if(messageToRespond[i] != "") {
            messageToRespond[i] = "";
            break;
          }
        }
        
        int index1 = inputString1.indexOf('$');
        int index2 = inputString1.indexOf('$', index1 + 1);
        int index3 = inputString1.indexOf('$', index2 + 1);

        // if correct amount of '$'
        if(index1 != -1 && index2 != -1 && index3 != -1) {
          // check that a command
          if(searchString(inputString1, "command")) {
             // get new sessionID
             if(inputString1.substring(index1+1,index2) == "sessionID") {
               // if not a new sessionID no change, if empty or different reset heat
               if(sessionID != inputString1.substring(index2+1,index3)) {
                sessionID = inputString1.substring(index2+1,index3);
                HEAT_ON = false;
               }
               messageToRespond[0] = sessionID + "$sessionID$" + sessionID + "$";
             }

             // if reset command D
             if(inputString1.substring(index1+1,index2) == "D") {
               boolean found = false;
               for(int i = 0; i<3; i++) {
                 if(messageToRespond[i] != "") found = true;
               }
               if(found == false) {
                messageToRespond[0] = sessionID + "$ready$$";
               }
             }

             // if state handle as one
             if(isState(inputString1.substring(index1+1,index2)[0])) {
               previous_state = state;
               state = inputString1.substring(index1+1,index2)[0];
               parameters = inputString1.substring(index2+1,index3);
               //messageToRespond[0] = sessionID + "$" + inputString1.substring(index1+1,index2) + "$" + inputString1.substring(index2+1,index3) + "$";
             }
          } else {
            Serial.println("Not a command");
          }
        } else {
          Serial.println("Command had wrong amount of '$'");
        }
        connection4 = true;
        connection3 = false;
      }

      //**
      //** Close connection
      //**
      
      if(connection4) {
        Serial.println("AT+CIPCLOSE\r\n");
        Serial1.println("AT+CIPCLOSE\r\n");
        connection4 = false;
        connection5 = true;
        rstSwitch = 0;
      }
      
      inputString1 = "";
    }

  }
  
  String responce = "";
  
  //Calculate PID for heaters
  if (HEAT_ON) Calculate_Heat();
  //Calculate_Heat();
  // state machine
  switch (state) {

  case INIT:
    previous_state = state;
    state = 'I';
    break;

  case CANCEL:

    HEAT_ON = false;
    
    Output_MIDDLE = 0;
    Output_UPPER = 0;
    computePIDs();
    
    Serial.println(sessionID + "$C$$");
    if(wifiConnected) messageToRespond[0] = sessionID + "$C$$";
    //delay(4000);

    //turn the PID's off
    //PID_MIDDLE.SetMode(MANUAL);
    //PID_UPPER.SetMode(MANUAL);
    
    Setpoint_MIDDLE = IDDLE_MIDDLE_TEMP;    
    Setpoint_UPPER = IDDLE_UPPER_TEMP;
    
    state = defaultState; 
    
    break;

  case HEAT_BOARDS:  
    
    HEAT_ON = true;
      
    Serial.println(sessionID + "$H$$");
    
    if(wifiConnected) messageToRespond[0] = sessionID + "$H$$";
  
    //turn the PID's on
    PID_MIDDLE.SetMode(AUTOMATIC);
    PID_UPPER.SetMode(AUTOMATIC);
    
    state = defaultState;
    break;

  case INFO:

    responce = sessionID + "$i$" + String(Setpoint_UPPER) + "," + Temperature(TH_UPPERBED,T_CELSIUS,NCP18XH103F03RB,10000.0f) + "," +
      String(Setpoint_MIDDLE * 0.6548 + 17.931) + "," + (Temperature(TH_MIDDLEBED_2,T_CELSIUS,NCP18XH103F03RB,10000.0f) * 0.6548 + 17.931) + "$";

    Serial.println(responce); 

    if(wifiConnected) {
      messageToRespond[0] = responce;
    }
    
    state = defaultState;

    break;

  case SET_TEMP_UPPER:

    Setpoint_UPPER = parameters.toInt();
    
    responce = sessionID + "$U$" + String(Setpoint_UPPER) + "$";
  
    Serial.println(responce);
    
    if(wifiConnected) messageToRespond[0] = responce;
    
    state = defaultState;
    break;

  case SET_TEMP_MIDDLE:

    Setpoint_MIDDLE = (parameters.toInt() - 17.931) / 0.6548;  
    
    responce = sessionID + "$M$" + String(Setpoint_MIDDLE * 0.6548 + 17.931) + "$";
    
    Serial.println(responce);
    
    if(wifiConnected) messageToRespond[0] = responce;
    
    state = defaultState;

    break;
    
  case READ_ASSAY:
        
    Read_Assay();


    int num = 0;

    int index1 = 0;
    int index2 = results.indexOf(',');

    // Only 32 measurements per message
    for(int j = 0; j<3; j++) {
      String res = "";
      if(j==0) {
        res = res + results.substring(0,index2) + ",";
      } else {
        res = res + results.substring(index1+1,index2) + ",";
      }
      index1 = index2;
      index2 = results.indexOf(',', index1+1);
      
      for(int i = 0; i<31; i++) {
        res = res + results.substring(index1+1,index2) + ",";
        //Serial.println(res);
        index1 = index2;
        index2 = results.indexOf(',', index1+1);
      }
      
      messageToRespond[j] = sessionID + "$R$" + res + "$";

      num = num + 32;
    }
    Serial.println(results);
    Serial.println(messageToRespond[0]);
    Serial.println(messageToRespond[1]);
    Serial.println(messageToRespond[2]);

     // if wifi not connected clear send buffer
    if(!wifiConnected) {
      messageToRespond[0] = "";
      messageToRespond[1] = "";
      messageToRespond[2] = "";
    }
  
    state = defaultState;
    break;

  }
}

void setPin(int outputPin) {
  PORTL = controlLPins[outputPin];
}

void Read_Assay() {

  //set the PIDs to zero to boost LED power
  Output_MIDDLE = 0;
  Output_UPPER = 0;
  computePIDs();
  
  digitalWrite(22,HIGH);

  //take 5 samples for measurement
  for (int j=0 ; j<5 ;j++)
  {
    for (int i = 0; i < 16; i++)
    {
      setPin(i); // choose an input pin
      ReadAssay(i);

    }    
    delay(1000);
  }


  digitalWrite(22,LOW);


  //Calculate the average of the measurements for each sample
  for (int k=0; k< 8;k++)
  {
    for (int l = 0;l < 12;l++)
    {
      sensorValues[k][l] /= 5;
    }
  }

  int j=0;
  results = "";

  for (j=0; j < 8; j++)
  {

    for (int i = 0; i < 12; i++)
    {
      results = results + sensorValues[j][i] + ",";
    } 
  }

  
  //Empty the array
  for (int k=0; k< 8;k++)
  {
    for (int l = 0;l < 12;l++)
    {
      sensorValues[k][l] = 0;
    }
  }
  
  //Empty possibly stored charge
  //take 5 samples for measurement
  for (int j=0 ; j<15 ;j++)
  {
    for (int i = 0; i < 16; i++)
    {
      setPin(i); // choose an input pin
      
      for (int k=0; k<5; k++) {
        
        ReadAssayEmpty(i);

      }
    }    
    delay(10);
  }  
}

void ReadAssayEmpty(int i) {

  for (int j = 0; j < 6; j++)
  {

    switch (i) {

    case 0:
      analogRead(MUL[j]);
      break;
    case 1:
      analogRead(MUL[j]);
      break;
    case 2:
      analogRead(MUL[j]);
      break;
    case 3:
      analogRead(MUL[j]);
      break;
    case 4:
      analogRead(MUL[j]);
      break;
    case 5:
      analogRead(MUL[j]);
      break;
    case 6:
      analogRead(MUL[j]);
      break;
    case 7:
      analogRead(MUL[j]);
      break;
    case 8:
      analogRead(MUL[j]);
      break;
    case 9:
      analogRead(MUL[j]);
      break;
    case 10:
      analogRead(MUL[j]);
      break;
    case 11:
      analogRead(MUL[j]);
      break;
    case 12:
      analogRead(MUL[j]);
      break;
    case 13:
      analogRead(MUL[j]);
      break;
    case 14:
      analogRead(MUL[j]);
      break;
    case 15:
      analogRead(MUL[j]);
      break;
    }
  }
}

void ReadAssay(int i) {

  for (int j = 0; j < 6; j++)
  {

    switch (i) {

    case 0:
      sensorValues[3][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 1:
      sensorValues[2][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 2:
      sensorValues[1][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 3:
      sensorValues[0][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 4:
      sensorValues[3][j*2]+= analogRead(MUL[j]);
      break;
    case 5:
      sensorValues[2][j*2]+= analogRead(MUL[j]);
      break;
    case 6:
      sensorValues[1][j*2]+= analogRead(MUL[j]);
      break;
    case 7:
      sensorValues[0][j*2]+= analogRead(MUL[j]);
      break;
    case 8:
      sensorValues[4][j*2]+= analogRead(MUL[j]);
      break;
    case 9:
      sensorValues[5][j*2]+= analogRead(MUL[j]);
      break;
    case 10:
      sensorValues[6][j*2]+= analogRead(MUL[j]);
      break;
    case 11:
      sensorValues[7][j*2]+= analogRead(MUL[j]);
      break;
    case 12:
      sensorValues[4][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 13:
      sensorValues[5][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 14:
      sensorValues[6][(j*2)+1]+= analogRead(MUL[j]);
      break;
    case 15:
      sensorValues[7][(j*2)+1]+= analogRead(MUL[j]);
      break;
    }

  }
}

void Calculate_Heat() {

  ReaderSensorTemp();
  SetTunings_PID();
  computePIDs();    


}    

void computePIDs() {

  PID_MIDDLE.Compute();
  PID_UPPER.Compute();

  analogWrite(HEAT_MIDDLE,Output_MIDDLE);
  analogWrite(HEAT_UPPER,Output_UPPER);
  /*
  
   analogWrite(HEAT_MIDDLE,0);
   analogWrite(HEAT_UPPER,0);
   */
}


void ReaderSensorTemp() {

  Input_UPPER = Temperature(TH_UPPERBED,T_CELSIUS,NCP18XH103F03RB,10000.0f);
  //Input_MIDDLE = Temperature(TH_MIDDLEBED_1,T_CELSIUS,NCP18XH103F03RB,10000.0f);
  Input_MIDDLE_2 = Temperature(TH_MIDDLEBED_2,T_CELSIUS,NCP18XH103F03RB,10000.0f);

}

void SetTunings_PID() {

  double gap1,gap2;

  gap1 = abs(Setpoint_MIDDLE - Temperature(TH_MIDDLEBED_2,T_CELSIUS,NCP18XH103F03RB,10000.0f)); //distance away from setpoint
  gap2 = abs(Setpoint_UPPER - Temperature(TH_UPPERBED,T_CELSIUS,NCP18XH103F03RB,10000.0f)); //distance away from setpoint


  if(Temperature(TH_MIDDLEBED_2,T_CELSIUS,NCP18XH103F03RB,10000.0f)>Setpoint_MIDDLE)
  {  //we're close to setpoint, use conservative tuning parameters
    PID_MIDDLE.SetTunings(consKp, consKi, consKd);
    Output_MIDDLE = 0;
  }
  else
  {
    //we're far from setpoint, use aggressive tuning parameters
    PID_MIDDLE.SetTunings(aggKp, aggKi, aggKd);
  }

  if(Setpoint_UPPER<Temperature(TH_UPPERBED,T_CELSIUS,NCP18XH103F03RB,10000.0f))
  {  //we're close to setpoint, use conservative tuning parameters
    PID_UPPER.SetTunings(consKp, consKi, consKd);
    Output_UPPER = 0;
  }
  else
  {
    //we're far from setpoint, use aggressive tuning parameters
    PID_UPPER.SetTunings(aggKp, aggKi, aggKd);
  }
}

// check if it is a valid state for the state machine
boolean isState(int s) {
  boolean r = false;
  for(int i = 0; i < sizeof(states); i++) {
    r = (states[i] == s);
    if(r) return r;
  }
  return r;
}

float Temperature(int AnalogInputNumber,int OutputUnit,float B,float T0,float R0,float R_Balance) {
  float R,T;

  //  R=1024.0f*R_Balance/float(analogRead(AnalogInputNumber)))-R_Balance;
  R=R_Balance/(1024.0f/float(analogRead(AnalogInputNumber))-1);

  T=1.0f/(1.0f/T0+(1.0f/B)*log(R/R0));

  switch(OutputUnit) {
  case T_CELSIUS :
    T-=273.15f;
    break;
  case T_FAHRENHEIT :
    T=9.0f*(T-273.15f)/5.0f+32.0f;
    break;
  default:
    break;
  };
  return T;
}

boolean searchString(String str, String search) {
  for (int i=0; i<str.length(); i++) {
    if (str.substring(i,i+search.length()) == search) return true;
  }
  return false;
}

int searchString1(String str, String search) {
  for (int i=0; i<str.length(); i++) {
    if (str.substring(i,i+search.length()) == search) return i;
  }
  return -1;
}


// serial event
void serialEvent() {
  if (Serial.available()) {

    while (Serial.available()) {
      // get the new byte:
      char inChar = (char)Serial.read();
      // add it to the inputString:
      inputString += inChar;
      // if the incoming character is a newline, set a flag
      // so the main loop can do something about it:
      if (inChar == '\n') {
        stringComplete = true;
      }
    }
  }
  
}

// serial event for bluetooth
void serialEvent1() {
  if (Serial1.available()) {
    // Always when something has been received from Serial1 something is to be printed in the loop
    stringComplete1 = true;
    while (Serial1.available()) {
      // get the new byte:
      char inChar = (char)Serial1.read();
      // add it to the inputString:
      inputString1 += inChar;
    }
  }
}
















