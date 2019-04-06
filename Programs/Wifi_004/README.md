Wifi_004
========

This SW / FW combination is meant for communication over webservice. If Miriam is not already connected to a WIFI, it creates it's own WIFI. The user connects to Miriam WIFI and sends a message using web browser 'http://192.168.4.1/$ssid$ssidName$pass$passPass$. Miriam turns WIFI network off and connects to the WIFI network it was given. It starts to communicate with Miroculus webservice called mirbeat, the user can find Miriam connected from the mirbeat (Please note that the FW contains the name used, so be sure to use unique name to find MIriam in question).

Please see the API.pdf for more information about the two way communication process between mirbeat and Miriam.