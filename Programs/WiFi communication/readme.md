The Wifi_004 is meant for communication between online web service (WS) and controlling Miriam trhough this.

Miriam first creates an access point (AP) where an external device can connect to. From this AP it is possible to ask for errors by using a browser and sending message

192.168.4.1/Error

The Miriam can connect to an existing wifi in order to receive commands and send results. This can be done by sending a message in browser

192.168.4.1/$ssid$nameOfssid$pass$ssidpass$

If at any point there are persistent problems in the communication, Miriam creates an AP and the user can ask for the error with the first command.

More extensive description can be found from the API.pdf