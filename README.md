# LogicSnifferSTM
STM32 BluePill-based logic analyzer with OneWire protocol parser (and extendable)

Hadrware capabilities:

 - Acquisition: 1MSa/s, i.e. maximum signal frequency is 500kHz and resolution is 1uS, which is fine for OneWire. Can be improved using ARM inline assembler for aquisition code.
 - Trigger: technically, any edge, but see Memory.
 - Accuracy: depends on quality of the crystal oscillator/resonator used on the board. For debugging OneWire any BluePill is okay, however for precision applications calibration against a known good frequency source is essential.
 - Memory: 2560 edges (default). The instument stores waveforms as edges (uint32 timestamp + bool edge sign). Bitwise operations are avoided for the sake of acquisition speed. You could push some more edges in (currently only about 3/4 of RAM is reserved for storage). The memory is, of course, volatile, therefore you can't power the device down until you dump the data to a PC.

Hardware requirements:
 - STM32 BluePill + battery + a button + a 100k resistor (optional, intended to pull the input down to avoid capturing noise).

Special features:
 - Memory can be split into several parts that can be filled sequentially, see below.
 - Acquisition status is indicated by the builtin LED.
 - Acquisition is started by pressing the (single) "start aquisition" button.
 - Acquisition is stopped either when the memory is full or by pressing the button.

The instrument was designed for field operation where anything larger and more complicated than a BluePill (i.e. laptop) is not an option. The intended usage sequence is: power the device on, press "start acquisition" button, connect to the network of interest. If the network is alive, then the memory will get filled up soon and the LED will turn off, indicating that the acquisition has ended. If not, just press the button to stop acquisition manually.

When memory is split into several banks, they are used sequentially, i.e. press the button -> recording into the first bank -> first bank filled/manual stop -> press the button -> recording into the second bank -> etc.

Software capabilities:

 - Dumping the captures to the PC
 - Plotting raw and parsed data
 - Parsing OneWire protocol
 - New parsers can be added easilly
 - Parsed info output is also designed to be human-readable
 - Load simulation data from Proteus ISIS (by Labcenter Electronics)
 
Software requirements: .NET 4.5.2, VisualMicro visual studio plugin with STM32 core installed.

Projects in the solution:
 - DataConsoleHelper = OneWire parser (it's a command-line app, but DataPlotter provides some GUI for it)
 - DataPlotter = Main app, dumps data from the device, plots it and invokes parsers
 - LogicSnifferSTM = firmware
 - ProteusDataConverter = converts Proteus ISIS simulation data (exported) into LogicSnifferSTM format
 - SquareWaveGen = quick and dirty test for system bandwidth (arduino as a crude freqency generator)
 - TimerTest = some stuff for debug purposes
 
Examples:
https://imgur.com/a/sn2cK5r
