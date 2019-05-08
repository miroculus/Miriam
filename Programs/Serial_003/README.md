Serial_003
========

This SW / FW combination is meant for serial communication between a computer and Miriam. Miriam is connected with a USB cable to the controlling computer and messaging between the two happen via string messages over RX/TX protocol.
- SW 'R' (read assay) - FW 'A1,A2,A3...E12'
- SW 'i' (info) - FW 'outputMiddle, outputUpper, temperatureMiddle, temperatureUpper, temperatureMiddleC, temperatureUpperC'
- SW 'U param' (upper wanted temperature, i.e. U 80) - FW 'temperatureUpperSet'
- SW 'M param' (middle wanted temperature, i.e. M 63) - FW 'temperatureMiddleSet'
- SW 'C' (cancel, turn off the heat) - FW 'Cancel'

Once the temperature has been set, the actual temperature needs to be refreshed by clicking 'Ask heat'. The assay, once started, can always be turned off simply by closing the program. The assay results are saved to the desktop in a .csv file.