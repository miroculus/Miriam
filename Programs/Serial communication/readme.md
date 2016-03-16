These SW and FW are meant for serial communication between the computer and Miriam.

The communication happens via string commands between Arduino Mega and computer.
- R (read assay and return results)
- U param (set the upper temperature to wanted, i.e. U 80)
- M param (set the middle temperature to wanted, i.e. M 63)
- H (put PID controlled temperature on)
- i (info, return the current temperature)

The temperature in the middle is adjusted so that the upper temperature is 80 degrees C